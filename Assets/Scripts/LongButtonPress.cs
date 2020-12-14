using UnityEngine;
using UnityEngine.EventSystems;

public class LongButtonPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }
}
