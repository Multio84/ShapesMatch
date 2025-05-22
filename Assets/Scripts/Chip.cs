using UnityEngine;



[System.Serializable]
public struct ChipData
{
    public int shapeIdx;
    public int frameColorIdx;
    public int animalIdx;

    public ChipData(int shape, int color, int animal)
    {
        shapeIdx = shape;
        frameColorIdx = color;
        animalIdx = animal;
    }

    public bool IsSameAs(ChipData other)
    {
        return shapeIdx == other.shapeIdx &&
               frameColorIdx == other.frameColorIdx &&
               animalIdx == other.animalIdx;
    }
}

public class Chip : MonoBehaviour
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer shapeRenderer;
    [SerializeField] private SpriteRenderer frameRenderer;
    [SerializeField] private SpriteRenderer animalRenderer;

    private ChipData data;
    public ChipData Data => data;

    public void Init(ChipData data, ChipPartsDatabase db)
    {
        this.data = data;

        // shape
        ChipShape shape = db.GetShape(data.shapeIdx);
        shapeRenderer.sprite = shape.shapeSprite;

        // frame color
        frameRenderer.sprite = shape.frameSprite;
        frameRenderer.color = db.GetFrameColor(data.frameColorIdx);

        // animal
        animalRenderer.sprite = db.GetAnimal(data.animalIdx);
    }
}