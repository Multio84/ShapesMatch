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
    private ActionBarView   _view;
    private ActionBarModel  _model;
    private int             chipCopies; 
    private BarState _state;

    public void Setup(GameSettings gs, ActionBarView bv, ActionBarModel bm)
    {
        settings = gs;
        _view = bv;
        _model = bm;

        chipCopies = settings.chipCopies;
    }

    public void UpdateState()
    {
        BarState state;
        int chipsInBar = _model.CountPlacedChips();

        if (chipsInBar == 0)
            state = BarState.Empty;   // win case

        else if (chipsInBar == ActionBarModel.SlostCount)
            state = BarState.Full;    // lose case or reshuffle tutorial

        else if (chipsInBar >= chipCopies)
            state = BarState.PotentialMatch; // can find matches and destroy

        else
            state = BarState.PotentialShift; // got chips to shift

        if (_state == state) return;
        _state = state;

        StateChanged?.Invoke(state);
    }

    public void TryCollectChip(Chip chip)
    {
        if (Busy) return;
        if (!_model.TryBeginPlacement(out int slotIdx)) return;

        Busy = true;
        chip.Fly();
        ChipCollected?.Invoke(chip);

        _view.FlyChip(
            chip,
            _view.GetSlotPoint(slotIdx),
            () => PlaceChip(chip, slotIdx)
            );
    }

    public List<Chip> DropChips()
    {
        return _view.DropChips(_model.RemoveAllChips());
    }

    private void PlaceChip(Chip chip, int slotIdx)
    {
        _model.CommitPlacement(chip, slotIdx);
        _view.AttachChipToSlot(chip, slotIdx);

        // don't collapse if some chips are flying to bar
        if (_model.HasFlyingChips())
            return;

        var collapse = _model.BuildCollapse();
        Collapse(collapse);
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
        yield return StartCoroutine(_view.PlayRemoval(collapseInfo.Matches));
        yield return StartCoroutine(_view.PlayMoves(collapseInfo.Moves));

        UpdateState();
        Busy = false;
    }
}