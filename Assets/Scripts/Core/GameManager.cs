using UnityEngine;


public class GameManager : MonoBehaviour, IInitializable
{
    private GameplayManager gameplayManager;
    private UIManager uiManager;
    private ChipSpawner chipSpawner;


    public void Setup(GameplayManager gm, UIManager uim, ChipSpawner cs)
    {
        gameplayManager = gm;
        uiManager = uim;
        chipSpawner = cs;
    }

    public void Init()
    {
        chipSpawner.LevelGenerated += OnLevelGenerated;
        uiManager.WindowClosed += OnWindowClosed;

        StartGame();
    }

    private void OnDisable()
    {
        uiManager.WindowClosed -= OnWindowClosed;
    }

    private void StartGame() => gameplayManager.GenerateLevel();

    private void OnLevelGenerated()
    {
        //if (levelNum == 1)
            uiManager.ShowTutorialWindow();
        //else
        //    StartLevel();
    }

    private void OnWindowClosed(WindowKind kind)
    {
        switch (kind)
        {
            case WindowKind.Tutorial:
                StartLevel();
                break;
            case WindowKind.LevelCompletion:
                QuitGame();
                break;
        }
    }

    private void StartLevel() { } // enable chips here



    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}