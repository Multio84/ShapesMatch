using UnityEngine;
using UnityEngine.EventSystems;


public struct ChipPassport
{
    public int prefabIdx;
    public int colorIdx;
    public int animalIdx;

    public ChipPassport(int prefab, int color, int animal)
    {
        prefabIdx = prefab;
        colorIdx = color;
        animalIdx = animal;
    }

    public bool IsSameAs(ChipPassport other)
    {
        return prefabIdx == other.prefabIdx &&
               colorIdx == other.colorIdx &&
               animalIdx == other.animalIdx;
    }
}

public class Chip : MonoBehaviour, IPointerDownHandler
{
    [Header("Renderers")]
    [SerializeField] private SpriteRenderer frameRenderer;  // to set color
    [SerializeField] private SpriteRenderer animalRenderer; // to set animal

    private ChipPassport passport;
    public ChipPassport Passport => passport;

    public void Init(ChipPassport passport, ChipPartsDatabase db)
    {
        this.passport = passport;

        frameRenderer.color = db.GetColor(passport.colorIdx);
        animalRenderer.sprite = db.GetAnimal(passport.animalIdx);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Destroy(gameObject);
    }
}