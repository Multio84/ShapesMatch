using System;


// tracks chips' falling in game field container
public class ChipMonitor
{
    public event Action ChipsStopped;

    private ChipPile chipPile;
    private GameSettings settings;
    private int stoppedChipsCount = 0;
    //private float checkDelay;

    public void Setup(GameSettings gs, ChipPile cp)
    {
        chipPile = cp;
        settings = gs;

        //checkDelay = settings.chipStopCheckDelay;
    }

    public void StartChipStopCheck(Chip chip)
    {
        chip.FallingStopped -= HandleChipStopped;
        chip.FallingStopped += HandleChipStopped;

        // =====  !!!!! now chip starts check itself - check if it's ook !!!!!  =====
        //chip.StartCheckIfStopped(checkDelay);
    }

    // if all emitting chips have stopped
    private void HandleChipStopped(Chip chip)
    {
        chip.FallingStopped -= HandleChipStopped;
        stoppedChipsCount++;

        if (stoppedChipsCount >= chipPile.Count)
        {
            stoppedChipsCount = 0;
            ChipsStopped?.Invoke();
        }
    }
}
