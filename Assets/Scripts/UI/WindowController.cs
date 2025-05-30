using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private RectTransform windowRoot;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body;
    [SerializeField] private Button okButton;

    [Header("Animation Settings")]
    [SerializeField] private float animDuration;

    private Vector2 startPos;
    private Vector2 targetPos;
    private Tween moveTween;
    private WindowKind kind;

    public event System.Action<WindowKind> HideCompleted;


    public Button OkButton => okButton;

    private void Awake()
    {
        if (windowRoot is null) 
            windowRoot = transform as RectTransform;

        startPos = Vector2.up * Screen.height;

        okButton.onClick.AddListener(OnOkPressed);
        gameObject.SetActive(false);
    }

    private Vector2 GetTargetPos(RectTransform targetTransform)
    {
        if (targetTransform is null)
            targetTransform = windowRoot;

        return targetTransform.anchoredPosition;
    }

    public void Open(
        string headerText, 
        string bodyText, 
        RectTransform targetTransform, 
        WindowKind kind, 
        float animDuration
        )
    {
        this.kind = kind;
        header.text = headerText;
        body.text = bodyText;
        gameObject.SetActive(true);

        windowRoot.anchoredPosition = startPos;
        moveTween = windowRoot.DOAnchorPos(GetTargetPos(targetTransform), animDuration)
            .SetEase(Ease.OutBack);
    }

    private void OnOkPressed() => Hide();

    public void Hide()
    {
        moveTween?.Kill();

        moveTween = windowRoot.DOAnchorPos(startPos, animDuration)
                              .SetEase(Ease.InBack)
                              .OnComplete(() =>
                              {
                                  gameObject.SetActive(false);
                                  HideCompleted?.Invoke(kind);
                              });
    }
}
