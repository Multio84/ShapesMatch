using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


public struct ChipPassport
{
    public int prefabIdx;
    public int colorIdx;
    public int animalIdx;

    public ChipPassport(int prefab, int color, int animal)
    {
        prefabIdx = prefab;
        colorIdx = color;
        animalIdx = animal;
    }

    public bool IsSameAs(ChipPassport other)
    {
        return prefabIdx == other.prefabIdx &&
               colorIdx == other.colorIdx &&
               animalIdx == other.animalIdx;
    }
}

public class Chip : MonoBehaviour, IPointerDownHandler
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private SpriteRenderer animalRenderer;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;
    private GameSettings settings;
    private GameplayManager gameplayManager;
    private ActionBar actionBar;
    private ChipPassport passport;
    public ChipPassport Passport => passport;

    private Tween move;
    private Tween rotate;
    private Tween death;

    private float flyDuration;
    private float deathDuration;
    private float checkDelay;
    private float prevSpeed = 0;
    public bool isInteractable = false;

    public event Action<Chip> Stopped;
    public event Action<Chip> SentToActionBar;
    public event Action Placed;
    public event Action<Chip> DeathCompleted;


    void OnDisable()
    {
        SentToActionBar -= gameplayManager.OnChipSentToActionBar;
        Placed -= gameplayManager.OnChipPlaced;
    }

    public void Init(
        GameSettings gs, 
        GameplayManager gm,
        ActionBar ab, 
        ChipPassport p, 
        ChipPartsDatabase db
        )
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
        if (rb is null || col is null)
        {
            Debug.LogError("RigidBody2D or Collider2D wasn't found in a chip.");
            return;
        }

        settings = gs;
        gameplayManager = gm;
        actionBar = ab;
        passport = p;

        flyDuration = settings.chipFlyDuration;
        deathDuration = settings.chipDeathDuration;
        checkDelay = settings.chipStopCheckDelay;

        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);

        SentToActionBar += gameplayManager.OnChipSentToActionBar;
        Placed += gameplayManager.OnChipPlaced;

        StartCheckIfStopped();
    }

    public void StartCheckIfStopped()
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
            Stopped?.Invoke(this);
            
            return;
        }

        prevSpeed = curSpeed;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isInteractable)
        {
            int idx = actionBar.GetNextAvailableSlot();
            if (idx < 0) return;

            isInteractable = false;

            SendToActionBar(idx);
        }
    }

    public void SendToActionBar(int idx)
    {
        rb.simulated = false;
        ChangeLayerOrder(true);

        SentToActionBar?.Invoke(this);
        actionBar.ReserveSlot(idx, this);

        Rotate(actionBar.GetSlotTransform(idx));
        Move(actionBar.GetSlotTransform(idx), flyDuration).
            OnComplete(() => ChipArrivedToActionBar(idx));
    }

    public void ChangeLayerOrder(bool isOverActionBar)
    {
        int actionBarOrder = actionBar.spriteRenderer.sortingOrder;

        if (isOverActionBar)
        {
            frameRenderer.sortingOrder += actionBarOrder;
            animalRenderer.sortingOrder += actionBarOrder;
        }
        else
        {
            frameRenderer.sortingOrder -= actionBarOrder;
            animalRenderer.sortingOrder -= actionBarOrder;
        }
    }

    private void ChipArrivedToActionBar(int idx)
    {
        actionBar.PlaceChip(idx, this);
        Placed?.Invoke();
    }

    public Tween Move(Transform targetTransform, float duration)
    {
        return move = transform
            .DOMove(targetTransform.position, duration)
            .SetEase(Ease.InOutQuad);
    }

    public void Rotate(Transform targetTransform)
    {
        Vector3 targetZVector = new Vector3(0, 0, targetTransform.rotation.eulerAngles.z);
        rotate = transform
            .DORotate(targetZVector, flyDuration)
            .SetEase(Ease.InOutQuad);
    }

    public void Die()
    {
        death = transform
            .DOScale(Vector3.zero, deathDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => {
                KillAllTweens();
                DeathCompleted(this);
                });
    }

    public void KillAllTweens()
    {
        move.Kill(false);
        rotate.Kill(false);
        death.Kill(false);
    }

    public void DropFromActionBar()
    {
        KillAllTweens();
        rb.simulated = true;
    }
}