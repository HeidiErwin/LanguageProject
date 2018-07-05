using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * This script gets attached to any object that the user 
 * can drag, i.e. ExpressionPieces.
 */
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    /* parentToReturnTo - the section of the screen that the expression should return to if not dropped in 
    * an acceptable area. If the Workspace is still the only area in which words can be combined,
    * this will be more or less obsolete. This was originally implemented when pieces were
    * dragged from the keyboard, rather than 'clicked' from the keyboard.
    */ 
    public Transform parentToReturnTo = null;

    /**
    * placeholder - keeps track of the position that the box should return
    * to when the mouse is released. (expressions in the keyboard don't all shift inwards the
    * second a expression is dragged outside the keyboard)
    * Essentially, placeholder maintains an empty space
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

        //sibling index tracks order of cards in hand
        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;

        DropZone[] zones = GameObject.FindObjectsOfType<DropZone>();

        ExpressionPiece[] expressionsInWorkspace = GameObject.FindObjectsOfType<ExpressionPiece>();
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

    public void OnDestroy() {
        Destroy(placeholder);
    }
}
