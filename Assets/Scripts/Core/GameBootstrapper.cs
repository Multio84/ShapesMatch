using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    private IInitializable[] initializables;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ChipFactory chipFactory;
    [SerializeField] private ChipSpawner chipSpawner;
    [SerializeField] private ActionBar actionBar;


    private void Awake()
    {
        Setup();
        Init();
    }

    private void Setup()
    {
        if (gameManager is null ||
            chipFactory is null ||
            chipSpawner is null ||
            actionBar is null)
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup(chipSpawner);
        chipFactory.Setup(actionBar);
    }

    
    void Init()
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
