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
    [SerializeField] private WindowController window;
    [SerializeField] private ClickBlocker blocker;
    [Tooltip("Empty RectTransform to set end of window appearing position.")]
    [SerializeField] private RectTransform windowTarget;
    [SerializeField] private Button buttonReshuffle;

    [Header("Window Texts")]
    [SerializeField] [TextArea] private string tutorialHeader;
    [SerializeField] [TextArea] private string tutorialBody;
    [SerializeField] [TextArea] private string winHeader;
    [SerializeField] [TextArea] private string winBody;
    [SerializeField] [TextArea] private string loseHeader;
    [SerializeField] [TextArea] private string loseBody;

    public Reshuffle reshuffle;
    private GameSettings settings;
    private float windowAnimDuration;

    public event Action<WindowKind> WindowClosed;


    public void Setup(GameSettings gs, Reshuffle r)
    {
        settings = gs;
        reshuffle = r;

        windowAnimDuration = settings.windowAnimDuration;
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
        okBtn.onClick.RemoveListener(OnOkPressed);
        okBtn.onClick.AddListener(OnOkPressed);
    }

    private void OnOkPressed()
    {
        blocker.Hide();
    }

    private void OnReshufflePressed()
    {
        reshuffle.Activate();
    }

    private void OnWindowHideCompleted(WindowKind kind)
    {
        window.HideCompleted -= OnWindowHideCompleted;

        WindowClosed?.Invoke(kind);
    }
}