using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, ISelectHandler {

    private bool hovering;
    
    public void OnPointerEnter(PointerEventData eventData) {
        Debug.Log("Working");
    }

    public void OnPointerExit(PointerEventData eventData) {
        Debug.Log("Working2");
    }

    public void OnSelect(BaseEventData eventData) {
        //do your stuff when selected
    }
}