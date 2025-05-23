using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "ChipPartsDatabase", menuName = "Game/ChipPartsDatabase")]
public class ChipPartsDatabase : ScriptableObject
{
    [Header("Chip Prefabs")]
    public List<Chip> prefabs;

    [Header("Frame Colors")]
    public List<Color> frameColors;

    [Header("Animal Sprites")]
    public List<Sprite> animalSprites;


    public Chip GetPrefab(int idx) => prefabs[idx];
    public Color GetColor(int idx) => frameColors[idx];
    public Sprite GetAnimal(int idx) => animalSprites[idx];
}