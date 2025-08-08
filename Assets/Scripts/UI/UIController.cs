using System;
using System.Collections.Generic;
using UnityEngine;


public enum WindowKind
{
    Pause,
    Settings,
    Win,
    Lose
}

public sealed class UIController : MonoBehaviour
{
    [SerializeField] WindowBase[] prefabs;
    [SerializeField] Transform root;
    [SerializeField] RectTransform target;
    [SerializeField] ClickBlocker blocker;

    //public event Action<WindowKind> WindowClosed;

    private GameSettings settings;
    private float animDuration;
    private readonly Dictionary<WindowKind, WindowBase> cache = new();
    private readonly HashSet<WindowKind> visible = new();

    public void Setup(GameSettings gs)
    {
        settings = gs;
        animDuration = settings.windowAnimDuration;
    }

    public void Show(WindowKind kind, bool withBlocker = true)
    {
        var window = GetOrCreate(kind);

        if (withBlocker && blocker)
        {
            blocker.Show(animDuration);
            //blocker.blocksRaycasts = true;
        }

        window.StartedHiding -= OnWindowStartedHiding;
        window.StartedHiding += OnWindowStartedHiding;
        //window.Closed -= OnWindowClosed;
        //window.Closed += OnWindowClosed;

        window.Show(animDuration, DG.Tweening.Ease.InCubic);
        visible.Add(kind);
    }

    private WindowBase GetOrCreate(WindowKind kind)
    {
        if (cache.TryGetValue(kind, out var w))
            return w;

        var prefab = Array.Find(prefabs, p => p.Kind == kind);
        var go = Instantiate(prefab, root);
        cache.Add(kind, go);

        return go;
    }

    private void OnWindowStartedHiding()
    {
        if (blocker && visible.Count == 1)
        {
            blocker.Hide(animDuration);
        }
    }

    // maybe useless method cause noone needs to know that window is closed
    //private void OnWindowClosed(WindowKind kind)
    //{
    //    visible.Remove(kind);
    //    WindowClosed?.Invoke(kind);
    //    //blocker.blocksRaycasts = false;

    //    if (cache.TryGetValue(kind, out var w))
    //        w.Closed -= OnWindowClosed;
    //}
}