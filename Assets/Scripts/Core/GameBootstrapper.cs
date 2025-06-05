using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    private IInitializable[] initializables;
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private ChipPartsDatabase chipPartsDatabase;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private ChipFactory chipFactory;
    [SerializeField] private ChipSpawner chipSpawner;
    [SerializeField] private ActionBar actionBar;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Reshuffle reshuffle;
    private ChipPile chipPile;  // all chips of game field (not in action bar)


    private void Awake()
    {
        CreateChipPile();
        Setup();
        Init();
    }

    private void CreateChipPile()
    {
        chipPile = new ChipPile();
    }

    private void Setup()
    {
        if (chipPile is null)
        {
            Debug.LogError("ChipPile is null.");
            return;
        }

        if (gameSettings is null ||
            gameManager is null ||
            uiManager is null ||
            chipPartsDatabase is null ||
            gameplayManager is null ||
            chipFactory is null ||
            chipSpawner is null ||
            actionBar is null ||
            reshuffle is null)
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup(gameplayManager, uiManager, chipSpawner);
        gameplayManager.Setup(gameSettings, chipSpawner, actionBar, uiManager, chipPile);
        uiManager.Setup(gameSettings, reshuffle);
        chipSpawner.Setup(gameSettings, chipFactory, actionBar, chipPile);
        chipFactory.Setup(gameSettings, chipPartsDatabase, gameplayManager, actionBar);
        actionBar.Setup(gameSettings, chipSpawner);
        reshuffle.Setup(gameSettings, uiManager, chipSpawner);
    }

    
    void Init()
    {
        initializables = new IInitializable[]
        {
            gameManager,
            gameplayManager,
            actionBar
        };

        foreach (var obj in initializables)
        {
            obj.Init();
        }
    }
}
