using UnityEngine;


[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Chips")]
    [Tooltip("Duration in seconds of chip's flying to ActionBar.")]
    public float flyDuration = 0.5f;
    [Tooltip("Duration in seconds of chip's shifting in ActionBar.")]
    public float chipShiftDuration = 0.15f;
    public float deathDuration = 0.25f;

    [Header("Gameplay")]
    [Tooltip("Number of unique chips in the level.")]
    public int uniqueChips = 3;
    [Tooltip("Number of copies for each unique chip.")]
    [Range(1, 6)]
    public int chipCopies = 3;
    [Tooltip("Delay in seconds before next chip will be spawned on a level.")]
    public float spawnInterval = 0.2f;
    public float delayAfterReshuffle = 1f;    // for all chips to finish falling
    [Tooltip("Threshold of velocity that won't count as chip's movement.")]
    public float stopThreshold = 0.05f;
    [Tooltip("Seconds of delay before next check for chips' movement.")]
    public float checkDelay = 0.2f;

    [Header("UI")]
    public float uiAnimDuration = 0.3f;
}
