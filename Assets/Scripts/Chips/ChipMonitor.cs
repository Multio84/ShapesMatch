using System;


// tracks chips' falling in game field container
public class ChipMonitor
{
    public event Action ChipsStopped;

    private ChipPile chipPile;
    private int stoppedChipsCount = 0;

    public void Setup(ChipPile cp)
    {
        chipPile = cp;
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
