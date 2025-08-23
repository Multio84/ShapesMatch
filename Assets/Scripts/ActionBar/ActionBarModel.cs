using System.Collections.Generic;
using UnityEngine;


// for moving chips inside action bar
public struct MoveInfo
{
    public Chip Chip;
    public int From;
    public int To;
}

// contains chips, deleted in array, to animate removal
// and chips, moved in array, to animate movement
public struct CollapseInfo
{
    public List<Chip> Matches;
    public List<MoveInfo> Moves;

    // there can't be Moves for absent Matches
    public bool IsEmpty => (Matches is null || Matches.Count == 0);
}

//public interface IActionBarModel
//{
//    bool TryBeginPlacement(out int idx);        // reserves slot if possible
//    void CommitPlacement(Chip chip, int idx);   // place chip in the slot
//    CollapseInfo BuildCollapse();               // collect info for destroying chips and shifting left if needed
//}

public class ActionBarModel
{
    public enum SlotState
    {
        Free,
        Reserved,
        Occupied,
        Matched,
        None
    }

    public const int SlotsCount = 7;

    private sealed class Slot
    {
        public Chip Chip;
        public SlotState State = SlotState.Free;
    }

    private GameSettings settings;
    private readonly Slot[] slots = new Slot[SlotsCount];
    private int chipCopies;


    public void Setup(GameSettings gs)
    {
        settings = gs;
        chipCopies = settings.chipCopies;

        CreateSlots();
    }

    /* ----- IActionBarModel ----- */

    public bool TryBeginPlacement(out int slotIdx)
    {
        slotIdx = GetFirstFreeSlotIdx();
        if (slotIdx < 0 || HasFlyingChips()) return false;

        SetState(slotIdx, SlotState.Reserved);
        ReserveSlot(slotIdx);
        return true;
    }

    public void CommitPlacement(Chip chip, int idx)
    {
        SetChip(idx, chip);
        SetState(idx, SlotState.Occupied);
    }

    public CollapseInfo BuildCollapse()
    {
        // find matchedSlots
        List<int> matchedSlots = FindMatchedSlots();
        if (matchedSlots.Count == 0)
            return default;

        // collect matches and destroy them
        var matches = new List<Chip>();
        foreach (int idx in matchedSlots)
        {
            matches.Add(GetChip(idx));
            RemoveChip(idx);
        }

        // collect move info
        var moves = FindMoves();

        return new CollapseInfo
        {
            Matches = matches,
            Moves = moves
        };
    }

    public int CountPlacedChips()
    {
        int count = 0;
        foreach (var s in slots)
            if (s.State == SlotState.Occupied)
                count++;

        return count;
    }

    public bool HasFlyingChips()
    {
        foreach (var s in slots)
            if (s.State == SlotState.Reserved)
                return true;

        return false;
    }

    public List<Chip> RemoveAllChips()
    {
        List<Chip> chips = new();
        for (int i = 0; i < SlotsCount; i++)
        {
            //if (slots[i].State != SlotState.Occupied &&
            //    slots[i].State != SlotState.Reserved) continue;
            var chip = GetChip(i);
            if (chip)
            {
                chips.Add(chip);
                RemoveChip(i);
            }
        }

        return chips;
    }

    /* ----- private API ----- */

    private void CreateSlots()
    {
        for (int i = 0; i < SlotsCount; i++)
            slots[i] = new Slot();
    }

    private List<int> FindMatchedSlots()
    {
        List<int> matches = new();

        for (int i = 0; i < SlotsCount; i++)
        {
            if (GetState(i) != SlotState.Occupied) continue;
            matches.Add(i);

            for (int j = i + 1; j < SlotsCount; j++)
            {
                if (GetState(j) != SlotState.Occupied) continue;

                if (GetChip(i).Passport.IsSameAs(GetChip(j).Passport))
                    
                    matches.Add(j);

                if (matches.Count == chipCopies)
                    return matches;
            }

            matches.Clear();
        }

        return matches;
    }

    private List<MoveInfo> FindMoves()
    {
        List<MoveInfo> moves = new();

        for (int i = 0; i < SlotsCount; i++)
        {
            if (GetState(i) != SlotState.Free) continue;

            for (int j = i + 1; j < SlotsCount; j++)
            {
                if (GetState(j) != SlotState.Occupied) continue;

                MoveInfo move = new();
                move.Chip = GetChip(j);
                move.From = j;
                move.To = i;

                moves.Add(move);
                RelocateChip(j, i);

                break;
            }
        }

        return moves;
    }

    private int GetFirstFreeSlotIdx()
    {
        for (int i = 0; i < SlotsCount; i++)
            if (GetState(i) == SlotState.Free)
                return i;

        return -1;
    }

    private void ReserveSlot(int idx)
    {
        SetState(idx, SlotState.Reserved);
    }

    // moves chip from current to target place in slots array
    private void RelocateChip(int currentIdx, int targetIdx)
    {
        Chip currentChip = GetChip(currentIdx);
        SetChip(targetIdx, currentChip);
        RemoveChip(currentIdx);
    }

    private void RemoveChip(int idx)
    {
        SetChip(idx, null);
        SetState(idx, SlotState.Free);
    }

    private Chip GetChip(int idx)
    {
        if (!IsValidSlot(idx))
            return null;

        return GetSlot(idx).Chip;
    }

    private bool SetChip(int idx, Chip chip)
    {
        if (!IsValidSlot(idx))
            return false;

        GetSlot(idx).Chip = chip;
        return true;
    }

    private SlotState GetState(int idx)
    {
        if (!IsValidSlot(idx))
            return SlotState.None;

        return GetSlot(idx).State;
    }

    private void SetState(int idx, SlotState state)
    {
        GetSlot(idx).State = state;
    }

    private Slot GetSlot(int idx)
    {
        if (IsValidSlot(idx))
            return slots[idx];

        return null;
    }

    private bool IsValidSlot(int idx)
    {
        if (idx < 0 || idx >= slots.Length)
        {
            Debug.LogError($"Invalid slotTransform idx: {idx}");
            return false;
        }

        return true;
    }

}

