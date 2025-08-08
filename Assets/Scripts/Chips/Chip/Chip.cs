using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


//public enum ChipState
//{
//    Idle, 
//    Emitting, 
//    Flying,
//    Falling
//}

[RequireComponent(typeof(Rigidbody2D))]
public class Chip : MonoBehaviour, IPointerDownHandler
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private SpriteRenderer animalRenderer;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D col;

    public ChipPassport Passport => passport;
    public bool IsInteractable { get; set; }
    public ChipState State { get; private set; }

    public event Action<Chip> Stopped;
    //public event Action<Chip> DeathCompleted;

    private ChipState state;
    private bool isInteractable = false;
    private IChipCollector collector;
    private ChipPassport passport;
    private Tween move;
    private Tween rotate;
    private Tween death;
    private float prevSpeed;    // save speed to detect if chip slowed down

    public void Init(IChipCollector collector, ChipPassport passport, ChipPartsDatabase db)
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
        if (!rb || !col)
        {
            Debug.LogError("RigidBody2D or Collider2D wasn't found in a chip.");
            return;
        }

        this.collector = collector;
        this.passport = passport;
        SetView(passport, db);

        Emit();
    }

    public void SetState(ChipState newState)
    {
        if (state == newState) return;
        state = newState;
        ApplyVisualLayer();
    }

    private void ApplyVisualLayer()
    {
        string layer = state switch
        {
            ChipState.Emitting  => SortingLayers.UnderActionBar,
            ChipState.Flying    => SortingLayers.OverActionBar,
            ChipState.Falling   => SortingLayers.OverActionBar,
            _                   => frameRenderer.sortingLayerName
        };

        frameRenderer.sortingLayerName  = layer;
        animalRenderer.sortingLayerName = layer;
    }

    public void Emit() => State = ChipState.Emitting;
    public void Fly()
    {
        isInteractable = false;
        SetState(ChipState.Flying);
    }

    public void StartCheckIfStopped(float checkDelay)
    {
        prevSpeed = 0;
        InvokeRepeating(nameof(CheckIfStopped), checkDelay, checkDelay);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isInteractable) return;

        isInteractable = false;
        collector?.TryCollectChip(this);
    }

    public void SetView(ChipPassport passport, ChipPartsDatabase db)
    {
        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);
    }

    public void SetPhysEnabled(bool isPhysical) => rb.simulated = isPhysical;

    // can be used for flying to (over) ActionBar
    // or when emitting chips after reshuffle (under ActionBar)
    //public void SetPhysEnabled(bool isPhysical, bool isOverActionBar, int barSortingOffset)
    //{
    //    rb.simulated = isPhysical;

    //    if (isOverActionBar)
    //    {
    //        frameRenderer.sortingOrder += barSortingOffset;
    //        animalRenderer.sortingOrder += barSortingOffset;
    //    }
    //    else
    //    {
    //        frameRenderer.sortingOrder -= barSortingOffset;
    //        animalRenderer.sortingOrder -= barSortingOffset;
    //    }
    //}

    public void DropFromActionBar()
    {
        State = ChipState.Falling;
        KillAllTweens();
        rb.simulated = true;
    }

    public Tween Move(Transform targetTransform, float duration)
    {
        return move = transform
            .DOMove(targetTransform.position, duration)
            .SetEase(Ease.InOutQuad);
    }

    public void Rotate(Transform targetTransform, float duration)
    {
        Vector3 targetZVector = new Vector3(0, 0, targetTransform.rotation.eulerAngles.z);
        rotate = transform
            .DORotate(targetZVector, duration)
            .SetEase(Ease.InOutQuad);
    }

    public Tween Die(float duration)
    {
        return death = transform
            .DOScale(Vector3.zero, duration)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => KillAllTweens());
    }

    public void KillAllTweens()
    {
        move.Kill(false);
        rotate.Kill(false);
        death.Kill(false);
    }

    private void CheckIfStopped()
    {
        float curSpeed = rb.velocity.sqrMagnitude;
        if (curSpeed < prevSpeed)
        {
            CancelInvoke(nameof(CheckIfStopped));
            State = ChipState.Idle;
            Stopped?.Invoke(this);

            return;
        }

        prevSpeed = curSpeed;
        isInteractable = true;
    }


    //public void ChangeLayerOrder(bool isOverActionBar, int actionBarOrder)
    //{
    //    if (isOverActionBar)
    //    {
    //        frameRenderer.sortingOrder += actionBarOrder;
    //        animalRenderer.sortingOrder += actionBarOrder;
    //    }
    //    else
    //    {
    //        frameRenderer.sortingOrder -= actionBarOrder;
    //        animalRenderer.sortingOrder -= actionBarOrder;
    //    }
    //}

    //private void OnChipArrivedToActionBar(int idx)
    //{
    //    actionBar.PlaceChip(idx, this);
    //    Placed?.Invoke();
    //}

    // «јѕ–≈“»“№ отправл€ть в экшен бар фишки если панель полна!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // ƒл€ этого отрубаем фишки гдето вверху, если панель полна
    //public void SendToActionBar(int idx, IChipArrived arrivedListener)
    //{
    //    if (rb is not null) rb.simulated = false;
    //    ChangeLayerOrder(true);

    //    Collected?.Invoke(this);
    //    actionBar.ReserveSlot(idx, this);

    //    Tween tween;
    //    Rotate(actionBar.GetSlotTransform(idx));
    //    Move(actionBar.GetSlotTransform(idx), flyDuration).
    //        OnComplete(() => OnChipArrivedToActionBar(idx));

    //    //arrivedListener.OnChipArrived(this);
    //    //
    //    collector.TryCollectChip(this);
    //}
}