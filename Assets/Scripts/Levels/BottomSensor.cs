using System.Collections.Generic;
using UnityEngine;


public class BottomSensor : MonoBehaviour
{
    private readonly HashSet<Chip> passed = new();

    public void ResetSensor() => passed.Clear();

    public bool IsAllPassed(IReadOnlyCollection<Chip> chips)
        => passed.Count == chips.Count;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Chip chip = other.GetComponentInParent<Chip>();

        if (chip && other == chip.col)
            passed.Add(chip);
    }
}
