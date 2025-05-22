using UnityEngine;


public class ChipFactory : MonoBehaviour
{
    public ChipPartsDatabase database;
    public GameObject chipPrefab;


    public void GenerateRandomChip()
    {
        // Get data
        ChipShape shapeDef = GetRandom(database.shapes);
        Color frameColor = GetRandom(database.frameColors);
        Sprite faceSprite = GetRandom(database.faceSprites);

        // Spawn and set
        GameObject chipGO = Instantiate(chipPrefab);
        Chip chip = chipGO.GetComponent<Chip>();

        chip.SetShape(shapeDef.shapeSprite);
        chip.SetFrame(shapeDef.frameSprite, frameColor);
        chip.SetFace(faceSprite);
    }

    private T GetRandom<T>(System.Collections.Generic.List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    
}