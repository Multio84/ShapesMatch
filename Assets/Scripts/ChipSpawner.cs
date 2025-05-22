using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    public ChipFactory factory;

    void Start()
    {
        SpawnChips();
    }

    void SpawnChips()
    {
        if (factory is null) return;

        for (int i = 0; i < 10; i++)
        {
            factory.GenerateRandomChip();
        }
    }
}
