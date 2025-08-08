using UnityEngine;
using System;


public enum GameState
{
    Loading,
    Playing,
    Win,
    Lose
}

public class GameManager : MonoBehaviour, IInitializable
{
    private IBarState barState;//ActionBarController barController;
    private UIController uiController;
    private ChipSpawner spawner;
    private ChipPile chipPile;
    private ReshuffleService reshuffleService;
    private static GameState state;
    //public static GameState State 
    //{
    //    get => state;
    //    private set
    //    {
    //        if (state == value) return;
    //        state = value;

    //        GameStateChanged?.Invoke();
    //    } 
    //}
    //public static event Action GameStateChanged;  


    public void Setup(
        IBarState bs,
        UIController uic,
        ChipSpawner cs,
        ChipPile cp,
        ReshuffleService rs
        )
    {
        barState = bs;
        uiController = uic;
        spawner = cs;
        chipPile = cp;
        reshuffleService = rs;
    }

    public void Init()
    {
        barState.StateChanged += UpdateGameState;

        StartGame();
    }

    private void OnDestroy()
    {
        barState.StateChanged -= UpdateGameState;
    }

    private void StartGame()
    {
        state = GameState.Loading;
        StartLevel();
    }

    private void StartLevel()
    {
        state = GameState.Playing;
        spawner.GenerateLevel();
    }

    private void EndLevel() { }

    private void UpdateGameState(BarState barState)
    {
        if (barState == BarState.Empty && chipPile.IsEmpty)
            state = GameState.Win;

        if (barState == BarState.Full && !reshuffleService.IsAvailable)
            state = GameState.Lose;

        else state = GameState.Playing;

        ApplyGameState();

        // TODO: other states
    }

    private void ApplyGameState()
    {
        if (state == GameState.Win)
        {
            Debug.Log("You Win!");
            chipPile.Dispose();
            Win();
        }
        else if (state == GameState.Lose)
        {
            Debug.Log("Game Over...");
            chipPile.Dispose();
            Lose();
        }
    }

    private void Win() => uiController.Show(WindowKind.Win);

    private void Lose() => uiController.Show(WindowKind.Lose);

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}