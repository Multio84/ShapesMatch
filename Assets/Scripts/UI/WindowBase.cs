using DG.Tweening;
using System;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public abstract class WindowBase : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    
    public abstract WindowKind Kind { get; }

    public event Action StartedHiding;
    //public event Action<WindowKind> Closed;

    private Vector2 startPos;
    private Vector2 targetPos;
    private Tween moveTween;

    public void Init(RectTransform target)
    {
        if (!root) root = transform as RectTransform;

        startPos = Vector2.up * Screen.height;
        targetPos = GetTargetPos(target);
    }

    internal void Show(float duration, Ease ease)
    {
        root.anchoredPosition = startPos;
        gameObject.SetActive(true);

        moveTween?.Kill();
        moveTween = Move(targetPos, duration, ease);
    }

    protected void Hide(float duration, Ease ease)
    {
        StartedHiding?.Invoke();

        moveTween?.Kill();
        moveTween = Move(startPos, duration, ease)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                //Closed?.Invoke(Kind);
            });
    }

    private Tween Move(Vector2 targetPos, float duration, Ease ease)
    {
        return root.DOAnchorPos(targetPos, duration).SetEase(ease);
    }

    private Vector2 GetTargetPos(RectTransform target)
    {
        if (!target) target = root;

        return target.anchoredPosition;
    }
}
