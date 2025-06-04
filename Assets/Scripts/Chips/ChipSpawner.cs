using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpawnerState
{
    LevelGeneration,
    LevelPlaying
}

public class ChipSpawner : MonoBehaviour
{
    [SerializeField] private Collider2D containerBottom;
    [SerializeField] private BottomSensor bottomSensor;

    private int chipCopies;
    private int uniqueChips;
    private float spawnInterval;
    private int stoppedChipsCount = 0;

    private GameSettings settings;
    private GameplayManager gameplayManager;
    private ChipFactory factory;
    private ActionBar actionBar;
    private List<ChipPassport> passportsDeck;

    private SpawnerState state;

    public event Action<SpawnerState> ChipsStopped;


    public void Setup(GameSettings gs, GameplayManager gm, ChipFactory cf, ActionBar ab)
    {
        settings = gs;
        gameplayManager = gm;
        factory = cf;
        actionBar = ab;

        chipCopies = settings.chipCopies;
        uniqueChips = settings.uniqueChips;
        spawnInterval = settings.chipSpawnInterval;
    }

    public void GenerateLevel()
    {
        state = SpawnerState.LevelGeneration;
        stoppedChipsCount = 0;

        StartCoroutine(SpawnChips());
    }

    private IEnumerator SpawnChips()
    {
        passportsDeck = factory.BuildPassportDeck(uniqueChips, chipCopies);

        foreach (var passport in passportsDeck)
        {
            yield return new WaitForSeconds(spawnInterval);

            Chip chip = factory.SpawnChip(passport, transform);
            gameplayManager.spawnedChips.Add(chip);
            StartChipStopCheck(chip);
        }
    }

    private void StartChipStopCheck(Chip chip)
    {
        chip.Stopped -= HandleChipStopped;
        chip.Stopped += HandleChipStopped;

        chip.StartCheckIfStopped();
    }

    private void HandleChipStopped(Chip chip)
    {
        chip.Stopped -= HandleChipStopped;
        stoppedChipsCount++;

        if (stoppedChipsCount >= gameplayManager.spawnedChips.Count)
        {
            //Debug.Log("All chips stopped.");
            SetChipsInteractable(true);
            ChipsStopped?.Invoke(state);
        }
    }

    private void SetChipsInteractable(bool isInteractable)
    {
        foreach (Chip chip in gameplayManager.spawnedChips)
            chip.isInteractable = isInteractable;
    }

    public void StartReshuffle()
    {
        state = SpawnerState.LevelPlaying;
        stoppedChipsCount = 0;

        StartCoroutine(Reshuffle());
    }

    private IEnumerator Reshuffle()
    {
        SetChipsInteractable(false);

        // let chips fall under screen
        containerBottom.enabled = false;
        bottomSensor.ResetSensor();

        List<Chip> chips = gameplayManager.spawnedChips;
        List<Chip> chipsFromBar = actionBar.DropCollectedChips();
        chips.AddRange(chipsFromBar);

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            bottomSensor.IsAllPassed(chips));

        foreach (Chip chip in chipsFromBar)
            chip.ChangeLayerOrder(false);

        chipsFromBar = null;

        // stop chips' movement
        foreach (Chip chip in chips)
        {
            var rb = chip.rb;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        containerBottom.enabled = true;

        ChipFactory.Shuffle(chips);

        // drop chips again
        foreach (Chip chip in chips)
        {
            yield return new WaitForSeconds(spawnInterval);

            chip.rb.simulated = true;
            chip.transform.position = transform.position;
            StartChipStopCheck(chip);
        }
    }
}