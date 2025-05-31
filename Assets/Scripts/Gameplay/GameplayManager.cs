using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour, IInitializable
{
    private GameSettings settings;
    private ChipSpawner spawner;
    private ActionBar actionBar;
    private UIManager uiManager;

    private int chipCopies;

    [HideInInspector] public List<Chip> spawnedChips = new List<Chip>();


    public void Setup(GameSettings gs, ChipSpawner cs, ActionBar ab, UIManager uim)
    {
        settings = gs;
        spawner = cs;
        actionBar = ab;
        uiManager = uim;

        chipCopies = settings.chipCopies;
    }

    public void Init()
    {
        actionBar.MatchesDestroyed += OnMatchesDestroyed;
        actionBar.ChipsMoveCompleted += OnChipsMoveCompleted;
        uiManager.buttonReshuffle.onClick.AddListener(OnReshufflePressed);
    }

    void OnDisable()
    {
        actionBar.MatchesDestroyed -= OnMatchesDestroyed;
        actionBar.ChipsMoveCompleted -= OnChipsMoveCompleted;
        uiManager.buttonReshuffle.onClick.RemoveListener(OnReshufflePressed);
    }

    public void GenerateLevel() => spawner.GenerateLevel();

    public void OnChipSentToActionBar(Chip chip)
    {
        spawnedChips.Remove(chip);
    }

    public void OnChipPlaced()
    {
        if (actionBar.HasFlyingChips()) return;

        if (!actionBar.MoveChipsToEmptySlots())
            FindAndDestroyMatches();
    }

    public void OnChipsMoveCompleted()
    {
        FindAndDestroyMatches();
    }

    private void FindAndDestroyMatches()
    {
        if (actionBar.CountPlacedChips() >= chipCopies)
        {
            if (actionBar.FindMatches())
                actionBar.DestroyMatches();
            else
                UpdateGameState();
        }
    }

    public void OnMatchesDestroyed()
    {
        UpdateGameState();

        if (actionBar.CountPlacedChips() > 0)
            actionBar.MoveChipsToEmptySlots();
    }

    private void UpdateGameState()
    {
        if (actionBar.CountPlacedChips() == 0 && spawnedChips.Count == 0)
            uiManager.ShowWinWindow();

        if (actionBar.CountPlacedChips() == ActionBar.SLOTS_COUNT)
            uiManager.ShowLoseWindow();
    }

    private void OnReshufflePressed()
    {
        spawner.StartReshuffle();
    }
}
