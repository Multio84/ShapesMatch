using UnityEngine;


public class Chip : MonoBehaviour
{
    [Header("ChipComponents")]
    public SpriteRenderer shapeRenderer;
    public SpriteRenderer frameRenderer;
    public SpriteRenderer animalRenderer;


    public void SetShape(Sprite shape)
    {
        if (shapeRenderer is null)
        {
            Debug.LogError($"Chip has no ShapeRenderer.");
            return;
        }

        shapeRenderer.sprite = shape;
    }

    public void SetFrame(Sprite frame, Color color)
    {
        if (frameRenderer is null)
        {
            Debug.LogError("Chip has no FrameRenderer.");
            return;
        }

        frameRenderer.sprite = frame;
        frameRenderer.color = color;
    }

    public void SetFace(Sprite face)
    {
        if (animalRenderer is null)
        {
            Debug.LogError("Chip has no AnimalRenderer.");
            return;
        }

        animalRenderer.sprite = face;
    }
}
