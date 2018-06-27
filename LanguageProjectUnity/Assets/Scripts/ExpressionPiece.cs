using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A script to be attached to any Expression objects.
 */
public class ExpressionPiece : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    //the expression in English (e.g. Key, Door, etc.)
    public string expressionName;

    public Sprite currentSprite;
    public Sprite defaultSprite;
    public Sprite previousSprite;
    public bool isShowingPreview;

    private Expression myExpression;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    public ExpressionPiece(string expressionName, Sprite defaultSprite, Sprite currentSprite, Expression expr) {
        this.expressionName = expressionName;
        this.defaultSprite = defaultSprite;
        this.currentSprite = currentSprite;
        myExpression = expr;
    }

    public void SetExpression(Expression expression) { 
        myExpression = expression;
    }

    public void Update() {
        //there should only be one image in the images array; the loop below is just for safe measure
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images) {
            image.sprite = currentSprite;
        }
    }

    /**
    * Returns true if this Expression can accept another Expression as input, false otherwise
    */
    public bool CanAccept (ExpressionPiece otherExpression) {
        List<SemanticType> myInputTypes = myExpression.GetInputType();
        if(myInputTypes == null) {
            return false;
        } else {
            return myInputTypes.Exists(semtype => semtype.Equals(otherExpression.GetExpression().GetSemanticType()));
        }
    }

    /**
     * Returns the new sprite that this expression should display when COMBINED WITH
     * the ExpressionPiece expressionToCombine
     */
    public Sprite DetermineUpdatedSprite (ExpressionPiece expressionToCombine) {
        Sprite updatedSprite = Resources.Load<Sprite>("PlaceholderSprites/" + this.expressionName + expressionToCombine.expressionName);

        if (updatedSprite != null) {
            return updatedSprite;
        }
        else {
            return Resources.Load<Sprite>("PlaceholderSprites/smile"); //smile as placeholder if no appropriate sprite exists
        }
    }

    /**
    * Returns the sprite that this expression should display when the user is HOLDING
    * the ExpressionPiece expressionToCombine
    */
    public Sprite DeterminePreviewSprite(ExpressionPiece expressionToCombine) {
        Sprite previewSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expressionToCombine.expressionName + this.expressionName + "open");

        if (previewSprite != null) {
            return previewSprite;
        }
        else {
            return Resources.Load<Sprite>("PlaceholderSprites/smile"); //smile as placeholder if no appropriate sprite exists
        }
    }

    /**
    * Triggered anytime an object is released on top of this expression. 
    * The image of this expression is updated appropriately.
    */
    public void OnDrop(PointerEventData eventData) {
        ExpressionPiece droppedexpression = eventData.pointerDrag.GetComponent<ExpressionPiece>();
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name + " :)");

        if (CanAccept(droppedexpression)) {
            this.currentSprite = DetermineUpdatedSprite(droppedexpression);
            this.isShowingPreview = false;
        }
    }

    /**
     * When this expression is picked up, anything that it can be combined with will adopt
     * the appropriate preview sprite.
     * e.g. if this expression is an E, a expression of type E->T will turn from a solid shape into a 
     * shape that shows that this E can be inserted into the shape.
     */
    public void OnBeginDrag(PointerEventData eventData) {
        ExpressionPiece[] expressionsOnScreen = FindObjectsOfType<ExpressionPiece>();
        compatibleAcceptingExpressions = new List<ExpressionPiece>();
        foreach (ExpressionPiece ep in expressionsOnScreen) {
            if (ep.CanAccept(gameObject.GetComponent<ExpressionPiece>()) && !this.Equals(ep)) { 
                compatibleAcceptingExpressions.Add(ep);
            }
        }
        foreach (ExpressionPiece ep in compatibleAcceptingExpressions) {
            Sprite previewSprite = DeterminePreviewSprite(ep);
            ep.previousSprite = ep.currentSprite;
            ep.currentSprite = previewSprite;
            ep.isShowingPreview = true;
        }
    }

    public void OnDrag(PointerEventData eventData) {
    }

    /**
    * When dragging this ExpressionPiece ends, change the preview sprites on screen back to 
    * their original sprites.
    */
    public void OnEndDrag(PointerEventData eventData) {
        foreach (ExpressionPiece ep in compatibleAcceptingExpressions) {
            if (ep.isShowingPreview) {
                ep.currentSprite = ep.previousSprite;
            }
        }
        compatibleAcceptingExpressions.Clear();
    }

    public Expression GetExpression() {
        return myExpression;
    }
}
