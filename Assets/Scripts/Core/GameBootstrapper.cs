using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    private IInitializable[] initializables;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ChipPartsDatabase chipPartsDatabase;
    [SerializeField] private GameplayManager gameplayManager;
    [SerializeField] private ChipFactory chipFactory;
    [SerializeField] private ChipSpawner chipSpawner;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private UIManager uiManager;


    private void Awake()
    {
        Setup();
        Init();
    }

    private void Setup()
    {
        if (gameManager is null ||
            chipPartsDatabase is null ||
            gameplayManager is null ||
            chipFactory is null ||
            chipSpawner is null ||
            gamePanel is null ||
            uiManager is null)
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup(gameplayManager, uiManager);
        gameplayManager.Setup(chipSpawner, gamePanel, uiManager);
        chipSpawner.Setup(gameplayManager, chipFactory);
        chipFactory.Setup(chipPartsDatabase, gameplayManager, gamePanel);
    }

    
    void Init()
    {
        initializables = new IInitializable[]
        {
            gameManager,
            gameplayManager,
            gamePanel
        };

        foreach (var obj in initializables)
        {
            obj.Init();
        }
    }
}
