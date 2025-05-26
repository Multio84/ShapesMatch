using System.Collections.Generic;
using UnityEngine;


public enum SlotState
{
    Free,
    Reserved,
    Occupied,
    Matched
}

[System.Serializable]
public class Slot
{
    public Transform transform;
    [System.NonSerialized] public int index;
    //[System.NonSerialized] 
    public SlotState state = SlotState.Free;
    [System.NonSerialized] public Chip chip;
}

public class GamePanel : MonoBehaviour, IInitializable
{
    public const int SLOTS_COUNT = 7;
    public Canvas canvas;
    [SerializeField] private Transform chipsRoot;
    [SerializeField] private Slot[] slots = new Slot[SLOTS_COUNT];


    public void Init()
    {
        InitSlotsIndices();
    }

    private void InitSlotsIndices()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
            slots[i].index = i;
    }

    public int GetNextAvailableSlot()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (slots[i].state == SlotState.Free)
                return i;
        }
        
        Debug.LogError("Attempt to send chip to non-existing slotIdx!");
        return -1;
    }

    public void ReserveSlot(int idx, Chip chip)
    {
        slots[idx].state = SlotState.Reserved;
    }

    public void PlaceChip(int idx, Chip chip)
    {
        slots[idx].state = SlotState.Occupied;
        slots[idx].chip = chip;
        slots[idx].chip.transform.SetParent(chipsRoot);
    }

    private void DestroyChip(int idx)
    {
        Destroy(slots[idx].chip.gameObject);
        slots[idx].chip = null;
        slots[idx].state = SlotState.Free;
    }

    // moves chip from current to target place in slots array
    private void RelocateChip(int currentIdx, int targetIdx)
    {
        slots[targetIdx].chip = slots[currentIdx].chip;
        slots[targetIdx].state = SlotState.Occupied;

        slots[currentIdx].chip = null;
        slots[currentIdx].state = SlotState.Free;
    }

    public bool HasFlyingChips()
    {
        foreach (var s in slots)
            if (s.state == SlotState.Reserved)
                return true;
        
        return false;
    }

    public int CountPlacedChips()
    {
        int count = 0;
        foreach (var s in slots)
            if (s.state == SlotState.Occupied)
                count++;

        return count;
    }

    public Transform GetSlotTransform(int idx) => slots[idx].transform;

    public bool FindMatches()
    {
        bool wasMatched = false;

        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (slots[i].state != SlotState.Occupied) continue;

            int matchCount = 1;
            List<int> matchedIndices = new List<int> { i };

            for (int j = i + 1; j < SLOTS_COUNT; j++)
            {
                if (slots[j].state != SlotState.Occupied) continue;

                if (slots[i].chip.Passport.IsSameAs(slots[j].chip.Passport))
                {
                    matchedIndices.Add(j);
                    matchCount++;
                }

                if (matchCount == 3)
                    break;
            }

            if (matchCount == 3)
                foreach (int idx in matchedIndices)
                {
                    slots[idx].state = SlotState.Matched;
                    wasMatched = true;
                }
        }

        return wasMatched;
    }

    public void DestroyMatches()
    {
        foreach (var s in slots)
            if (s.state == SlotState.Matched)
                DestroyChip(s.index);
    }

    public void MoveChipsToEmptySlots()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (slots[i].state == SlotState.Occupied) continue;

            for (int j = i + 1; j < SLOTS_COUNT; j++)
            {
                if (slots[j].state == SlotState.Free) continue;

                Transform target = slots[i].transform;
                Chip chip = slots[j].chip;
                RelocateChip(j, i);

                chip.Move(target);

                break;
            }
        }
    }
}
