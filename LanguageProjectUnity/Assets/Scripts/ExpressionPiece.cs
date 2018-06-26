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

    public SemanticType semanticType;

    private Expression myExpression;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    public ExpressionPiece(string expressionName, Sprite defaultSprite, Sprite currentSprite) {
        this.expressionName = expressionName;
        this.defaultSprite = defaultSprite;
        this.currentSprite = currentSprite;
    }

    /**
     * This method should be called after the ExpressionPiece is made, so that an 
     * Expression script can be built and attached to this ExpressionPiece
     * 
     * @param name - the name of this Expression
     * @param isWord - false if the Expression is a Phrase, false if the Expression is a Word
     * @param semType - the SemanticType of this Expression
     * @param phraseParts - if the Expression is a Word, null 
     *                      if the Expression is a Phrase, a 2-element List with the input and 
     *                      output Expressions for the phrase
     * 
     * NOTE: potentially merge this with the ExpressionPiece constructor later
     */
    public void SetExpression(string name, bool isWord, SemanticType semType, List<Expression> phraseParts) {
        if (isWord) {
            myExpression = new Word(semType, name);
        }
        else {
            myExpression = new Phrase(phraseParts[0], phraseParts[1]);
        }
    }

    public void Update() {
        //TODO: efficiency, don't update every tick if unnecessary
        //there should only be one image in the images array; the loop below is just for safe measure
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images) {
            image.sprite = currentSprite;
        }
    }

    /**
    * Returns true if this Expression can accept another Expression as input, false otherwise
    */
    public bool CanAccept (ExpressionPiece otherexpression) {
        //TODO: test below
        List<SemanticType> myInputTypes = myExpression.GetInputType();
        // return (myInputTypes.Exists(semtype => semtype.Equals(otherexpression.semanticType)));
        return true;
    }

    /**
     * Returns the new sprite that this expression should display when COMBINED WITH
     * the ExpressionPiece expressionToCombine
     */
    public Sprite DetermineUpdatedSprite (ExpressionPiece expressionToCombine) {
        Sprite fXSprite = Resources.Load<Sprite>("PlaceholderSprites/fXImage");

        //TODO: given expressionToCombine, return appropriate Sprite
        return fXSprite;
    }

    /**
    * Returns the sprite that this expression should display when the user is HOLDING
    * the ExpressionPiece expressionToCombine
    */
    public Sprite DeterminePreviewSprite(ExpressionPiece expressionToCombine) {
        Sprite fOpenSprite = Resources.Load<Sprite>("PlaceholderSprites/fOpenSlot");

        //TODO: given expressionToCombine, return appropriate Sprite
        return fOpenSprite;
    }

    /**
    * Triggered anytime an object is released on top of this expression. 
    * The image of this expression is updated appropriately.
    */
    public void OnDrop(PointerEventData eventData) {
        ExpressionPiece droppedexpression = eventData.pointerDrag.GetComponent<ExpressionPiece>();
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name + " :)");

        if (CanAccept(droppedexpression)) {
            // TODO: the dropped expression "returns" to the keyboard (actually, 
            //       the expression never vanishes to begin with, just a copy is made) and the current expression changes to the new sprite
            // TODO: make sure you can only combine expressions in the workspace---keyboard should stay constant

            this.currentSprite = DetermineUpdatedSprite(droppedexpression);
            this.isShowingPreview = false;
        }
    }

    /**
     * When this expression is picked up, anything that it can be combined with will adopt
     * the appropriate preview sprite.
     * e.g. if this expression is an E, a expression of type E->T will turn from a solid shape into a 
     * shape that clearly shows that this E can be inserted into the shape.
     */
    public void OnBeginDrag(PointerEventData eventData) {
        ExpressionPiece[] expressionsOnScreen = FindObjectsOfType<ExpressionPiece>();
        compatibleAcceptingExpressions = new List<ExpressionPiece>();
        foreach (ExpressionPiece wp in expressionsOnScreen) {
            if (this.CanAccept(wp) && !this.Equals(wp)) {
                compatibleAcceptingExpressions.Add(wp);
            }
        }
        foreach (ExpressionPiece wp in compatibleAcceptingExpressions) {
            Sprite previewSprite = DeterminePreviewSprite(wp);
            wp.previousSprite = wp.currentSprite;
            wp.currentSprite = previewSprite;
            wp.isShowingPreview = true;
        }
    }

    public void OnDrag(PointerEventData eventData) {
    }

    /**
    * When dragging this ExpressionPiece ends, change the preview sprites on screen back to 
    * their original sprites.
    */
    public void OnEndDrag(PointerEventData eventData) {
        foreach (ExpressionPiece wp in compatibleAcceptingExpressions) {
            if (wp.isShowingPreview) {
                wp.currentSprite = wp.previousSprite;
            }
        }
        compatibleAcceptingExpressions.Clear();
    }

    public Expression GetExpression() {
        return myExpression;
    }
}
