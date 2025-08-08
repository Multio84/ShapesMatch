using System;


public interface IChipCollector
{ 
    void TryCollectChip(Chip chip);
    event Action<Chip> ChipCollected;
}