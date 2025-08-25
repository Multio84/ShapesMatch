using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BarState
{
    Empty,
    PotentialShift, // chips number > 2
    PotentialMatch, // chips number >= chipsCopies
    Full
}

public interface IBarState
{
    event Action<BarState> StateChanged;
    void UpdateState();
}

public interface IChipDropper
{
    List<Chip> DropChips();
}

public class ActionBarController : MonoBehaviour, IChipCollector, IChipDropper, IBarState
{
    public event Action<BarState>   StateChanged;
    public event Action<Chip>       ChipCollected;
    public bool Busy { get; private set; }

    private GameSettings    settings;
    private ActionBarView   view;
    private ActionBarModel  model;
    private int             chipCopies; 
    private BarState        state;

    public void Setup(GameSettings gs, ActionBarView bv, ActionBarModel bm)
    {
        settings = gs;
        view = bv;
        model = bm;

        chipCopies = settings.chipCopies;
    }

    public void UpdateState()
    {
        BarState state;
        int chipsInBar = model.CountPlacedChips();

        if (chipsInBar == 0)
            state = BarState.Empty;   // win case

        else if (chipsInBar == ActionBarModel.SlotsCount)
            state = BarState.Full;    // lose case or reshuffle tutorial

        else if (chipsInBar >= chipCopies)
            state = BarState.PotentialMatch; // can find matches and destroy

        else
            state = BarState.PotentialShift; // got chips to shift

        if (this.state == state) return;
        this.state = state;

        StateChanged?.Invoke(state);
    }

    public void TryCollectChip(Chip chip)
    {
        if (Busy) return;
        if (!model.TryBeginPlacement(out int slotIdx)) return;
        Busy = true;

        view.FlyChip(chip, view.GetSlotPoint(slotIdx), 
            () => PlaceChip(chip, slotIdx));
    }

    private void PlaceChip(Chip chip, int slotIdx)
    {
        model.CommitPlacement(chip, slotIdx);
        ChipCollected?.Invoke(chip);

        view.AttachChipToSlot(chip, slotIdx);

        // don't collapse if there are chips flying to bar
        if (model.HasFlyingChips()) return;

        var collapse = model.BuildCollapse();
        Collapse(collapse);
    }

    public List<Chip> DropChips()
    {
        return model.RemoveAllChips();
    }

    /*  
     *  collapse is:
     *  finding chip matches,
     *  destroying, if matches found,
     *  and shifting, if there are chips to move
    */
    private void Collapse(CollapseInfo collapseInfo)
    {
        if (collapseInfo.IsEmpty)
        {
            Busy = false;
            return;
        }

        Busy = true;
        StartCoroutine(ProcessCollapse(collapseInfo));
    }

    private IEnumerator ProcessCollapse(CollapseInfo collapseInfo)
    {
        yield return StartCoroutine(view.PlayRemoval(collapseInfo.Matches));
        yield return StartCoroutine(view.PlayMoves(collapseInfo.Moves));

        UpdateState();
        Busy = false;
    }
}