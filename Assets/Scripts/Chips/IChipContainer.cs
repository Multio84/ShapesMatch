using System.Collections.Generic;
using System;


public interface IChipContainer
{
    int Count { get; }
    IReadOnlyList<Chip> Chips { get; }
    void Add(Chip chip);
    void AddRange(List<Chip> chipsToAdd);
    bool Remove(Chip chip);
    void Clear();
    void Shuffle();
    event Action<Chip> ChipAdded;
    event Action<Chip> ChipRemoved;
}
