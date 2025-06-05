using System;
using System.Collections.Generic;


public class ChipPile : IChipContainer
{
    private readonly List<Chip> chips = new();
    public int Count => chips.Count;
    public IReadOnlyList<Chip> Chips => chips;

    public event Action<Chip> ChipAdded;
    public event Action<Chip> ChipRemoved;


    public void Add(Chip chip)
    {
        if (chip is null) throw new ArgumentNullException(nameof(chip));

        chips.Add(chip);
        ChipAdded?.Invoke(chip);
    }

    public void AddRange(List<Chip> chipsToAdd)
    {
        if (chipsToAdd is null) throw new ArgumentNullException(nameof(chipsToAdd));

        chips.AddRange(chipsToAdd);
    }

    public bool Remove(Chip chip)
    {
        if (chips.Remove(chip))
        {
            ChipRemoved?.Invoke(chip);
            return true;
        }

        return false;
    }

    public void Clear()
    {
        chips.Clear();
    }

    public void Shuffle()
    {
        for (int i = 0; i < chips.Count; i++)
        {
            int j = UnityEngine.Random.Range(i, chips.Count);
            (chips[i], chips[j]) = (chips[j], chips[i]);
        }
    }
}
