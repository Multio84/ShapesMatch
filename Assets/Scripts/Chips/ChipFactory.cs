using System.Collections.Generic;
using UnityEngine;


public class ChipFactory : MonoBehaviour
{
    private ChipPartsDatabase database;
    private IChipCollector collector;
    private GameSettings settings;

    private readonly List<ChipPassport> uniquePassports = new();
    private int maxUniqueChipsCount;

    public void Setup(GameSettings gs, ChipPartsDatabase db, IChipCollector cc)
    {
        settings = gs;
        database = db;
        collector = cc;
    }

    public List<ChipPassport> BuildPassportDeck(int uniqueChipsCount, int chipCopies)
    {
        maxUniqueChipsCount = CountMaxUniquePassports(database);
        if (uniqueChipsCount > maxUniqueChipsCount)
        {
            Debug.LogWarning("Quantity of availableCount unique chips is less than required to generate. It will be clamped.");
            uniqueChipsCount = Mathf.Clamp(uniqueChipsCount, 0, maxUniqueChipsCount);
        }

        // make unique passports
        for (int i = 0; i < uniqueChipsCount; i++)
            uniquePassports.Add(GetUniqueRandomPassport());

        // copy unique passports
        List<ChipPassport> deck = new List<ChipPassport>(uniqueChipsCount * chipCopies);
        foreach (ChipPassport p in uniquePassports)
            for (int i = 0; i < chipCopies; i++)
                deck.Add(p);

        // shuffle all deck of passports
        Shuffle(deck);

        return deck;
    }

    public Chip SpawnChip(ChipPassport passport, Transform parent)
    {
        Chip prefab = database.GetPrefab(passport.prefabIdx);
        Chip chip = Instantiate(prefab, parent);

        chip.Init(settings, passport, database);
        chip.Clicked += collector.TryCollectChip;
        return chip;
    }

    //public ChipPresenter SpawnChip(ChipPassport passport, Transform parent)
    //{
    //    // 1. Спауним визуал (префаб)
    //    ChipView prefabView = database.GetPrefab(passport.prefabIdx);
    //    ChipView view = Object.Instantiate(prefabView, parent);

    //    // 2. Берём компонент ввода
    //    ChipInput input = view.GetComponent<ChipInput>();

    //    // 3. Создаём модель (plain-C#) и инициализируем её тем,
    //    //    что раньше задавалось через Chip.Init(...)
    //    var model = new ChipModel();
    //    model.SetInteractable(true);
    //    model.SetState(ChipState.Idle);
    //    // если нужно, передайте collector / passport в модель
    //    // либо храните эти данные отдельными полями

    //    // 4. Склеиваем всё презентером
    //    var presenter = new ChipPresenter(model, view, input);
    //    presenter.InteralClicked += _collector.TryCollectChip;  // подписка как раньше

    //    return presenter;
    //}

    private ChipPassport GetUniqueRandomPassport()
    {
        const int ATTEMPTS_LIMIT_COEF = 3;
        int attemptsLimit = maxUniqueChipsCount * ATTEMPTS_LIMIT_COEF;
        int attempts = 0;

        while (attempts < attemptsLimit)
        {
            ChipPassport candidate = MakeRandomPassport();

            if (!ContainsPassport(candidate))
                return candidate;

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

    private static void Shuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int j = Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}