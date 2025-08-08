using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;


// shows and hides tutorial UI elements
public sealed class TutorialUIController : MonoBehaviour, IInitializable
{
    [Serializable]
    public class TutorialElementEntry
    {
        public TutorialElementBase prefab;
        public RectTransform anchor;
    }

    [SerializeField] private List<TutorialElementEntry> entries = new();

    public event Action<TutorialStep> TutorialElementClosed;

    private GameSettings settings;
    private float animDuration;
    private Dictionary<TutorialStep, TutorialElementEntry> entriesByStep;
    private TutorialElementBase currentElement;

    public void Setup(GameSettings gs)
    {
        settings = gs;

        animDuration = settings.tutorialAnimDuration;
    }

    public void Init()
    {
        CollectElements();
    }

    public void Show(TutorialStep step)
    {
        currentElement = SpawnElement(step);
        if (currentElement is null) return;

        currentElement.Closed += OnElementClosed;
        currentElement.Show(animDuration, Ease.InCubic);
    }

    // forms TutorialElementEntry collection into dictionary of elements with steps as keys
    private void CollectElements()
    {
        entriesByStep = new Dictionary<TutorialStep, TutorialElementEntry>();

        foreach (var entry in entries)
        {
            if (!entry?.prefab)
            {
                Debug.LogWarning("TutorialCollection: element prefab is null.");
                continue;
            }

            var step = entry.prefab.Step;
            if (entriesByStep.ContainsKey(step))
            {
                Debug.LogWarning($"TutorialCollection: duplicate entry for step [{step}].");
                continue;
            }

            entriesByStep.Add(step, entry);
        }
    }

    private TutorialElementBase SpawnElement(TutorialStep step)
    {
        var entry = GetEntry(step);
        if (entry is null) return null;

        return Instantiate(entry.prefab, entry.anchor, worldPositionStays: false);
    }

    private TutorialElementEntry GetEntry(TutorialStep step)
    {
        entriesByStep.TryGetValue(step, out var entry);
        return entry;
    }

    private void OnElementClosed(TutorialStep step)
    {
        TutorialElementClosed?.Invoke(step);
        currentElement.Closed -= OnElementClosed;
    }
}
