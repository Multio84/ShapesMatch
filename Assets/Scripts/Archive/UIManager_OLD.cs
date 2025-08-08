using System;
using UnityEngine;
using UnityEngine.UI;


//public enum WindowKind
//{
//    TutorialManager,
//    LevelCompletion
//}

//public enum WindowKind
//{
//    TutorialFindMatches,
//    Win,
//    Lose,
//    Pause,
//    Settings
//}

public class UIManager_OLD : MonoBehaviour
{
    /*
    [Header("Scene Links")]
    [SerializeField] private WindowController controller;
    [SerializeField] private ClickBlocker blocker;
    [Tooltip("Empty RectTransform to set end of window appearing Position.")]
    [SerializeField] private RectTransform animTarget;

    [Header("Window Texts")]
    [SerializeField] [TextArea] private string tutorialHeader;
    [SerializeField] [TextArea] private string tutorialBody;
    [SerializeField] [TextArea] private string winHeader;
    [SerializeField] [TextArea] private string winBody;
    [SerializeField] [TextArea] private string loseHeader;
    [SerializeField] [TextArea] private string loseBody;

    private ReshuffleButton reshuffleButton;
    private GameSettings settings;
    private float animDuration;

    public event Action<WindowKind> WindowClosed;
    public event Action ReshuffleRequested;


    public void Setup(GameSettings gs, ReshuffleButton rb)
    {
        settings = gs;
        reshuffleButton = rb;

        animDuration = settings.windowAnimDuration;
    }

    private void Awake()
    {
        reshuffleButton.Pressed += OnReshufflePressed;
    }

    public void ShowTutorialWindow() => 
        ShowWindow(tutorialHeader, tutorialBody, WindowKind.Lose);
    public void ShowWinWindow() => 
        ShowWindow(winHeader, winBody, WindowKind.Lose);
    public void ShowLoseWindow() => 
        ShowWindow(loseHeader, loseBody, WindowKind.Lose);
    public void ShowReshuffleTutorial() =>
        ShowWindow(loseHeader, loseBody, WindowKind.Lose);

    private void ShowWindow(string headerText, string bodyText, WindowKind kind)
    {
        if (controller.gameObject.activeSelf) return;

        blocker.Show(animDuration);
        controller.Open(headerText, bodyText, animTarget, kind, animDuration);

        controller.HideCompleted -= OnWindowHideCompleted;
        controller.HideCompleted += OnWindowHideCompleted;

        Button okBtn = controller.OkButton;
        okBtn.onClick.RemoveListener(OnOkPressed); 
        okBtn.onClick.AddListener(OnOkPressed);
    }

    public void SetReshuffleTutorialVisible(bool isVisible)
    {
        //reshuffleTutorial.SetActive(isVisible);
    }

    private void OnOkPressed()
    {
        blocker.Hide(animDuration);
    }

    private void OnReshufflePressed()
    {
        ReshuffleRequested?.Invoke();
    }

    private void OnWindowHideCompleted(WindowKind kind)
    {
        controller.HideCompleted -= OnWindowHideCompleted;
        WindowClosed?.Invoke(kind);
    }
 */   
}