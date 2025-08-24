using System.Collections;
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
        // drop chips from bar
        var chipsFromBar = dropper.DropChips();
        foreach (var chip in chipsFromBar)
            chip.transform.SetParent(spawner.transform);
        chipPile.AddRange(chipsFromBar);
        chipsFromBar = null;

        // let chips fall under screen
        containerBottom.enabled = false;

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            bottomSensor.IsAllPassed(chipPile.Chips));
        bottomSensor.ResetSensor();

        chipPile.FreezeChips();

        containerBottom.enabled = true;

        chipPile.Shuffle();
        spawner.EmitExistingChips();
    }
}
