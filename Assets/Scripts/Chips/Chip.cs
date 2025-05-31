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
    private bool isInteractable = true;
    private float flyDuration;
    private float deathDuration;

    private ChipPassport passport;
    private bool hasStopped = false;
    private float stopThreshold;
    private float checkDelay;

    public event Action<Chip> Stopped;
    public event Action<Chip> SentToActionBar;
    public event Action Placed;
    public event Action<Chip> DeathCompleted;


    public ChipPassport Passport => passport;

    void OnDisable()
    {
        Placed -= gameplayManager.OnChipPlaced;
    }

    public void Init(GameSettings gs, GameplayManager gm, ActionBar ab, ChipPassport p, ChipPartsDatabase db)
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
        stopThreshold = settings.chipStopThreshold;
        checkDelay = settings.chipStopCheckDelay;

        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);

        SentToActionBar += gameplayManager.OnChipSentToActionBar;
        Placed += gameplayManager.OnChipPlaced;

        StartCheckIfStopped();
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

    private void StartCheckIfStopped() => 
        InvokeRepeating(nameof(CheckIfStopped), checkDelay, checkDelay);
    
    private void CheckIfStopped()
    {
        if (hasStopped) return;

        bool isMoving = rb.velocity.sqrMagnitude > stopThreshold * stopThreshold;
        bool isRotating = Mathf.Abs(rb.angularVelocity) > stopThreshold;

        if (!isMoving && !isRotating)
        {
            hasStopped = true;
            Stopped?.Invoke(this);
            CancelInvoke(nameof(CheckIfStopped)); // больше не проверяем
        }
    }

    public void SendToActionBar(int idx)
    {
        rb.simulated = false;
        col.enabled = false;
        PlaceOverActionBar();

        SentToActionBar?.Invoke(this);

        actionBar.ReserveSlot(idx, this);

        Rotate(actionBar.GetSlotTransform(idx));
        Move(actionBar.GetSlotTransform(idx), flyDuration).
            OnComplete(() => ChipArrivedToActionBar(idx));
    }

    private void PlaceOverActionBar()
    {
        int actionBarOrder = actionBar.spriteRenderer.sortingOrder;

        frameRenderer.sortingOrder += actionBarOrder;
        animalRenderer.sortingOrder += actionBarOrder;
    }

    private void ChipArrivedToActionBar(int idx)
    {
        actionBar.PlaceChip(idx, this);
        Placed?.Invoke();
    }

    public Tween Move(Transform targetTransform, float duration)
    {
        return transform
            .DOMove(targetTransform.position, duration)
            .SetEase(Ease.InOutQuad);
    }

    public void Rotate(Transform targetTransform)
    {
        Vector3 targetZVector = new Vector3(0, 0, targetTransform.rotation.eulerAngles.z);

        transform
            .DORotate(targetZVector, flyDuration)
            .SetEase(Ease.InOutQuad);
    }

    public void Die()
    {
        transform
            .DOScale(Vector3.zero, deathDuration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => DeathCompleted(this));
    }
}