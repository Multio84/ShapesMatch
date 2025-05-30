using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChipSpawner : MonoBehaviour
{
    [SerializeField] private Collider2D containerBottom;
    [SerializeField] private BottomSensor bottomSensor;

    private int chipCopies;
    private int uniqueChips;
    private float spawnInterval;
    private float delayAfterReshuffle;
    private int stoppedChipsCount = 0;

    private GameSettings settings;
    private GameplayManager gameplayManager;
    private ChipFactory factory;
    private List<ChipPassport> passportsDeck;
    private bool reshuffling;

    public event Action LevelGenerated;


    public void Setup(GameSettings gs, GameplayManager gm, ChipFactory cf)
    {
        settings = gs;
        gameplayManager = gm;
        factory = cf;

        chipCopies = settings.chipCopies;
        uniqueChips = settings.uniqueChips;
        spawnInterval = settings.spawnInterval;
        delayAfterReshuffle = settings.delayAfterReshuffle;
    }

    public void GenerateLevel()
    {
        StartCoroutine(SpawnChips());
    }

    private IEnumerator SpawnChips()
    {
        passportsDeck = factory.BuildPassportDeck(uniqueChips, chipCopies);

        foreach (var passport in passportsDeck)
        {
            Chip chip = factory.SpawnChip(passport, transform);

            gameplayManager.spawnedChips.Add(chip);
            chip.Stopped += HandleChipStopped;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void HandleChipStopped(Chip chip)
    {
        chip.Stopped -= HandleChipStopped;
        stoppedChipsCount++;

        if (stoppedChipsCount >= gameplayManager.spawnedChips.Count)
        {
            Debug.Log("All chips stopped.");
            LevelGenerated?.Invoke();
        }
    }

    public void StartReshuffle()
    {
        if (!reshuffling) StartCoroutine(Reshuffle());
    }

    private IEnumerator Reshuffle()
    {
        reshuffling = true;

        // let chips fall under screen
        containerBottom.enabled = false;
        bottomSensor.ResetSensor();

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            bottomSensor.IsAllPassed(gameplayManager.spawnedChips));

        // stop chips' movement
        foreach (Chip chip in gameplayManager.spawnedChips)
        {
            var rb = chip.rb;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        containerBottom.enabled = true;

        ChipFactory.Shuffle(gameplayManager.spawnedChips);

        // drop chips again
        foreach (Chip chip in gameplayManager.spawnedChips)
        {
            var rb = chip.rb;
            rb.simulated = true;

            chip.transform.position = transform.position;

            yield return new WaitForSeconds(spawnInterval);
        }

        
        yield return new WaitForSeconds(delayAfterReshuffle);

        reshuffling = false;
    }
}