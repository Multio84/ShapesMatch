using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


//public enum BarState
//{
//    Empty,
//    PotentialShift,     // chips number > 2
//    PotentialMatch, // chips number >= chipsCopies
//    Full
//}

[RequireComponent(typeof(SpriteRenderer))]
public class ActionBar : MonoBehaviour//, IChipCollector
{
    /*
    [Header("Links")]
    [SerializeField] private Transform chipsRoot;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private GameSettings settings;
    private ChipSpawner chipSpawner;
    private ISlotProvider provider;
    private IChipInputToggle chipInput;

    private List<Chip> chipsToDelete = new();
    private List<Chip> chipsToMove = new();
    private float chipFlyDuration;
    private float chipShiftDuration;
    private float chipDeathDuration;
    private int chipCopies;
    private int layerOrder;
    private BarState barState;
    
    public int LayerOrder => layerOrder;
    public BarState BarState => barState;
    public event Action MatchesDestroyed;
    public event Action ChipsShiftCompleted;
    public event Action<Chip> ChipCollected;
    public event Action<Chip, int> ChipAdded;


    public void Setup(GameSettings gs, ChipSpawner cs, SlotManager sm)
    {
        settings = gs;
        chipSpawner = cs;
        this.provider = sm;

        chipFlyDuration = settings.chipFlyDuration;
        chipShiftDuration = settings.chipShiftDuration;
        chipDeathDuration = settings.chipDeathDuration;
        chipCopies = settings.chipCopies;

        barState = UpdateState();
        layerOrder = spriteRenderer.sortingOrder;
    }

    public BarState UpdateState()
    {
        int chipsInBar = provider.CountPlacedChips();

        if (chipsInBar == 0)
            return barState = BarState.Empty;   // win case

        else if (chipsInBar >= chipCopies)
            return barState = BarState.PotentialMatch;  // find matches and destroy

        else if (chipsInBar == SlotManager.SlotsCount)
            return barState = BarState.Full;    // lose case, reshuffle tutorial

        else if (chipsInBar > 0)
            return barState = BarState.PotentialShift; // got chips to shift
    }


    public void TryCollectChip(Chip chip)
    {
        // just insurance
        if (!provider.HasFreeSlot()) return;

        int idx = provider.GetNextAvailableSlot();
        if (idx < 0) return;

        provider.ReserveSlot(idx);
        ChipCollected?.Invoke(chip);

        chip.SetPhysEnabled(true, false, layerOrder);
        chip.Move(provider.GetSlotTransform(idx), chipFlyDuration).
            OnComplete(() => OnChipArrived(idx, chip));

        EvaluateInputGate();
    }

    private void OnChipArrived(int idx, Chip chip)
    {
        provider.OccupySlot(idx, chip);
        chip.transform.SetParent(chipsRoot);
        ChipAdded?.Invoke(chip, idx);

        UpdateState();
        EvaluateInputGate();
    }

    // make chips in field interactable or not
    private void EvaluateInputGate()
    {
        bool hasFreeSlot = provider.HasFreeSlot();
        chipInput.SetInteractable(hasFreeSlot);
    }


    public bool FindMatches()
    {
        bool wasMatched = false;    // if any of chips were matched

        for (int i = 0; i < SlotManager.SlotsCount; i++)
        {
            if (provider.GetState(i) != SlotState.Occupied) continue;

            int matchCount = 1;
            List<int> matchedIndices = new List<int> { i };

            for (int j = i + 1; j < SlotManager.SlotsCount; j++)
            {
                if (provider.GetState(j) != SlotState.Occupied) continue;

                if (provider.GetSlot(i).chip.Passport.IsSameAs(provider.GetSlot(j).chip.Passport))
                {
                    matchedIndices.Add(j);
                    matchCount++;
                }

                if (matchCount == settings.chipCopies) break;
            }

            if (matchCount == settings.chipCopies)
                foreach (int idx in matchedIndices)
                {
                    provider.SetState(idx, SlotState.Matched);
                    wasMatched = true;
                }
        }

        return wasMatched;
    }

    public void DestroyMatches()
    {
        foreach (var s in provider.Slots)
            if (s.state == SlotState.Matched)
            {
                Chip chip = s.chip;
                chipsToDelete.Add(chip);
                chip.DeathCompleted += HandleMatchesDestroyed;
                chip.Die(chipDeathDuration);
            }
    }

    private void HandleMatchesDestroyed(Chip chip)
    {
        chip.DeathCompleted -= HandleMatchesDestroyed;
        chipsToDelete.Remove(chip);

        provider.DestroyChip(chip);
        Destroy(chip.gameObject);

        if (chipsToDelete.Count <= 0)
            MatchesDestroyed?.Invoke();
    }


    public bool ShiftChipsToEmptySlots()
    {
        bool chipsSentToMove = false;

        for (int i = 0; i < SlotManager.SlotsCount; i++)
        {
            if (provider.GetSlot(i).state != SlotState.Free) continue;

            for (int j = i + 1; j < SlotManager.SlotsCount; j++)
            {
                if (provider.GetSlot(j).state != SlotState.Occupied) continue;

                chipsSentToMove = true;

                Transform target = provider.GetSlotTransform(i);
                Chip chip = provider.GetSlot(j).chip;
                provider.RelocateChip(j, i);

                chipsToMove.Add(chip);
                chip.Move(target, chipShiftDuration)
                    .OnComplete(() => HandleChipsShifted(chip));

                break;
            }
        }

        return chipsSentToMove;
    }

    private void HandleChipsShifted(Chip chip)
    {
        chipsToMove.Remove(chip);

        if (chipsToMove.Count <= 0)
            ChipsShiftCompleted?.Invoke();
    }



    // drops all chips from ActionBar to fall down when ReshuffleService had been used
    public List<Chip> DropCollectedChips()
    {
        List<Chip> chips = new List<Chip>();
        foreach (var s in provider.Slots)
        {
            if (s.state != SlotState.Occupied &&
                s.state != SlotState.Reserved) continue;

            Chip chip = s.chip;
            provider.DestroyChip(s.index);

            chip.transform.SetParent(chipSpawner.transform);
            chip.DropFromActionBar();
            chips.Add(chip);
        }

        return chips;
    }


    public bool HasFlyingChips()
    {
        foreach (var s in provider.Slots)
            if (s.state == SlotState.Reserved)
                return true;

        return false;
    }
    */
}
