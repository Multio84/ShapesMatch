using System.Collections.Generic;
using UnityEngine;


public class BottomSensor : MonoBehaviour
{
    private readonly HashSet<Chip> passed = new HashSet<Chip>();


    public void ResetSensor() => passed.Clear();

    public bool IsAllPassed(IReadOnlyCollection<Chip> chips)
        => passed.Count == chips.Count;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Chip chip = other.GetComponentInParent<Chip>();

        // Добавляем фишку, если пришёл её «главный» коллайдер.
        if (chip is not null && other == chip.col)
            passed.Add(chip);
    }
}
