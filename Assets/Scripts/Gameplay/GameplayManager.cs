using UnityEngine;


public class GameplayManager : MonoBehaviour, IInitializable
{
    private GameSettings settings;
    private ChipSpawner spawner;
    private ActionBar actionBar;
    private UIManager uiManager;

    private ChipPile chipPile;
    private int chipCopies;


    public void Setup(GameSettings gs, ChipSpawner cs, ActionBar ab, UIManager uim, ChipPile cp)
    {
        settings = gs;
        spawner = cs;
        actionBar = ab;
        uiManager = uim;
        chipPile = cp;

        chipCopies = settings.chipCopies;
    }

    public void Init()
    {
        actionBar.MatchesDestroyed += OnMatchesDestroyed;
        actionBar.ChipsMoveCompleted += OnChipsMoveCompleted;
    }

    void OnDisable()
    {
        actionBar.MatchesDestroyed -= OnMatchesDestroyed;
        actionBar.ChipsMoveCompleted -= OnChipsMoveCompleted;
    }

    public void GenerateLevel() => spawner.GenerateLevel();

    public void OnChipSentToActionBar(Chip chip) => chipPile.Remove(chip);

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
        if (actionBar.CountPlacedChips() == 0 && chipPile.Count == 0)
            uiManager.ShowWinWindow();

        if (actionBar.CountPlacedChips() == ActionBar.SLOTS_COUNT &&
            !uiManager.reshuffle.IsReshuffleAvailable)
                uiManager.ShowLoseWindow();
    }
}
