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
    [SerializeField] private SpriteRenderer backRenderer;
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private SpriteRenderer animalRenderer;

    private Rigidbody2D rb;
    private Collider2D col;
    private GameplayManager gameplayManager;
    private GamePanel panel;
    private bool isIteractable = true;
    private const float MOVE_DURATION = 2.6f;

    private ChipPassport passport;
    public ChipPassport Passport => passport;
    public bool isMatched = false;

    public event Action<Chip> ChipSent;
    public event Action ChipPlaced;


    void OnDisable()
    {
        ChipPlaced -= gameplayManager.OnChipPlaced;
    }

    public void Init(GameplayManager gm, GamePanel gp, ChipPassport p, ChipPartsDatabase db)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
        if (rb is null || col is null)
        {
            Debug.LogError("RigidBody2D or Collider2D wasn't found in a chip.");
            return;
        }

        gameplayManager = gm;
        panel = gp;
        passport = p;

        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);

        ChipSent += gameplayManager.OnChipSent;
        ChipPlaced += gameplayManager.OnChipPlaced;

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isIteractable)
        {
            isIteractable = false;

            ChipSent?.Invoke(this);
            SendToActionBar();
        }
    }

    public void SendToActionBar()
    {
        int slotIdx = panel.GetNextAvailableSlot();
        if (slotIdx < 0)
        {
            Debug.LogError("No available slot to place next chip.");
            return;
        }

        PlaceOverActionBar();
        rb.simulated = false;
        col.enabled = false;
        
        panel.ReserveSlot(slotIdx, this);

        Rotate(panel.GetSlotTransform(slotIdx));
        Move(panel.GetSlotTransform(slotIdx)).
            OnComplete(() => ChipArrived(slotIdx));
    }

    private void ChipArrived(int idx)
    {
        panel.PlaceChip(idx, this);
        ChipPlaced?.Invoke();
    }

    private void PlaceOverActionBar()
    {
        if (panel.canvas is null)
        {
            Debug.LogWarning("ActionBarCanvas is not assigned.");
            return;
        }

        int actionBarOrder = panel.canvas.sortingOrder;

        backRenderer.sortingOrder += actionBarOrder;
        frameRenderer.sortingOrder += actionBarOrder;
        animalRenderer.sortingOrder += actionBarOrder;
    }

    public Tween Move(Transform targetTransform)
    {
        return transform.DOMove(targetTransform.position, MOVE_DURATION)
            .SetEase(Ease.InOutQuad);
    }

    public void Rotate(Transform targetTransform)
    {
        Vector3 targetZVector = new Vector3(0, 0, targetTransform.rotation.eulerAngles.z);

        transform.DORotate(targetZVector, MOVE_DURATION)
            .SetEase(Ease.InOutQuad);
    }
}