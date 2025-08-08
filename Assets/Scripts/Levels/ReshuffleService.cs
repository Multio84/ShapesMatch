using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReshuffleService : MonoBehaviour
{
    [SerializeField] private Collider2D containerBottom;
    [SerializeField] private BottomSensor bottomSensor;

    public bool hasBeenUsed = false;

    public bool CanExecute => canExecute;
    public bool IsAvailable => availableCount > 0 ? true : false;

    private ChipPile chipPile;
    private IChipDropper dropper;
    private ChipSpawner spawner;
    private ChipMonitor monitor;
    private ReshuffleButton button;
    private int availableCount = 10;    // number of reshuffles, available in current level
    private bool canExecute = false;

    public void Setup(
        ChipPile cp,
        IChipDropper cd,
        ChipSpawner cs, 
        ChipMonitor cm,
        ReshuffleButton rb
        )
    {
        chipPile = cp;
        spawner = cs;
        monitor = cm;
        dropper = cd;
        button = rb;
    }

    private void Awake()
    {
        spawner.ChipsFallingStarted += OnChipsFallingStarted;
        monitor.ChipsStopped += MakeExecutable;
        button.Pressed += Execute;
    }

    private void OnDestroy()
    {
        spawner.ChipsFallingStarted -= OnChipsFallingStarted;
        monitor.ChipsStopped -= MakeExecutable;
        button.Pressed -= Execute;
    }

    public void Execute()
    {
        hasBeenUsed = true;
        availableCount--;

        StartCoroutine(Reshuffle());
    }

    private void OnChipsFallingStarted() => canExecute = false;
    
    private void MakeExecutable()
    {
        if (!IsAvailable) return;

        canExecute = true;
        button.UpdateInteractable();
    }

    private IEnumerator Reshuffle()
    {
        chipPile.SetInteractable(false);

        List<Chip> chipsFromBar = dropper.DropChips();
        foreach (var chip in chipsFromBar)
            chip.transform.SetParent(spawner.transform);
        chipPile.AddRange(chipsFromBar);

        // let chips fall under screen
        containerBottom.enabled = false;
        bottomSensor.ResetSensor();

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            bottomSensor.IsAllPassed(chipPile.Chips));

        foreach (Chip chip in chipsFromBar)
            chip.SetPhysEnabled(true);//PrepareForFlight(true, false, bar._view.LayerOrder);
        chipsFromBar = null;

        chipPile.StopChips();

        containerBottom.enabled = true;

        chipPile.Shuffle();
        spawner.EmitExistingChips();
    }
}
