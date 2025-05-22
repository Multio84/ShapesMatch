using System.Collections.Generic;
using UnityEngine;


public class ChipFactory : MonoBehaviour
{
    [SerializeField] private ChipPartsDatabase database;
    [SerializeField] private Chip chipPrefab;

    private List<ChipData> selectedChipsData = new List<ChipData>();
    private int maxUniqueChipsCount;

    public Chip SpawnUniqueChip(Vector3 worldPos)
    {
        maxUniqueChipsCount = CountMaxUniqueCombinations(database);
        ChipData data = GetUniqueRandomChipData();

        Chip chip = Instantiate(chipPrefab, worldPos, Quaternion.identity);
        chip.Init(data, database);
        return chip;
    }

    private int CountMaxUniqueCombinations(ChipPartsDatabase database)
    {
        return database.shapes.Count * 
            database.frameColors.Count * 
            database.faceSprites.Count;
    }

    private ChipData GetUniqueRandomChipData()
    {
        const int ATTEMPT_LIMIT_COEF = 3;
        int attemptsLimit = maxUniqueChipsCount * ATTEMPT_LIMIT_COEF;
        int attempts = 0;

        while (attempts < attemptsLimit)
        {
            ChipData candidate = MakeRandomChipData();

            if (!ContainsData(candidate))
            {
                selectedChipsData.Add(candidate);
                return candidate;
            }

            attempts++;
        }

        Debug.LogWarning("Couldn't find unique chip: returned duplicate.");
        return MakeRandomChipData();
    }

    private ChipData MakeRandomChipData()
    {
        int shape = Random.Range(0, database.shapes.Count);
        int color = Random.Range(0, database.frameColors.Count);
        int animal = Random.Range(0, database.faceSprites.Count);

        return new ChipData(shape, color, animal);
    }

    private bool ContainsData(ChipData data)
    {
        foreach (ChipData saved in selectedChipsData)
        {
            if (saved.IsSameAs(data))
                return true;
        }
        return false;
    }
}