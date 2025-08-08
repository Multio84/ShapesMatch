using UnityEngine;
using UnityEngine.UI;


//Передаём размер UI элемента в материал:
[RequireComponent(typeof(RectTransform), typeof(Graphic))]
public class UICutoutMaterialSetter : MonoBehaviour
{
    void Update()
    {
        var rectTransform = GetComponent<RectTransform>();
        var graphic = GetComponent<Graphic>();
        var mat = graphic.material;

        if (mat)
        {
            Vector2 size = rectTransform.rect.size;
            mat.SetVector("_ElementSize", new Vector4(size.x, size.y, 0, 0));
        }
    }
}
