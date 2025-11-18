using System;
using UnityEngine;


public enum ChipState
{
    None,
    Emitting,   // creating in field over screen - падает «ј баром: отрисовывать под баром, включить физику, отключить кликабельность
    Idle,       // lying in game field container - включить кликабельность
    Moving,     // flying to bar / shifting in bar - отрисовывать поверх бара, отключить физику, отключить кликабельность
    Falling,    // falling from game field container - падает ѕ≈–≈ƒ баром: отрисовывать над баром, включить физику, отключить кликабельность
    Freezed     // forced stop after reshuffle - phys off, velocity off
}

public interface IChipStateMediator
{
    void ApplyState(ChipState state);
    event Action<ChipState> StateProduced;
}

// тонкий "фасад-оrchestrator", склеивающий всЄ:
// Ц если нужно временно отключить физику, трогаем только ChipPhysics;  
// Ц если придЄтс€ перейти с DOTween на корутину, мен€етс€ только ChipView;  
// Ц логика ReshuffleService / ActionBarController общаетс€ с IChipView и IChipPhysics, не зна€ конкретной реализации.
public class Chip : MonoBehaviour
{
    public ChipPassport     Passport { get; private set; }
    public IChipView        View { get; private set; }
    public IChipPhysics     Physics { get; private set; }
    public IChipClickable   Clickable { get; private set; }
    public ChipState        State { get; private set; } = ChipState.None;

    public event Action<Chip> Clicked;
    public event Action<Chip> FallingStopped;

    private IChipStateMediator[] stateMediators;

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    public void Init(GameSettings settings, ChipPassport passport, ChipPartsDatabase db)
    {
        View = GetComponent<IChipView>();
        Physics = GetComponent<IChipPhysics>();
        Clickable = GetComponent<IChipClickable>();
        if (View is null || Physics is null || Clickable is null)
        {
            Debug.LogError("Chip: View, Physics or Clickable not found.");
        }

        stateMediators = GetComponentsInChildren<IChipStateMediator>();
        if (stateMediators.Length == 0)
        {
            Debug.LogError("Chip: StateMediators not found.");
            return;
        }

        Physics.Init(settings);
        View.Init(passport, db);
        Passport = passport;

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        Physics.InteralFallingStopped += OnFallingStopped;
        Clickable.InteralClicked += OnClicked;
        foreach (var sm in stateMediators)
            sm.StateProduced += SetState;
    }

    private void UnsubscribeFromEvents()
    {
        Physics.InteralFallingStopped -= OnFallingStopped;
        Clickable.InteralClicked -= OnClicked;
        foreach (var sm in stateMediators)
            sm.StateProduced -= SetState;
    }

    public void SetState(ChipState newState)
    {
        if (State == newState) return;
        State = newState;

        foreach (var sm in stateMediators)
            sm.ApplyState(newState);
    }

    private void OnClicked() => Clicked?.Invoke(this);
    private void OnFallingStopped() => FallingStopped?.Invoke(this);
}
