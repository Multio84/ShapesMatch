using System;
using UnityEngine;


//public enum SlotState
//{
//    Free,
//    Reserved,
//    Occupied,
//    Matched
//}

public class SlotManager : 
    MonoBehaviour//, IInitializable, ISlotProvider

{
    /*
    [Serializable]
    public sealed class Slot
    {
        public Transform transform;
        [NonSerialized] public int index;
        //[NonSerialized] public SlotState state = SlotState.Free;
        [NonSerialized] public Chip chip;
    }

    public const int SlostCount = 7;
    [SerializeField] private Slot[] slots = new Slot[SlostCount];
    public Slot[] Slots => slots;
    

    public void Init()
    {
        InitSlotsIndices();
    }

    private void InitSlotsIndices()
    {
        for (int i = 0; i < SlostCount; i++)
            slots[i].index = i;
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

    public Slot GetSlot(int idx)
    {
        if (IsValidSlot(idx))
            return GetSlot(idx);

        return null;
    }

    public SlotState GetState(int idx)
    {
        if (!IsValidSlot(idx)) return SlotState.Occupied;

        return GetSlot(idx).state;
    }

    public void SetState(int idx, SlotState state)
    {
        if (IsValidSlot(idx))
        {
            Debug.LogError($"Invalid slotTransform idx: {idx}");
            return;
        }

        GetSlot(idx).state = state;
    }

    public void SetChip(int idx, Chip chip)
    {
        if (IsValidSlot(idx))
        {
            Debug.LogError($"Invalid slotTransform idx: {idx}");
            return;
        }

        GetSlot(idx).chip = chip;
    }

    public void DestroyChip(Chip chip)
    {
        foreach (var s in slots)
            if (s.chip == chip)
            {
                DestroyChip(s.index);
                break;
            }
    }

    public bool HasFreeSlot()
    {
        foreach (var s in slots)
            if (s.state == SlotState.Free)
                return true;

        return false;
    }

    public int GetNextAvailableSlot()
    {
        for (int i = 0; i < SlostCount; i++)
            if (GetState(i) == SlotState.Free)
                return i;

        return -1;
    }

    public int CountPlacedChips()
    {
        int count = 0;
        foreach (var s in slots)
            if (s.state == SlotState.Occupied)
                count++;

        return count;
    }

    public void ReserveSlot(int idx)
    {
        SetState(idx, SlotState.Reserved);
    }

    public void OccupySlot(int idx, Chip chip)
    {
        SetState(idx, SlotState.Occupied);
        SetChip(idx, chip);
    }

    public void DestroyChip(int idx)
    {
        SetChip(idx, null);
        SetState(idx, SlotState.Free);
    }

    // moves chip from current to target place in slots array
    public void RelocateChip(int currentIdx, int targetIdx)
    {
        Chip currentChip = GetSlot(currentIdx).chip;
        SetChip(targetIdx, currentChip);
        DestroyChip(currentIdx);

        //slots[targetIdx].chip = slots[currentIdx].chip;
        //slots[targetIdx].state = SlotState.Occupied;

        //slots[currentIdx].chip = null;
        //slots[currentIdx].state = SlotState.Free;
    }

    public Transform GetSlotTransform(int idx) => GetSlot(idx).transform;
    */
}
