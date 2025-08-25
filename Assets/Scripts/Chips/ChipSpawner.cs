using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ChipSpawner : MonoBehaviour
{
    public event Action ChipsFallingStarted;

    private int chipCopies;
    private int uniqueChips;
    private float spawnInterval;
    private float chipMaxImpulse;

    private GameSettings settings;
    private ChipFactory factory;
    private ChipPile chipPile;
    private ChipMonitor monitor;
    
    public void Setup(GameSettings gs, ChipFactory cf, ChipPile cp, ChipMonitor cm)
    {
        settings = gs;
        factory = cf;
        chipPile = cp;
        monitor = cm;

        chipCopies = settings.chipCopies;
        uniqueChips = settings.uniqueChips;
        spawnInterval = settings.chipSpawnInterval;
        chipMaxImpulse = settings.chipMaxStartImpulse;
    }

    public void GenerateLevel()
    {
        ChipsFallingStarted?.Invoke();
        StartCoroutine(SpawnChips());
    }

    private IEnumerator SpawnChips()
    {
        List<ChipPassport> passportsDeck = factory.BuildPassportDeck(uniqueChips, chipCopies);

        foreach (var passport in passportsDeck)
        {
            yield return new WaitForSeconds(spawnInterval);

            Chip chip = factory.SpawnChip(passport, transform);
            chip.SetState(ChipState.Emitting);
            chipPile.Add(chip);

            monitor.StartChipStopCheck(chip);
        }
    }

    public void EmitExistingChips()
    {
        ChipsFallingStarted?.Invoke();
        StartCoroutine(PlaceChips());
    }

    // relocate chips of pile to spawner pos again (after reshuffle)
    private IEnumerator PlaceChips()
    {
        foreach (Chip chip in chipPile.Chips)
        {
            yield return new WaitForSeconds(spawnInterval);

            chip.transform.position = transform.position;
            chip.SetState(ChipState.Emitting);

            monitor.StartChipStopCheck(chip);
        }
    }

    // prevents falling in one line
    //private void AddRandomHorizontalImpulse(Chip chip)
    //{
    //    float imp = UnityEngine.Random.Range(-chipMaxImpulse, chipMaxImpulse);
    //    chip.rb.AddForce(new Vector2(imp, 0f), ForceMode2D.Impulse);
    //}
    //private void AddRandomHorizontalImpulse(float maxImp)
    //{
    //    float imp = UnityEngine.Random.Range(-chipMaxImpulse, chipMaxImpulse);
    //    chip.rb.AddForce(new Vector2(imp, 0f), ForceMode2D.Impulse);
    //}

}