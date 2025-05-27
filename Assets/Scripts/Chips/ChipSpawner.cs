using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    [SerializeField] private Collider2D containerBottom;
    [SerializeField] private BottomSensor bottomSensor;

    [Header("Spawn settings")]
    public const int CHIP_COPIES = 3;
    [SerializeField] private int uniqueChips = 3;
    [SerializeField] private float spawnInterval = 0.2f;

    private GameplayManager gameplayManager;
    private ChipFactory factory;
    private List<ChipPassport> passportsDeck;
    private bool reshuffling;


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
        passportsDeck = factory.BuildPassportDeck(uniqueChips, CHIP_COPIES);

        foreach (ChipPassport passport in passportsDeck)
        {
            Chip chip = factory.SpawnChip(passport, transform);
            gameplayManager.spawnedChips.Add(chip);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void StartReshuffle()
    {
        if (!reshuffling) StartCoroutine(Reshuffle());
    }

    private IEnumerator Reshuffle()
    {
        reshuffling = true;

        // let chips fall
        containerBottom.enabled = false;
        bottomSensor.ResetSensor();

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            bottomSensor.IsAllPassed(gameplayManager.spawnedChips));

        containerBottom.enabled = true;

        ChipFactory.Shuffle(gameplayManager.spawnedChips);

        // drop chips again
        foreach (Chip chip in gameplayManager.spawnedChips)
        {
            chip.transform.position = transform.position;

            yield return new WaitForSeconds(spawnInterval);
        }

        // delay for all chips to finish falling
        yield return new WaitForSeconds(1.5f);

        reshuffling = false;
    }
}