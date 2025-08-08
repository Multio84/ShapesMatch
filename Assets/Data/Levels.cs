using UnityEngine;


[System.Serializable]
public class Level
{
    public int number;
    public int chipsCount;
}

[CreateAssetMenu(fileName = "Levels", menuName = "Game/Levels")]
public class Levels : ScriptableObject
{
    public Levels[] levels;
}
