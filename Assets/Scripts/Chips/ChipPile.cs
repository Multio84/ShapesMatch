using System;
using System.Collections.Generic;
using UnityEngine;


public class ChipPile : IDisposable
{
    public event Action<Chip> ChipAdded;
    public event Action<Chip> ChipRemoved;
 
    private IChipCollector collector;
    private readonly List<Chip> chips = new();

    public int Count => chips.Count;
    public bool IsEmpty => chips.Count == 0;
    public IReadOnlyList<Chip> Chips => chips;
    
    public void Setup(IChipCollector cc)
    {
        collector = cc;

        collector.ChipCollected += Remove;
    }

    public void Dispose()
    {
        collector.ChipCollected -= Remove;
    }

    public void SetInteractable(bool isInteractable)
    {
        foreach (Chip chip in chips)
            chip.IsInteractable = isInteractable;
    }

    public void Add(Chip chip)
    {
        if (!chip) throw new ArgumentNullException(nameof(chip));

        chips.Add(chip);
        ChipAdded?.Invoke(chip);
    }

    public void AddRange(List<Chip> chipsToAdd)
    {
        if (chipsToAdd is null) throw new ArgumentNullException(nameof(chipsToAdd));

        chips.AddRange(chipsToAdd);
    }

    public void Remove(Chip chip)
    {
        if (!chips.Remove(chip))
            Debug.LogError("Attempt to delete non-existing chip from chip pile.");
       
        ChipRemoved?.Invoke(chip);
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

    public void StopChips()
    {
        foreach (Chip chip in chips)
        {
            var rb = chip.rb;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }
    }
}
