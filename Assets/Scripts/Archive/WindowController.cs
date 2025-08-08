using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowController : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private GameObject window;
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI body;
    [SerializeField] private Button okButton;

    [Header("Animation Settings")]
    [SerializeField] private float animDuration;

    public Button OkButton => okButton;

    public event System.Action<WindowKind> HideCompleted;

    private RectTransform windowRoot;
    private Vector2 startPos;
    private Tween moveTween;
    private WindowKind kind;

    private void Awake()
    {
        if (!windowRoot) windowRoot = transform as RectTransform;

        startPos = Vector2.up * Screen.height;

        okButton.onClick.AddListener(OnOkPressed);
        gameObject.SetActive(false);
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

    private Vector2 GetTargetPos(RectTransform targetTransform)
    {
        if (!targetTransform) targetTransform = windowRoot;

        return targetTransform.anchoredPosition;
    }

    private void OnOkPressed() => Hide();
}
