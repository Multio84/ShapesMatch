using System.Collections.Generic;
using UnityEngine;


public class ChipFactory : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private ChipPartsDatabase database;

    private readonly List<ChipPassport> uniquePassports = new List<ChipPassport>();
    private int maxUniqueChipsCount;


    public List<ChipPassport> BuildPassportDeck(int uniqueCount, int chipCopies = 3)
    {
        int max = CountMaxUniquePassports(database);
        if (uniqueCount > maxUniqueChipsCount)
        {
            Debug.LogWarning("Quantity of available unique chips is less than required to generate. It will be clamped.");
            uniqueCount = Mathf.Clamp(uniqueCount, 0, max);
        }

        // make unique passports
        for (int i = 0; i < uniqueCount; i++)
            uniquePassports.Add(GetUniqueRandomPassport());

        // triple unique passports
        List<ChipPassport> deck = new List<ChipPassport>(uniqueCount * chipCopies);
        foreach (ChipPassport p in uniquePassports)
            for (int i = 0; i < chipCopies; i++)
                deck.Add(p);

        // shuffle all passportsDeck of passports
        Shuffle(deck);

        return deck;
    }

    public Chip SpawnChip(ChipPassport passport, Transform parent)
    {
        Chip prefab = database.GetPrefab(passport.prefabIdx);
        Chip chip = Instantiate(prefab, parent);
        chip.Init(passport, database);
        return chip;
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
                uniquePassports.Add(candidate);
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
        foreach (ChipPassport saved in uniquePassports)
        {
            if (saved.IsSameAs(passport))
                return true;
        }
        return false;
    }

    private int CountMaxUniquePassports(ChipPartsDatabase db) =>
       db.prefabs.Count * db.frameColors.Count * db.animalSprites.Count;

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}