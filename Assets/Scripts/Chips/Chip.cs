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
    private GameplayManager gameplayManager;
    private ActionBar actionBar;
    private bool isInteractable = true;
    private const float FLY_DURATION = 0.5f;
    private const float DEATH_DURATION = 0.25f;

    private ChipPassport passport;
    public bool isMatched = false;

    public event Action<Chip> ChipSentToActionBar;
    public event Action ChipPlaced;
    public event Action<Chip> DeathCompleted;


    public ChipPassport Passport => passport;

    void OnDisable()
    {
        ChipPlaced -= gameplayManager.OnChipPlaced;
    }

    public void Init(GameplayManager gm, ActionBar ab, ChipPassport p, ChipPartsDatabase db)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
        if (rb is null || col is null)
        {
            Debug.LogError("RigidBody2D or Collider2D wasn't found in a chip.");
            return;
        }

        gameplayManager = gm;
        actionBar = ab;
        passport = p;

        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);

        ChipSentToActionBar += gameplayManager.OnChipSentToActionBar;
        ChipPlaced += gameplayManager.OnChipPlaced;
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
        col.enabled = false;
        PlaceOverActionBar();

        ChipSentToActionBar?.Invoke(this);

        actionBar.ReserveSlot(idx, this);

        Rotate(actionBar.GetSlotTransform(idx));
        Move(actionBar.GetSlotTransform(idx), FLY_DURATION).
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
        ChipPlaced?.Invoke();
    }

    public Tween Move(Transform targetTransform, float duration)
    {
        return transform.DOMove(targetTransform.position, duration)
            .SetEase(Ease.InOutQuad);
    }

    public void Rotate(Transform targetTransform)
    {
        Vector3 targetZVector = new Vector3(0, 0, targetTransform.rotation.eulerAngles.z);

        transform.DORotate(targetZVector, FLY_DURATION)
            .SetEase(Ease.InOutQuad);
    }

    public void Die()
    {
        transform.DOScale(Vector3.zero, DEATH_DURATION)
         .SetEase(Ease.InOutQuad)
         .OnComplete(() => DeathCompleted(this));
    }
}