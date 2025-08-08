using UnityEngine;
using System;





/*НОВОЕ УСЛОВИЕ ВМЕСТО БЛОКИРОВКИ ФИШЕК:
 * 
 * 
фишка добавлена. Если нет летящих, то
    - поискать пустые слоты, и если ничего не было передвинуто, то
    (хз на какой случай)
        - поискать комбинации и уничтожить
 */
// фишки передвинуты - найти комбинации и уничтожить
// Если фишек > 0, попытаться сдвинуть их
/*
Если в баре > 3 фишек, то
    - если найдены комбинации, то
        - уничтожить
    - иначе просто обновить состояние бара
 */




public class ActionBarManager : MonoBehaviour//, IInitializable
{
    /*
    private ActionBar ab;

    public event Action StateChanged;


    public void Setup(GameSettings gs, ActionBar ab)
    {
        this.ab = ab;
    }

    public void Init()
    {
        ab.ChipAdded += OnChipAddedToBar;
        ab.MatchesDestroyed += OnMatchesDestroyed;
        ab.ChipsShiftCompleted += OnChipsShiftCompleted;
    }

    void OnDisable()
    {
        ab.ChipAdded -= OnChipAddedToBar;
        ab.MatchesDestroyed -= OnMatchesDestroyed;
        ab.ChipsShiftCompleted -= OnChipsShiftCompleted;
    }

    public bool IsBarState(BarState barState)
    {
        return ab.UpdateState() == barState;
    }

    
    //фишка добавлена. Если нет летящих, то
    //    - поискать пустые слоты, и если ничего не было передвинуто, то
    //    (хз на какой случай)
    //        - поискать комбинации и уничтожить
    
    public void OnChipAddedToBar(Chip chip, int idx)
    {
        if (ab.HasFlyingChips()) return;

        if (!ab.ShiftChipsToEmptySlots())
            FindAndDestroyMatches();
    }

    // фишки передвинуты - найти комбинации и уничтожить
    public void OnChipsShiftCompleted()
    {
        FindAndDestroyMatches();
    }

    // Если фишек > 0, попытаться сдвинуть их
    public void OnMatchesDestroyed()
    {
        StateChanged?.Invoke();

        if (IsBarState(BarState.PotentialShift))
            ab.ShiftChipsToEmptySlots();
    }

    
    //Если в баре > 3 фишек, то
    //    - если найдены комбинации, то
    //        - уничтожить
    //    - иначе просто обновить состояние бара
    
    private void FindAndDestroyMatches()
    {
        if (IsBarState(BarState.PotentialMatch))
        {
            if (ab.FindMatches())
                ab.DestroyMatches();
            else
                StateChanged?.Invoke();
        }
    }
    */
}