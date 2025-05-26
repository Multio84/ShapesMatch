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

    }

    void OnDisable()
    {

    }

    public void OnChipPlaced()
    {
        ProcessMove();
    }

    void ProcessMove()
    {
        if (panel.HasFlyingChips())
            return;

        panel.MoveChipsToEmptySlots();

        if (panel.CountPlacedChips() >= ChipSpawner.CHIP_COPIES)
        {
            if (panel.FindMatches())
            {
                panel.DestroyMatches();

                if (panel.CountPlacedChips() > 0)
                    panel.MoveChipsToEmptySlots();
            }
            else if (panel.CountPlacedChips() == GamePanel.SLOTS_COUNT)
                Debug.Log("Game Over.");
        }    
    }

    
}
