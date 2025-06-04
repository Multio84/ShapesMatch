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
    private int availableCount = 10; // number of reshuffles, available in current level
    
    
    public bool IsReshuffleAvailable => availableCount > 0 ? true : false;
    
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

        chipSpawner.StartReshuffle();
    }

    private void OnChipsStopped(SpawnerState state)
    {
        if (state == SpawnerState.LevelPlaying && availableCount > 0)
            SetEnabled(true);
    }
}
