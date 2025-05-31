using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;


public enum WindowKind 
{
    Tutorial,
    LevelCompletion
}

public class UIManager : MonoBehaviour
{
    [Header("Scene Links")]
    public Button buttonReshuffle;
    public RectTransform buttonReshuffleIcon;
    [SerializeField] private WindowController window;
    [SerializeField] private ClickBlocker blocker;
    [Tooltip("Empty RectTransform to set end of window appearing position.")]
    [SerializeField] private RectTransform windowTarget;
    
    [Header("Window Texts")]
    [SerializeField] [TextArea] private string tutorialHeader;
    [SerializeField] [TextArea] private string tutorialBody;
    [SerializeField] [TextArea] private string winHeader;
    [SerializeField] [TextArea] private string winBody;
    [SerializeField] [TextArea] private string loseHeader;
    [SerializeField] [TextArea] private string loseBody;

    private GameSettings settings;
    private float windowAnimDuration;
    private float reshuffleAnimDuration;

    public event Action<WindowKind> WindowClosed;

    public void Setup(GameSettings gs)
    {
        settings = gs;
        windowAnimDuration = settings.windowAnimDuration;
        reshuffleAnimDuration = settings.reshuffleAnimDuration;
    }

    private void Awake()
    {
        buttonReshuffle.onClick.AddListener(OnReshufflePressed);
    }

    private void OnDisable()
    {
        buttonReshuffle.onClick.RemoveListener(OnReshufflePressed);
    }

    public void ShowTutorialWindow() => 
        ShowWindow(tutorialHeader, tutorialBody, WindowKind.Tutorial);
    public void ShowWinWindow() => 
        ShowWindow(winHeader, winBody, WindowKind.LevelCompletion);
    public void ShowLoseWindow() => 
        ShowWindow(loseHeader, loseBody, WindowKind.LevelCompletion);

    private void ShowWindow(string headerText, string bodyText, WindowKind kind)
    {
        if (window.gameObject.activeSelf) return;

        blocker.Show(windowAnimDuration);
        window.Open(headerText, bodyText, windowTarget, kind, windowAnimDuration);

        window.HideCompleted -= OnWindowHideCompleted;
        window.HideCompleted += OnWindowHideCompleted;

        Button okBtn = window.OkButton;
        okBtn.onClick.RemoveListener(OnOkClicked);
        okBtn.onClick.AddListener(OnOkClicked);
    }

    private void OnOkClicked()
    {
        blocker.Hide();
    }

    private void OnWindowHideCompleted(WindowKind kind)
    {
        window.HideCompleted -= OnWindowHideCompleted;

        WindowClosed?.Invoke(kind);
    }

    private void OnReshufflePressed()
    {
        buttonReshuffleIcon
            .DORotate(new Vector3(0, 0, 360), reshuffleAnimDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InSine)
            .OnComplete(() => DisableReshuffle());
    }

    private void DisableReshuffle()
    {
        buttonReshuffle.interactable = false;
    }

}