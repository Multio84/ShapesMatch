using System.Collections.Generic;
using UnityEngine;


public class ChipFactory : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private ChipPartsDatabase database;

    private List<ChipPassport> usedPassports = new List<ChipPassport>();
    private int maxUniqueChipsCount;


    public Chip SpawnUniqueChip(Transform parent)
    {
        maxUniqueChipsCount = CountMaxUniqueCombinations(database);
        ChipPassport passport = GetUniqueRandomPassport();

        Chip prefab = database.GetPrefab(passport.prefabIdx);
        Chip chip = Instantiate(prefab, parent);
        chip.Init(passport, database);
        return chip;
    }

    private int CountMaxUniqueCombinations(ChipPartsDatabase db)
    {
        return db.prefabs.Count *
            db.frameColors.Count *
            db.animalSprites.Count;
    }

    private ChipPassport GetUniqueRandomPassport()
    {
        const int ATTEMPTS_LIMIT_COEF = 3;
        int attemptsLimit = maxUniqueChipsCount * ATTEMPTS_LIMIT_COEF;
        int attempts = 0;

        while (attempts < attemptsLimit)
        {
            ChipPassport candidate = MakeRandomPassport();

            if (!ContainsPassport(candidate))
            {
                usedPassports.Add(candidate);
                return candidate;
            }

            attempts++;
        }

        Debug.LogWarning("Couldn't find unique chip: returned duplicate.");
        return MakeRandomPassport();
    }

    private ChipPassport MakeRandomPassport()
    {
        int prefab = Random.Range(0, database.prefabs.Count);
        int color = Random.Range(0, database.frameColors.Count);
        int animal = Random.Range(0, database.animalSprites.Count);

        return new ChipPassport(prefab, color, animal);
    }

    private bool ContainsPassport(ChipPassport passport)
    {
        foreach (ChipPassport saved in usedPassports)
        {
            if (saved.IsSameAs(passport))
                return true;
        }
        return false;
    }
}