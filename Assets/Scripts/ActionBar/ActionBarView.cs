using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionBarView : MonoBehaviour
{
    [Header("Links")]
    [SerializeField] private Transform chipsRoot;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public Transform[] slotPoints = new Transform[ActionBarModel.SlostCount];

    private GameSettings settings;

    private float chipFlyDuration;
    private float chipShiftDuration;
    private float chipDeathDuration;
    private int layerOrder;

    public int LayerOrder => layerOrder;


    /* --------------------- PUBLIC API --------------------- */

    public void Setup(GameSettings gs)
    {
        settings = gs;

        chipFlyDuration = settings.chipFlyDuration;
        chipShiftDuration = settings.chipShiftDuration;
        chipDeathDuration = settings.chipDeathDuration;

        layerOrder = spriteRenderer.sortingOrder;
    }

    public Transform GetSlotPoint(int idx) => slotPoints[idx];

    public IEnumerator PlayRemoval(List<Chip> chips)
    {
        if (chips is null || chips.Count == 0)
            yield break;

        int waiting = chips.Count;

        foreach (var chip in chips)
            DestroyChip(chip, () => waiting--);

        yield return new WaitUntil(() => waiting == 0);
    }

    public IEnumerator PlayMoves(List<MoveInfo> moves)
    {
        if (moves is null || moves.Count == 0)
            yield break;

        int waiting = moves.Count;

        foreach (var m in moves)
            ShiftChipToSlot(m.Chip, m.To, () => waiting--);

        yield return new WaitUntil(() => waiting == 0);
    }

    public void FlyChip(Chip chip, Transform target, Action onComplete)
    {
        //chip.SetState(ChipState.Flying);
        chip.SetPhysEnabled(false);
        chip.Rotate(target, chipFlyDuration);
        chip.Move(target, chipFlyDuration).
            OnComplete(() => onComplete?.Invoke());
    }

    public List<Chip> DropChips(List<Chip> chips)
    {
        foreach (var chip in chips)
            chip.DropFromActionBar();

        return chips;
    }

    public void AttachChipToSlot(Chip chip, int slotIdx)
    {
        chip.transform.SetParent(chipsRoot);
        chip.transform.position = slotPoints[slotIdx].position;
    }

    private void ShiftChipToSlot(Chip chip, int targetIdx, Action OnDone)
    {
        chip.Move(slotPoints[targetIdx], chipShiftDuration)
            .OnComplete(() => OnDone?.Invoke());
    }

    private void DestroyChip(Chip chip, Action OnDone)
    {
        chip.Die(chipDeathDuration)
            .OnComplete(() => OnDone?.Invoke());
    }
}