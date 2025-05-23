using UnityEngine;


public class GameManager : MonoBehaviour, IInitializable
{
    private ChipSpawner chipSpawner;


    public void Setup(ChipSpawner chipSpawner)
    {
        this.chipSpawner = chipSpawner;
    }

    public void Init()
    {
        chipSpawner.GenerateLevel();
    }
}
