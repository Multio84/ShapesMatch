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
        chipSpawner.ChipsStopped += OnChipsStopped;
        uiManager.WindowClosed += OnWindowClosed;

        StartGame();
    }

    private void OnDisable()
    {
        chipSpawner.ChipsStopped -= OnChipsStopped;
        uiManager.WindowClosed -= OnWindowClosed;
    }

    private void StartGame() => gameplayManager.GenerateLevel();


    private void OnChipsStopped(SpawnerState state)
    {
        if (state == SpawnerState.LevelGeneration)
            uiManager.ShowTutorialWindow();
    }

    private void OnWindowClosed(WindowKind kind)
    {
        switch (kind)
        {
            case WindowKind.Tutorial:
                StartLevel();
                uiManager.reshuffle.SetEnabled(true);
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