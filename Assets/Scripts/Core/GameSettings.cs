using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Chips")]
    [Tooltip("Duration in seconds of chip's flying to ActionBar.")]
    public float chipFlyDuration = 0.5f;
    [Tooltip("Duration in seconds of chip's shifting in ActionBar.")]
    public float chipShiftDuration = 0.15f;
    public float chipDeathDuration = 0.25f;
    [Tooltip("Max value of the force applied to the chip after spawning to randomize it's falling direction.")]
    public float chipMaxStartImpulse = 0.1f;

    [Header("Gameplay")]
    [Tooltip("Number of unique chips in the level.")]
    [Range(1, 50)]
    public int uniqueChips = 10;
    [Tooltip("Number of copies for each unique chip.")]
    [Range(1, 6)]
    public int chipCopies = 3;
    [Tooltip("Delay in seconds before next chip will be spawned on a level.")]
    public float chipSpawnInterval = 0.2f;
    [Tooltip("Seconds of delay before next check for chips' movement.")]
    public float chipStopCheckDelay = 0.2f;

    [Header("UI")]
    [Tooltip("Window appearing and disappearing duration.")]
    public float windowAnimDuration = 0.3f;
    [Tooltip("Tutorial element appearing and disappearing duration.")]
    public float tutorialAnimDuration = 0.3f;
    [Tooltip("LevelPlaying icon animation duration.")]
    public float reshuffleAnimDuration = 1f;
}
