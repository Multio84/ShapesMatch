using UnityEngine;

[System.Serializable]
public class Slot
{
    public Transform transform;
    public int index;
}

public class ActionBar : MonoBehaviour
{
    private const int SLOTS_COUNT = 7;
    public Canvas canvas;
    [SerializeField] private Transform placedChipsRoot;
    [SerializeField] private Slot[] slots = new Slot[SLOTS_COUNT];
    private Chip[] placedChips = new Chip[SLOTS_COUNT];


    private void Awake()
    {
        InitSlotIndexes();
    }

    private void InitSlotIndexes()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
            slots[i].index = i;
    }

    public Slot GetNextAvailableSlot()
    {
        for (int i = 0; i < SLOTS_COUNT; i++)
        {
            if (placedChips[i] is null) return slots[i];
        }
        
        Debug.LogError("Attempt to send chip to non-existing slotIdx!");
        return null;
    }

    public void AddChipToPanel(Chip chip, int slotIdx)
    {
        placedChips[slotIdx] = chip;
        chip.transform.SetParent(placedChipsRoot);
    }

    //private void FindMatch()
    //{ 

    //}

    //private void DestroyMatch(Chip[] chipsToDelete)
    //{
    //    foreach (Chip chip in chipsToDelete)
    //    {
    //        Destroy(chip.gameObject);
    //    }
    //}

    //private void MoveChipsToEmptySlots()
    //{
    //    for (int slot = 0; slot < SLOTS_COUNT; slot++)
    //    {
    //        int? targetEmptySlotIdx = null;
    //        if (placedChips[slot] is not null) continue;

    //        targetEmptySlotIdx = slot;

    //        for (int i = targetEmptySlotIdx.Value; i < SLOTS_COUNT; slot++)
    //        {
    //            if (placedChips[i] is null) continue;

    //            Slot targetSlot = slots[targetEmptySlotIdx.Value];
    //            Chip chip = placedChips[i];

    //            placedChips[targetEmptySlotIdx.Value] = placedChips[i];
    //            placedChips[i] = null;

    //            chip.Move(targetSlot);
    //            break;
    //        }
    //    }
    //}
}
