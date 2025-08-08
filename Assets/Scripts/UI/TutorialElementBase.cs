using DG.Tweening;
using System;
using UnityEngine;


public abstract class TutorialElementBase : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public TutorialStep Step => step;

    public event Action<TutorialStep> Closed;

    private TutorialStep step;
    private Tween tween;

    internal void Show(float duration, Ease ease)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;

        tween?.Kill();
        tween = Animate(1f, duration, ease);
    }

    internal void Hide(float duration, Ease ease)
    {
        tween.Kill();
        tween = Animate(0f, duration, ease)
            .OnComplete(() =>
            {
                Closed?.Invoke(Step);
                Destroy(gameObject);
            });
    }

    private Tween Animate(float alpha, float duration, Ease ease)
    {

        return canvasGroup.DOFade(alpha, duration)
            .SetEase(ease);
    }
}
