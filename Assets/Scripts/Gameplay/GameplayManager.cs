using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour, IInitializable
{
    private ChipSpawner spawner;
    private GamePanel panel;
    private UIManager uiManager;

    [HideInInspector] public List<Chip> spawnedChips = new List<Chip>();


    public void Setup(ChipSpawner s, GamePanel gp, UIManager uim)
    {
        spawner = s;
        panel = gp;
        uiManager = uim;
    }

    public void Init()
    {
        panel.MatchesDestroyed += OnMatchesDestroyed;
        panel.ChipsMoveCompleted += OnChipsMoveCompleted;
    }

    void OnDisable()
    {
        panel.MatchesDestroyed -= OnMatchesDestroyed;
        panel.ChipsMoveCompleted -= OnChipsMoveCompleted;
    }

    public void GenerateLevel()
    {
        spawner.GenerateLevel();
    }

    public void OnChipSentToPanel(Chip chip)
    {
        spawnedChips.Remove(chip);
    }

    public void OnChipPlaced()
    {
        if (panel.HasFlyingChips()) return;

        if (!panel.MoveChipsToEmptySlots())
            FindAndDestroyMatches();
    }

    public void OnChipsMoveCompleted()
    {
        FindAndDestroyMatches();
    }

    private void FindAndDestroyMatches()
    {
        if (panel.CountPlacedChips() >= ChipSpawner.CHIP_COPIES)
        {
            if (panel.FindMatches())
                panel.DestroyMatches();
            else
                UpdateGameState();
        }
    }

    public void OnMatchesDestroyed()
    {
        UpdateGameState();

        if (panel.CountPlacedChips() > 0)
            panel.MoveChipsToEmptySlots();
    }

    private void UpdateGameState()
    {
        if (panel.CountPlacedChips() == 0 && spawnedChips.Count == 0)
            uiManager.ShowWinWindow();

        if (panel.CountPlacedChips() == GamePanel.SLOTS_COUNT)
            uiManager.ShowLoseWindow();
    }
}
