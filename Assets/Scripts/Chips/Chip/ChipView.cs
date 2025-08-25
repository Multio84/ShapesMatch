using DG.Tweening;
using System;
using UnityEngine;


public interface IChipView
{
    void Init(ChipPassport passport, ChipPartsDatabase db);
    Tween Move(Transform target, float duration);
    void Rotate(Transform target, float duration);
    Tween Die(float duration);
}

// visual chip presentation
public class ChipView : MonoBehaviour, 
    IChipView, 
    IChipStateMediator
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private SpriteRenderer animalRenderer;

    public event Action<ChipState> StateProduced;

    private Tween move;
    private Tween rotate;
    private Tween death;

    public void Init(ChipPassport passport, ChipPartsDatabase db)
    {
        // setting chip's view
        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);
    }

    public void ApplyState(ChipState state)
    {
        if (state == ChipState.Falling)
            KillAllTweens();

        SetOrderLayer(state);
    }

    public Tween Move(Transform targetTransform, float duration)
    {
        StateProduced?.Invoke(ChipState.Moving);    // -> Moving

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

    private void KillAllTweens()
    {
        move.Kill(false);
        rotate.Kill(false);
        death.Kill(false);
    }

    private void SetOrderLayer(ChipState state)
    {
        string layer = state switch
        {
            ChipState.Emitting => SortingLayers.UnderActionBar,
            ChipState.Moving => SortingLayers.OverActionBar,
            ChipState.Falling => SortingLayers.OverActionBar,
            _ => frameRenderer.sortingLayerName
        };

        frameRenderer.sortingLayerName = layer;
        animalRenderer.sortingLayerName = layer;
    }
}
