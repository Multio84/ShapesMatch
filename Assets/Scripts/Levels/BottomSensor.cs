using System.Collections.Generic;
using UnityEngine;


public class BottomSensor : MonoBehaviour
{
    private readonly HashSet<IChipPhysics> passed = new();

    public void ResetSensor() => passed.Clear();

    //  true, when IChipPhysics collisions quantity is equal to all chips in chipPile
    public bool IsAllPassed(IReadOnlyCollection<Chip> chips)
        => passed.Count >= chips.Count;

    private void OnTriggerEnter2D(Collider2D other)
    {
        var physics = other.GetComponentInParent<IChipPhysics>();
        if (physics is null) return;
        if (other != physics.Collider) return;

        passed.Add(physics);


        Debug.Log($"Passed chips: {passed.Count}");
    }
}
