using UnityEngine;
using UnityEngine.EventSystems;


// deselector for buttons never to be Selected
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
