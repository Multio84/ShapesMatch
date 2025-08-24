using System;
using UnityEngine;
using UnityEngine.EventSystems;


public interface IChipPhysics
{
    Collider2D Collider { get; }
    void Init(GameSettings settings);
    void Enable(bool value);
    event Action InteralFallingStopped;
}

public interface IChipClickable { event Action InteralClicked; }

// управляет Rigidbody2D/Collider2D и даёт события InteralFallingStopped/InteralClicked.  
public class ChipPhysics : MonoBehaviour, 
    IChipPhysics,
    IChipStateMediator,
    IChipClickable, 
    IPointerDownHandler
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;

    //public bool IsInteractable { get; set; } = false;
    private bool isInteractable = false;

    public Collider2D Collider => col;

    public event Action InteralFallingStopped;
    public event Action InteralClicked;
    public event Action<ChipState> StateProduced;

    private float prevSpeed;    // save speed to detect if chip slowed down
    private float maxXImpulse;  // max impulse value to shift on X axis when being emitted
    private float checkDelay;

    public void Init(GameSettings settings)
    {
        if (!rb || !col)
        {
            Debug.LogError("ChipPhysics: Rigidbody or Collider not found.");
            return;
        }

        maxXImpulse = settings.chipMaxStartImpulse;
        checkDelay = settings.chipStopCheckDelay;
    }

    public void Enable(bool value) => rb.simulated = value;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInteractable) return;
        InteralClicked?.Invoke();
    }

    /* ---- IChipStateMediator ---- */
    public void ApplyState(ChipState state)
    {
        switch (state)
        {
            case ChipState.Emitting:
                isInteractable = false;
                Enable(true);
                AddRandomHorizontalImpulse();
                StartCheckIfStopped();
                break;
            case ChipState.Idle:
                // the chip had fallen and is lying, physics stays enabled
                isInteractable = true;
                break;
            case ChipState.Moving:
                Enable(false);
                isInteractable = false;
                break;
            case ChipState.Falling:
                Enable(true);
                isInteractable = false;
                break;
            case ChipState.Freezed:
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                Enable(false);
                break;
            case ChipState.None:
                break;
            default:
                Debug.LogWarning($"Unhandled state: {state}");
                break;
        }
    }

    private void AddRandomHorizontalImpulse()
    {
        float imp = UnityEngine.Random.Range(-maxXImpulse, maxXImpulse);
        rb.AddForce(new Vector2(imp, 0f), ForceMode2D.Impulse);
    }

    private void StartCheckIfStopped()
    {
        prevSpeed = 0;
        InvokeRepeating(nameof(CheckIfStopped), checkDelay, checkDelay);
    }

    private void CheckIfStopped()
    {
        float curSpeed = rb.velocity.sqrMagnitude;
        if (curSpeed < prevSpeed)
        {
            CancelInvoke(nameof(CheckIfStopped));
            StateProduced?.Invoke(ChipState.Idle);   // -> Idle
            InteralFallingStopped?.Invoke();
            return;
        }
        prevSpeed = curSpeed;
    }

    // a variant of stopping detection
    //IEnumerator Start()
    //{
    //    float eps = .05f;
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(.2f);
    //        if (rb.IsSleeping() || rb.velocity.sqrMagnitude < eps)
    //        {
    //            InteralFallingStopped?.Invoke();
    //            yield break;
    //        }
    //    }
    //}
}
