using UnityEngine;


public class GameManager : MonoBehaviour, IInitializable
{
    private GameplayManager gameplayManager;
    private ChipSpawner chipSpawner;


    public void Setup(GameplayManager gm, ChipSpawner cs)
    {
        gameplayManager = gm;
        chipSpawner = cs;
    }

    public void Init()
    {
        chipSpawner.GenerateLevel();
    }
}
