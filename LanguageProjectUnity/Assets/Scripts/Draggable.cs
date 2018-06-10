using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This script gets attached to any object that the user 
 * can drag, i.e. boxes/cards.
 */
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Transform parentToReturnTo = null;

    public enum Type { F, X };
    public Type type;

    /**
    * placeholder keeps track of the position that the box should return
    * to when the mouse is released.
    */
    GameObject placeholder = null;

    /**
    * placeholderParent keeps track of the zone that the box should return
    * to when the mouse is released, i.e. the zone of placeholder.
    */
    public Transform placeholderParent = null;

    /**
    * As soon as the user picks up a box, the parent of the box becomes
    * the Canvas, rather than the hand panel (so the rest of the boxes in
    * the hand realign). 
    * 
    * The placeholder keeps track of the position that the box should return
    * to when the mouse is released.
    */
    public void OnBeginDrag(PointerEventData eventData) {
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleHeight = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();
        //TODO: now, loop through zones and highlight the ones that are valid drop zones
    }

    /**
    * The position that the box should return to when dropped is constantly being
    * updated as the box is dragged
    */
    public void OnDrag(PointerEventData eventData) {
        this.transform.position = eventData.position;
        
        if (placeholder.transform.parent != placeholderParent) {
            placeholder.transform.SetParent(placeholderParent);
        }

        int newSiblingIndex = placeholderParent.childCount;

        for (int i = 0; i < placeholderParent.childCount; i++) {
            if (this.transform.position.x < placeholderParent.GetChild(i).position.x) {
                newSiblingIndex = i;

                if(placeholder.transform.GetSiblingIndex() < newSiblingIndex) {
                    newSiblingIndex--;
                }
                break;
            }
        }

        placeholder.transform.SetSiblingIndex(newSiblingIndex);

    }

    public void OnEndDrag(PointerEventData eventData) {
        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        Destroy(placeholder);
    }

    /**
     * This method was just Heidi testing something; will be moved and edited properly later.
     */
    public bool CanBeCombinedWith(Draggable otherObject) {
        return (this.type == otherObject.type);
    }
}
