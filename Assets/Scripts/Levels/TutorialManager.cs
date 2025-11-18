using UnityEngine;


public enum TutorialStep
{
    None,
    FindMatches,
    UseReshuffle,
    Done
}

// manages tutorial and it's step
public class TutorialManager
{
    public TutorialStep Step => step;
    //is it needed?:
    //public event Action TutorialClosed;

    private TutorialUIController controller;
    private IBarState barState;
    private ReshuffleService reshuffleService;
    private ChipMonitor chipsMonitor;
    private TutorialStep step;

    public void Setup (TutorialUIController tc)
    {
        step = TutorialStep.None;

        controller = tc;

        chipsMonitor.ChipsStopped += OnFirstLevelGenerated;
        barState.StateChanged += OnFirstActionBarFilled;
    }

    public void OnFirstLevelGenerated()
    {
        if (step != TutorialStep.FindMatches) return;

        step = TutorialStep.UseReshuffle;
        chipsMonitor.ChipsStopped -= OnFirstLevelGenerated;

        controller.Show(TutorialStep.FindMatches);
    }

    public void OnFirstActionBarFilled(BarState state)
    {
        if (state != BarState.Full ||
            step != TutorialStep.UseReshuffle)
            return;

        step = TutorialStep.Done;
        barState.StateChanged -= OnFirstActionBarFilled;

        if (reshuffleService.HasBeenUsed)
        {
            Debug.Log("Reshuffle has been used, so \'UseReshuffle\' tutorial skipped.");
            return;
        }

        controller.Show(TutorialStep.UseReshuffle);
    }

    public void ResetProgress()
    {
        step = TutorialStep.None;
    }
}
