
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