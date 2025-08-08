using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : IInitializable
{
    private int currentLevel;
    public int Level => currentLevel;

    private Level[] levels;



    public void Setup()
    {
        
    }

    public void Init()
    {

    }

    public void StartGame()
    {
        currentLevel = 1;
    }

    public bool IsFirstLevel()
    {
        return Level == 1;
    }

    public void StartLevel(int level)
    {

    }
}
