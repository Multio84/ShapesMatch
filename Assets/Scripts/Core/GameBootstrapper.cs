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


    private void Awake()
    {
        Setup();
        Init();
    }

    private void Setup()
    {
        if (gameSettings is null ||
            gameManager is null ||
            uiManager is null ||
            chipPartsDatabase is null ||
            gameplayManager is null ||
            chipFactory is null ||
            chipSpawner is null ||
            actionBar is null)
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup(gameplayManager, uiManager, chipSpawner);
        gameplayManager.Setup(gameSettings, chipSpawner, actionBar, uiManager);
        uiManager.Setup(gameSettings);
        chipSpawner.Setup(gameSettings, gameplayManager, chipFactory);
        chipFactory.Setup(gameSettings, chipPartsDatabase, gameplayManager, actionBar);
        actionBar.Setup(gameSettings);
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
