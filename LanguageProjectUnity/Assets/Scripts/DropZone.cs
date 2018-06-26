using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/**
 * This script is attached to areas on which expressions can be dropped (hand & tabletop)
 */ 
public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
    
    /**
     * Triggered anytime an object is released on top of this object
     */
    public void OnDrop(PointerEventData eventData) {
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null) {
            d.parentToReturnTo = this.transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null) {
                d.placeholderParent = this.transform;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (eventData.pointerDrag != null) {
            Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
            if (d != null && d.placeholderParent == this.transform) {
                d.placeholderParent = d.parentToReturnTo;
            }
        }
    }
}
