using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

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

    private ExpressionPiece[] myArguments;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    public ExpressionPiece(string expressionName, Sprite defaultSprite, Sprite currentSprite, Expression expr) {
        this.expressionName = expressionName;
        this.defaultSprite = defaultSprite;
        this.currentSprite = currentSprite;
        myExpression = expr;
    }

    /**
     * Called when an ExpressionPiece is created by a Controller or something that isn't OnDrop()
     */
    public void SetExpression(Expression expression) { 
        myExpression = expression;
        myArguments = new ExpressionPiece[expression.GetNumArgs()];
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

        Expression expr = null;
        //try to create new Expression
        try {
            expr = new Phrase(this.myExpression, droppedexpression.GetExpression());
        } catch (ArgumentException e) {
            Debug.Log("arg exception");
            return;
        }

        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(100, 100), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.SetExpression(expr);
        exprPieceInstance.transform.SetParent(this.transform.parent.transform);
        exprPieceScript.expressionName = expr.GetHead();
        exprPieceScript.myArguments = this.myArguments;

        int index = 0; //TODO --> actually compute index later, based on where dragging happened
        int counter = -1;
        for (int i = 0; i < myArguments.Length; i++) {
            exprPieceScript.myArguments[i] = this.myArguments[i];
            if (exprPieceScript.myArguments != null) {
                counter++;
            }
            if (counter == index) {
                exprPieceScript.myArguments[i] = droppedexpression;
                counter++;
            }
        }

        exprPieceScript.currentSprite = GenerateNewImage(exprPieceScript); 

        //exprPiece.transform.localScale = new Vector2(3f, 2f);
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/running");

        Destroy(this.gameObject, 0.0f);
        Destroy(droppedexpression.gameObject, 0.0f);
    }

    /**
     * Creates an Image, which is a component that contains other Images, stored inside
     * GameObjects, as children
     * 
     * STEPS TO GENERATE NEW SPRITE
     * 1. head in top left
     * 2. generate argument sprites
     * 3. set piece sprite width to sum of widths of args (+1 for head)
     * 4. set piece sprite height to max height of args + 1
     * 5. place head
     * 6. place args: if arg has width > 1, we place next arg however many after it
     */
    public Sprite GenerateNewImage(ExpressionPiece exprPieceScript) {
        Expression expr = exprPieceScript.myExpression;
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead());
        GameObject headObject = new GameObject();
        headObject.transform.SetParent(exprPieceScript.gameObject.transform);
        Image headImage = headObject.AddComponent<Image>();
        headImage.sprite = headSprite;

        return headSprite;
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
