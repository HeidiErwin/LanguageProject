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

    private static float MY_WIDTH = 70.0f;
    private static float MY_HEIGHT = 70.0f;

    private Expression myExpression;
    private ExpressionPiece[] myArguments;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    /**
     * Called when an ExpressionPiece is created by a Controller or something that isn't OnDrop()
     */
    public void SetExpression(Expression expression) { 
        myExpression = expression;
        myArguments = new ExpressionPiece[expression.GetNumArgs()];
    }

    /**
    * Returns true if this Expression can accept another Expression as input, false otherwise
    */
    public bool CanAccept (ExpressionPiece otherExpression) {
        List<SemanticType> myInputTypes = myExpression.GetInputType();
        return myInputTypes != null && myInputTypes.Exists(semtype => semtype.Equals(otherExpression.GetExpression().GetSemanticType()));
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

        Expression expr = null;
        //try to create new Expression
        try {
            expr = new Phrase(this.myExpression, droppedexpression.GetExpression());
        } catch (Exception e) {
            Debug.LogException(e);
            return;
        }

        //generate new Piece
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.SetExpression(expr);
        exprPieceInstance.transform.SetParent(this.transform.parent.transform);
        exprPieceScript.expressionName = expr.GetHead();
        exprPieceScript.myArguments = this.myArguments;

        int index = 0; //TODO --> actually compute index later, based on where dragging happened
        int counter = -1;
        for (int i = 0; i < myArguments.Length; i++) {
            exprPieceScript.myArguments[i] = this.myArguments[i];
            if (exprPieceScript.myArguments[i] == null) {
                counter++;
            }
            if (counter == index) {
                exprPieceScript.myArguments[i] = droppedexpression;
               // exprPieceScript.myArguments[i].SetExpression(droppedexpression.myExpression);
                counter++;
            }
        }

        exprPieceScript.SetVisual(GenerateVisual(exprPieceScript));
        int indexToOccupy = gameObject.transform.GetSiblingIndex();

        Destroy(this.gameObject, 0.0f);
        Destroy(droppedexpression.gameObject, 0.0f);

        exprPiece.transform.SetSiblingIndex(indexToOccupy);
    }

    /**
     * Creates a GameObject that will be a child of this ExpressionPiece, and will
     * have children GameObjects which hold Images (the different visuals of this 
     * ExpressionPiece).
     * This ExpressionPiece will later set the returned GameObject as a child of itself.
     * 
     * STEPS TO GENERATE NEW VISUAL:
     * 1. head in top left
     * 2. generate argument sprites
     * 3. set piece sprite width to sum of widths of args (+1 for head)
     * 4. set piece sprite height to max height of args + 1
     * 5. place head
     * 6. place args: if arg has width > 1, we place next arg however many after it
     */
    public GameObject GenerateVisual(ExpressionPiece exprPieceScript) {
        GameObject exprPiece = exprPieceScript.gameObject;
        RectTransform pieceRect = exprPiece.GetComponent<RectTransform>();
        float calculatedWidth = MY_WIDTH + (35.0f * (exprPieceScript.myExpression.GetNumArgs() - 1));
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
        
        float pieceCenterX = exprPiece.transform.position.x;
        float pieceCenterY = exprPiece.transform.position.y;
        float pieceTopLeftY = exprPiece.transform.position.y + pieceRect.rect.height/2;
        float pieceTopLeftX = exprPiece.transform.position.x - pieceRect.rect.width/2;

        //set color
        Image[] bgImage = exprPiece.GetComponents<Image>();
        bgImage[0].color = GetColorOfOutputType(exprPieceScript.myExpression.GetSemanticType());

        GameObject visualContainer = new GameObject();
        visualContainer.name = "VisualContainer";
        visualContainer.transform.SetParent(exprPiece.transform);

        GameObject headObject = new GameObject();
        headObject.name = "Head";
        headObject.transform.SetParent(visualContainer.transform);
        Image headImage = headObject.AddComponent<Image>();
        Expression expr = exprPieceScript.GetExpression();
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead() + "Symbol");
        headImage.sprite = headSprite;
        headImage.transform.localScale = headImage.transform.localScale*.25f;
        headImage.transform.position = new Vector3(pieceTopLeftX + 15, pieceTopLeftY - 15);

        //BILL: The below commented lines can be ignored; this was an attempt at arguments that
        //      was never completed. I am still not sure if the arguments are better maintained as 
        //      full ExpressionPieces, or if it's better to save their individual data.
        //      Keeping them as full ExpressionPieces seems easier, but if you're trying things out it's your call.
        //      
        //      this was my stab at making the arguments show up. still buggy and not working properly.
        //      still need to account for variable-sized arguments.
        int numArgs = exprPieceScript.myArguments.Length;
        for (int i = 0; i < numArgs; i++) {
            ExpressionPiece arg = exprPieceScript.myArguments[i];
            Debug.Log(arg.GetExpression()); // if you comment this out, then the second argument is no longer recognized.
            if (arg != null) {
                Debug.Log(expr.GetHead() + " @ " + i + " is " + arg.GetExpression().GetHead());
                GameObject argVisual = GenerateVisual(arg);
                argVisual.transform.SetParent(visualContainer.transform);
                argVisual.transform.position = new Vector3(pieceTopLeftX + (30 * i) + 45, pieceTopLeftY - 45);
                Debug.Log(arg.GetExpression().GetHead() + " @ " + i);
            }
            
            //GameObject argObject = new GameObject();
            //argObject.name = "Argument";
            //argObject.transform.SetParent(visualContainer.transform);
            //Image argImage = argObject.AddComponent<Image>();
            //Expression argExpr = arg.myExpression;
            //Sprite argSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead() + "Symbol");
            //headImage.sprite = headSprite;
            //headImage.transform.localScale = headImage.transform.localScale * .25f;
            //headImage.transform.position = new Vector3(centerPieceX - 20, centerPieceY + 20);
       }

        return visualContainer;
    }

    public void SetVisual(GameObject generatedVisual) {
        generatedVisual.transform.SetParent(gameObject.transform);
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

    /**
     * Given a semantic type, returns a color based on the output type of that semantic type.
     * If the type is atomic (the output type would be null), returns a color based on simply 
     * the semantic type.
     */
    public static Color GetColorOfOutputType(SemanticType semType) {
        SemanticType determiningType;

        if (semType.IsAtomic()) {
            determiningType = semType;
        } else {
            determiningType = semType.GetOutputType();
        }

        if (determiningType.GetType() == typeof(E)) {
            return new Color32(255, 80, 26, 255); //alpha = 140 for semi transparent
        }
        else if (determiningType.GetType() == typeof(T)) {
            return new Color32(86, 178, 255, 255);
        }
        else {
            return new Color32(218, 162, 255, 255);
        }
    }
}
