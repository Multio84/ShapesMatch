using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private ChipFactory factory;

    [Header("Spawn settings")]
    [SerializeField] private int uniqueChips = 3;
    [SerializeField] private int chipCopies = 3;
    [SerializeField] private float spawnInterval = 0.2f;

    private List<ChipPassport> passportsDeck;
    private List<Chip> spawnedChips = new List<Chip>();


    public void GenerateLevel()
    {
        StartCoroutine(SpawnChips());
    }

    private IEnumerator SpawnChips()
    {
        if (factory is null)
        {
            Debug.LogError("ChipSpawner: ChipFactory has no link.");
            yield break;
        }

        passportsDeck = factory.BuildPassportDeck(uniqueChips, chipCopies);

        foreach (ChipPassport passport in passportsDeck)
        {
            Chip chip = factory.SpawnChip(passport, transform);
            spawnedChips.Add(chip);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

}