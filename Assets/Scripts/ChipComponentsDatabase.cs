using UnityEngine;
using System.Collections.Generic;


public enum ChipShapeType
{
    Square,
    Triangle,
    Circle
}

[CreateAssetMenu(fileName = "ChipComponentsDatabase", menuName = "Game/ChipComponentsDatabase")]
public class ChipPartsDatabase : ScriptableObject
{
    [Header("Shapes")]
    public List<ChipShape> shapes;

    [Header("Frame Colors")]
    public List<Color> frameColors;

    [Header("Animals")]
    public List<Sprite> faceSprites;
}

[System.Serializable]
public class ChipShape
{
    public ChipShapeType shapeType;
    public Sprite shapeSprite;
    public Sprite frameSprite;
}