using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Reshuffle : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private RectTransform icon;

    private GameSettings settings;
    private ChipSpawner chipSpawner;

    private float animDuration;
    private bool isEnabled;
    private int availableCount = 2; // number of reshuffle activations, availableCount in current level


    public void Setup(GameSettings gs, UIManager uim, ChipSpawner cs)
    {
        settings = gs;
        chipSpawner = cs;

        animDuration = settings.reshuffleAnimDuration;
    }

    private void Awake()
    {
        SetEnabled(false);
        chipSpawner.ChipsStopped += OnChipsStopped;
    }

    private void OnDisable()
    {
        chipSpawner.ChipsStopped -= OnChipsStopped;
    }

    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            Debug.Log("Reshuffle enabled");
        }
        else
        {
            Debug.Log("Reshuffle disabled");
        }

        isEnabled = enabled;
        button.interactable = enabled;
    }

    public void Activate()
    {
        if (!isEnabled) return;
        isEnabled = false;
        availableCount--;

        icon.DORotate(new Vector3(0, 0, 360), animDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InSine)
            .OnComplete(() => button.interactable = false);

        Debug.Log($"Gameplay activated. {availableCount} left.");
        chipSpawner.StartReshuffle();
    }

    private void OnChipsStopped(SpawnerState state)
    {
        if (state == SpawnerState.Gameplay && availableCount > 0)
            SetEnabled(true);
    }

}
