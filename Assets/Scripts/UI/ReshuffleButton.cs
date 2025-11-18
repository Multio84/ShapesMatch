using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ReshuffleButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform icon;
    [SerializeField] private TextMeshProUGUI availableCountText;
    
    public event Action Pressed;

    private GameSettings settings;
    private ReshuffleService service;
    private bool isAnimating = false;
    private float animDuration;

    public void Setup(GameSettings gs, ReshuffleService rs)
    {
        settings = gs;
        service = rs;

        animDuration = settings.reshuffleAnimDuration;
    }

    private void Awake()
    {
        button.onClick.AddListener(OnButtonPressed);
        UpdateInteractable();

        DisplayReshuffleCount();
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnButtonPressed);
    }

    public void UpdateInteractable()
    {
        button.interactable = service.CanExecute;
    }

    public void Animate()
    {
        isAnimating = true;

        icon.DORotate(new Vector3(0, 0, 360), animDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InSine)
            .OnComplete(() => {
                isAnimating = false;
                // if service.CanExecute (usually not), button will be enabled after animation
                UpdateInteractable();
                DisplayReshuffleCount();
            });
    }

    private void OnButtonPressed()
    {
        // button should look active for animation visuals, so it's still interactable,
        // but reshuffle should be forbidden for activation until animation is completed
        if (isAnimating) return;

        Pressed?.Invoke();
        Animate();
    }

    private void DisplayReshuffleCount()
    {
        availableCountText.text = service.AvailableCount.ToString();
    }
}
