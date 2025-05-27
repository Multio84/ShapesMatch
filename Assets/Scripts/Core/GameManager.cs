using UnityEngine;
using static UnityEngine.Application;


public class GameManager : MonoBehaviour, IInitializable
{
    private GameplayManager gameplayManager;
    private ChipSpawner chipSpawner;
    private UIManager uiManager;


    public void Setup(GameplayManager gm, ChipSpawner cs, UIManager uim)
    {
        gameplayManager = gm;
        chipSpawner = cs;
        uiManager = uim;
    }

    public void Init()
    {
        uiManager.WindowClosed += QuitGame;

        StartGame();
    }

    private void OnDisable()
    {
        uiManager.WindowClosed -= QuitGame;
    }

    private void StartGame() => gameplayManager.GenerateLevel();

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}