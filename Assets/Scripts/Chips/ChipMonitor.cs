using System;


// tracks chips' falling in game field container
public class ChipMonitor
{
    public event Action ChipsStopped;

    private ChipPile chipPile;
    private GameSettings settings;
    private int stoppedChipsCount = 0;
    private float checkDelay;

    public void Setup(GameSettings gs, ChipPile cp)
    {
        chipPile = cp;
        settings = gs;

        checkDelay = settings.chipStopCheckDelay;
    }

    public void StartChipStopCheck(Chip chip)
    {
        chip.Stopped -= HandleChipStopped;
        chip.Stopped += HandleChipStopped;

        chip.StartCheckIfStopped(checkDelay);
    }

    private void HandleChipStopped(Chip chip)
    {
        chip.Stopped -= HandleChipStopped;
        stoppedChipsCount++;

        if (stoppedChipsCount >= chipPile.Count)
        {
            chipPile.SetInteractable(true);
            stoppedChipsCount = 0;
            ChipsStopped?.Invoke();
        }
    }
}
