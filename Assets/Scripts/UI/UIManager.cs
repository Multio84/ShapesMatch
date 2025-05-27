using System;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Scene references")]
    [SerializeField] private WindowController window;
    [SerializeField] private ClickBlocker blocker;

    [Header("Texts")]
    [TextArea] public string winHeader;
    [TextArea] public string winBody;
    [TextArea] public string loseHeader;
    [TextArea] public string loseBody;

    public event Action WindowClosed;


    public void ShowWinWindow() => ShowWindow(winHeader, winBody);
    public void ShowLoseWindow() => ShowWindow(loseHeader, loseBody);

    private void ShowWindow(string headerText, string bodyText)
    {
        if (window.gameObject.activeSelf) return;

        blocker.Show();
        window.Open(headerText, bodyText);

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

    private void OnWindowHideCompleted()
    {
        window.HideCompleted -= OnWindowHideCompleted;

        WindowClosed?.Invoke();
    }
}