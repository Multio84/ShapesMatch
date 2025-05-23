using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    [SerializeField] private ChipFactory factory;
    

    void Start()
    {
        SpawnChips();
    }

    void SpawnChips()
    {
        if (factory is null) return;

        for (int i = 0; i < 50; i++)
        {
            Chip chip = factory.SpawnUniqueChip(transform);
        }
    }
}