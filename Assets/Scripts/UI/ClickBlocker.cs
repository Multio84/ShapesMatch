using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class ClickBlocker : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.3f;

    private Image image;
    private Tween appearTween;


    private void Awake()
    {
        image = GetComponent<Image>();
        SetEnable(false);
    }

    private void SetEnable(bool isEnable)
    {
        image.raycastTarget = isEnable;
        gameObject.SetActive(isEnable);
    }

    public void Show()
    {
        SetEnable(true);

        // transparent on start showing
        Color c = image.color;
        float targetAlpha = c.a;
        c.a = 0;
        image.color = c;

        appearTween?.Kill();
        appearTween = image.DOFade(targetAlpha, fadeDuration);
    }

    public void Hide()
    {
        appearTween?.Kill();
        appearTween = image.DOFade(0f, fadeDuration)
                     .OnComplete(() => SetEnable(false));
    }
}
