using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class ClickBlocker : MonoBehaviour
{
    private Image image;
    private Tween tween;
    private float startAlpha = 0;   // transparent before showing
    private float targetAlpha;      // colorful being shown

    private void Awake()
    {
        image = GetComponent<Image>();

        Color startColor = image.color;
        targetAlpha = startColor.a;

        // make transparent on start
        startColor.a = startAlpha;
        image.color = startColor;

        SetEnable(false);
    }

    public void Show(float duration)
    {
        SetEnable(true);
        
        tween?.Kill();
        tween = image.DOFade(targetAlpha, duration);
    }

    public void Hide(float duration)
    {
        tween?.Kill();
        tween = image.DOFade(startAlpha, duration)
                     .OnComplete(() => SetEnable(false));
    }

    private void SetEnable(bool isEnable)
    {
        image.raycastTarget = isEnable;
        gameObject.SetActive(isEnable);
    }
}
