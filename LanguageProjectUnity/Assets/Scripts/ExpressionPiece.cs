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

    private int myWidthInUnits = 1;
    private int myHeightInUnits = 1;
    private const float BUFFER_IN_UNITS = 0.15f; //the slight space between args, etc. for visual appeal
    private const float PIXELS_PER_UNIT = 35.0f;

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

        if(myArguments.Length > 0) {
            myHeightInUnits = 2;
            myWidthInUnits = expression.GetNumArgs() + 1;
        }
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

        exprPieceScript.myWidthInUnits = 1;
        exprPieceScript.myWidthInUnits += droppedexpression.myWidthInUnits;
        if (exprPieceScript.myHeightInUnits < (droppedexpression.myHeightInUnits + 1)) {
            exprPieceScript.myHeightInUnits = droppedexpression.myHeightInUnits + 1;
        }

        int index = 0; //TODO --> actually compute index later, based on where dragging happened
        int counter = -1;
        for (int i = 0; i < myArguments.Length; i++) {
            exprPieceScript.myArguments[i] = this.myArguments[i];
            if (exprPieceScript.myArguments[i] == null) {
                counter++;
                exprPieceScript.myWidthInUnits++;
            } else {
                exprPieceScript.myWidthInUnits += exprPieceScript.myArguments[i].myWidthInUnits;
                if (exprPieceScript.myHeightInUnits < (exprPieceScript.myArguments[i].myHeightInUnits + 1)) {
                    exprPieceScript.myHeightInUnits = exprPieceScript.myArguments[i].myHeightInUnits + 1;
                }
            }
             
            if (counter == index) {
                exprPieceScript.myArguments[i] = droppedexpression;
                Debug.Log("^" + exprPieceScript.myArguments[i].GetExpression() + "^ was properly set.");
                counter++;
                exprPieceScript.myWidthInUnits--;
            }

        }

        exprPieceScript.SetVisual(GenerateVisual(exprPieceScript));
        int indexToOccupy = gameObject.transform.GetSiblingIndex();

        // TODO 7/10 --- THIS IS THE SOURCE OF OUR DISCONTENT!!!!
        // when these are commented out, the argument structure works out.
        // However, the expressions it leaves behind are dangerous:
        // unity will crash if you try to give a built expression to one of its
        // parts (memory-wise).
        // Destroy(this.gameObject, 0.0f);
        // Destroy(droppedexpression.gameObject, 0.0f);

        exprPiece.transform.SetSiblingIndex(indexToOccupy);
    }

    public GameObject GenerateVisual(ExpressionPiece exprPieceScript) {
        return GenerateVisual(exprPieceScript, 0);
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
    public GameObject GenerateVisual(ExpressionPiece exprPieceScript, int layer) {
        Debug.Log("Calling GenerateVisual on ^" + exprPieceScript.GetExpression() + "^");
        GameObject exprPiece = exprPieceScript.gameObject;

        RectTransform pieceRect = exprPiece.GetComponent<RectTransform>();
        float calculatedWidth = (PIXELS_PER_UNIT * (exprPieceScript.myWidthInUnits));
        float calculatedHeight = (PIXELS_PER_UNIT * (exprPieceScript.myHeightInUnits));
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);

        float pieceCenterX = 0;
        float pieceCenterY = 0;
        float pieceTopLeftX = pieceCenterX - calculatedWidth / 2;
        float pieceTopLeftY = pieceCenterY + calculatedHeight / 2;

        GameObject visualContainer = new GameObject();
        visualContainer.name = "VisualContainer";
        visualContainer.transform.SetParent(exprPiece.transform);
        visualContainer.layer = layer;

        RectTransform visContainerRectTransform = visualContainer.AddComponent<RectTransform>();
        Image bgImage = visualContainer.AddComponent<Image>();
        bgImage.color = GetColorOfOutputType(exprPieceScript.myExpression.GetSemanticType());
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth - PIXELS_PER_UNIT* BUFFER_IN_UNITS);
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight - PIXELS_PER_UNIT * BUFFER_IN_UNITS);

        GameObject headObject = new GameObject();
        headObject.name = "Head";
        headObject.transform.SetParent(visualContainer.transform);
        Image headImage = headObject.AddComponent<Image>();
        Expression expr = exprPieceScript.GetExpression();
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead() + "Symbol");
        headImage.sprite = headSprite;
        headImage.transform.localScale *= .25f;
        headImage.transform.position = new Vector3(pieceTopLeftX + (.5f*PIXELS_PER_UNIT), pieceTopLeftY - (.5f*PIXELS_PER_UNIT));

        visualContainer.transform.position = new Vector3(0, 0, 0);


        //BILL: The below commented lines can be ignored; this was an attempt at arguments that
        //      was never completed. I am still not sure if the arguments are better maintained as 
        //      full ExpressionPieces, or if it's better to save their individual data.
        //      Keeping them as full ExpressionPieces seems easier, but if you're trying things out it's your call.
        //      
        //      this was my stab at making the arguments show up. still buggy and not working properly.
        //      still need to account for variable-sized arguments.
        int numArgs = exprPieceScript.myArguments.Length;
        int currentX = 1; //in units
        int currentY = 1;

        for (int i = 0; i < numArgs; i++) {
            Debug.Log("^" + exprPieceScript.GetExpression().GetArg(i) + "^ is an argument...");
            ExpressionPiece arg = exprPieceScript.myArguments[i];
            // Debug.Log(arg.GetExpression()); // if you comment this out, then the second argument is no longer recognized.
            if (arg != null) {
                currentX += arg.myWidthInUnits;
                // Debug.Log(expr.GetHead() + " @ " + i + " is " + arg.GetExpression().GetHead());
                
                GameObject argVisual = GenerateVisual(arg, layer + 1);
                argVisual.transform.SetParent(visualContainer.transform);

                argVisual.transform.position = new Vector3(pieceTopLeftX + PIXELS_PER_UNIT*(currentX - ((.5f * arg.myWidthInUnits) + BUFFER_IN_UNITS)), PIXELS_PER_UNIT * ((-0.5f * currentY) + BUFFER_IN_UNITS));
                //argVisual.transform.position = new Vector3(pieceTopLeftX + (30 * i) + 45, pieceTopLeftY - 45);
                //Debug.Log(arg.GetExpression().GetHead() + " @ " + i);
            } else {
                currentX++;
            }
            
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
            return new Color32(255, 80, 26, 140); //alpha = 140 for semi transparent
        }
        else if (determiningType.GetType() == typeof(T)) {
            return new Color32(86, 178, 255, 140);
        }
        else {
            return new Color32(218, 162, 255, 140);
        }
    }
}
