using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;


public class ReshuffleService : MonoBehaviour
{
    [SerializeField] private Collider2D _containerBottom;
    [SerializeField] private BottomSensor _bottomSensor;

    public bool HasBeenUsed => _hasBeenUsed;

    public bool CanExecute => _canExecute;
    public bool IsAvailable => _availableCount > 0 ? true : false;
    public int AvailableCount => _availableCount;

    private ChipPile chipPile;
    private IChipDropper dropper;
    private ChipSpawner spawner;
    private ChipMonitor monitor;
    private ReshuffleButton button;
    private int _availableCount = 3;    // number of reshuffles, available in current level
    private bool _canExecute = false;
    private bool _hasBeenUsed = false;

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
        if (!_hasBeenUsed) _hasBeenUsed = true;
        if (!_canExecute) return;
        _canExecute = false;
        _availableCount--;

        StartCoroutine(Reshuffle());
    }

    private void OnChipsFallingStarted()
    {
        //_canExecute = false;
        button.UpdateInteractable();
    }
    
    private void MakeExecutable()
    {
        if (!IsAvailable) return;

        _canExecute = true;
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

        /*
         Общая идея:
        1. удаляем чипы из бар модел и возвращаем бар контроллеру
        2. бар контроллер просто возвращает все фишки, которые были удалены с бара решафлу
        3. решафл говорит чип пайлу всё дропнуть
        4. чиппайл просто устанавливает всем Falling: это убивает анимации, включает физику, норм сортирует отрисовку и всё ок по идее
         */

        // drop all chips
        chipPile.DropChips();

        // let chips fall under screen
        _containerBottom.enabled = false;

        // check that all chips intersected screen bottom
        yield return new WaitUntil(() =>
            _bottomSensor.IsAllPassed(chipPile.Chips));
        _bottomSensor.ResetSensor();

        chipPile.FreezeChips();

        _containerBottom.enabled = true;

        chipPile.Shuffle();
        spawner.EmitExistingChips();
    }
}
