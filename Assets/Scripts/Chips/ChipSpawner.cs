using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    [Header("Spawn settings")]
    [SerializeField] public const int CHIP_COPIES = 3;
    [SerializeField] private int uniqueChips = 3;
    [SerializeField] private const float SPAWN_INTERVAL = 0.2f;

    private GameplayManager gameplayManager;
    private ChipFactory factory;
    private List<ChipPassport> passportsDeck;
    

    public void Setup(GameplayManager gm, ChipFactory cf)
    {
        gameplayManager = gm;
        factory = cf;
    }

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

        passportsDeck = factory.BuildPassportDeck(uniqueChips, CHIP_COPIES);

        foreach (ChipPassport passport in passportsDeck)
        {
            Chip chip = factory.SpawnChip(passport, transform);
            gameplayManager.spawnedChips.Add(chip);

            yield return new WaitForSeconds(SPAWN_INTERVAL);
        }
    }

}