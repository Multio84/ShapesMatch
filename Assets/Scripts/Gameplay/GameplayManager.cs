using System.Collections.Generic;
using UnityEngine;


public class GameplayManager : MonoBehaviour, IInitializable
{
    private GamePanel panel;

    [HideInInspector] public List<Chip> spawnedChips = new List<Chip>();


    public void Setup(GamePanel gp)
    {
        panel = gp;
    }

    public void Init()
    {
        panel.MatchesDestroyed += OnMatchesDestroyed;
    }

    void OnDisable()
    {
        panel.MatchesDestroyed -= OnMatchesDestroyed;
    }

    public void OnChipSent(Chip chip)
    {
        spawnedChips.Remove(chip);
    }

    public void OnChipPlaced()
    {
        ProcessMove();
    }

    public void OnMatchesDestroyed()
    {
        UpdateGameState();

        if (panel.CountPlacedChips() > 0)
            panel.MoveChipsToEmptySlots();
    }

    private void ProcessMove()
    {
        if (panel.HasFlyingChips())
            return;

        panel.MoveChipsToEmptySlots();

        if (panel.CountPlacedChips() >= ChipSpawner.CHIP_COPIES)
        {
            if (panel.FindMatches())
                panel.DestroyMatches();
            else
                UpdateGameState();
        }   
    }

    private void UpdateGameState()
    {
        if (panel.CountPlacedChips() == GamePanel.SLOTS_COUNT)
            GameOver();

        if (panel.CountPlacedChips() == 0 && spawnedChips.Count == 0)
            YouWin();
    }

    private void GameOver()
    {
        Debug.Log("Game Over.");
    }

    private void YouWin()
    {
        Debug.Log("You win!");
    }
}
