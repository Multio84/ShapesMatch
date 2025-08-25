using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private ChipPartsDatabase chipPartsDatabase;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ActionBarController barController;
    [SerializeField] private ActionBarView barView;
    [SerializeField] private ChipFactory chipFactory;
    [SerializeField] private ChipSpawner chipSpawner;
    [SerializeField] private UIController uiController;
    [SerializeField] private ReshuffleService reshuffleService;
    [SerializeField] private ReshuffleButton reshuffleButton;
    [SerializeField] private TutorialUIController tutorialUI;

    private IInitializable[] initializables;
    private ActionBarModel barModel;
    private ChipPile chipPile;
    private ChipMonitor chipMonitor;
    private TutorialManager tutorial;

    private void Awake()
    {
        CreateObjects();
        Setup();
        Init();
    }

    private void CreateObjects()
    {
        barModel = new ActionBarModel();
        chipPile = new ChipPile();
        chipMonitor = new ChipMonitor();
        tutorial = new TutorialManager();
    }

    private void Setup()
    {
        if (!gameSettings ||
            !chipPartsDatabase ||
            !gameManager ||
            !barController ||
            !barView ||
            !chipFactory ||
            !chipSpawner ||
            !uiController ||
            !reshuffleService ||
            !reshuffleButton ||
            !tutorialUI
            )
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup((IBarState)barController, uiController, chipSpawner, chipPile,reshuffleService);
        barController.Setup(gameSettings, barView, barModel);
        barView.Setup(gameSettings);
        barModel.Setup(gameSettings);
        chipFactory.Setup(gameSettings, chipPartsDatabase, (IChipCollector)barController);
        chipSpawner.Setup(gameSettings, chipFactory, chipPile, chipMonitor);
        uiController.Setup(gameSettings);
        reshuffleService.Setup(chipPile, (IChipDropper)barController, chipSpawner, chipMonitor, reshuffleButton);
        reshuffleButton.Setup(gameSettings, reshuffleService);
        chipMonitor.Setup(chipPile);
        chipPile.Setup((IChipCollector)barController);
    }
    
    private void Init()
    {
        initializables = new IInitializable[]
        {
            gameManager
        };

        foreach (var obj in initializables)
        {
            obj.Init();
        }
    }
}
