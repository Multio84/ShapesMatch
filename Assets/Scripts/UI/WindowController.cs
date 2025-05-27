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
    [Tooltip("Empty RectTransform to set end window position.")]
    [SerializeField] private RectTransform targetAnchor;   // a place to move to

    [Header("Animation Settings")]
    [SerializeField] private float moveDuration = 0.3f;

    private Vector2 startPos;
    private Vector2 targetPos;
    private Tween moveTween;

    public event System.Action HideCompleted;


    public Button OkButton => okButton;

    private void Awake()
    {
        if (windowRoot is null) 
            windowRoot = transform as RectTransform;

        if (targetAnchor is null)
            targetAnchor = windowRoot;

        targetPos = targetAnchor.anchoredPosition;
        if (targetAnchor != windowRoot)
            targetAnchor.gameObject.SetActive(false);

        startPos = targetPos + Vector2.up * Screen.height;

        okButton.onClick.AddListener(OnOkPressed);
        gameObject.SetActive(false);
    }

    public void Open(string headerText, string bodyText)
    {
        header.text = headerText;
        body.text = bodyText;
        gameObject.SetActive(true);

        windowRoot.anchoredPosition = startPos;
        moveTween = windowRoot.DOAnchorPos(targetPos, moveDuration)
            .SetEase(Ease.OutBack);
    }

    private void OnOkPressed() => Hide();

    public void Hide()
    {
        moveTween?.Kill();

        moveTween = windowRoot.DOAnchorPos(startPos, moveDuration)
                              .SetEase(Ease.InBack)
                              .OnComplete(() =>
                              {
                                  gameObject.SetActive(false);
                                  HideCompleted?.Invoke();
                              });
    }
}
