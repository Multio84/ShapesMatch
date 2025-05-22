using UnityEngine;
using System.Collections.Generic;


public enum ChipShapeType
{
    Square,
    Triangle,
    Circle
}

[CreateAssetMenu(fileName = "ChipPartsDatabase", menuName = "Game/ChipPartsDatabase")]
public class ChipPartsDatabase : ScriptableObject
{
    [Header("Shapes")]
    public List<ChipShape> shapes;

    [Header("Frame Colors")]
    public List<Color> frameColors;

    [Header("Animals")]
    public List<Sprite> faceSprites;

    public ChipShape GetShape(int idx) => 
        (idx >= 0 && idx < shapes.Count) ? shapes[idx] : null;
    public Color GetFrameColor(int idx) => 
        (idx >= 0 && idx < frameColors.Count) ? frameColors[idx] : Color.black;
    public Sprite GetAnimal(int idx) => 
        (idx >= 0 && idx < faceSprites.Count) ? faceSprites[idx] : null;
}

[System.Serializable]
public class ChipShape
{
    public Sprite shapeSprite;
    public Sprite frameSprite;
}