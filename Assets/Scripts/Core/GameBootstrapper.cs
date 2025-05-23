using UnityEngine;


[DefaultExecutionOrder(-100)]
public class GameBootstrapper : MonoBehaviour
{
    private IInitializable[] initializables;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ChipSpawner chipSpawner;


    private void Awake()
    {
        Setup();
        Init();
    }

    private void Setup()
    {
        if (chipSpawner is null ||
            gameManager is null)
        {
            Debug.LogError("GameBootstrapper: Some links are not set in the inspector!");
            return;
        }

        gameManager.Setup(chipSpawner);
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
