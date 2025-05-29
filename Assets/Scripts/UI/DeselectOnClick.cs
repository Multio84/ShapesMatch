using UnityEngine;
using UnityEngine.EventSystems;


public class DeselectOnClick : MonoBehaviour,
                               IPointerUpHandler,
                               IPointerExitHandler
{

    public void OnPointerUp(PointerEventData eventData)
    {
        Deselect();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deselect();
    }


    public void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
