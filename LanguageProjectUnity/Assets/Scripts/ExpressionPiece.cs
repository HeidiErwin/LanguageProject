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

    public static bool DRAW_SUBEXPRESSION_TYPE = true;
    public static float EXPRESSION_OPACITY = 0.4f;
    public static bool DRAW_ARGUMENT_TYPE = true;
   
    public string expressionName; //the expression in English (e.g. Key, Door, etc.)

    public Sprite currentSprite;
    public Sprite defaultSprite;
    public Sprite previousSprite;
    public bool isShowingPreview;

    private int myWidthInUnits = 1;
    private int myHeightInUnits = 1;
    private const float BUFFER_IN_UNITS = 0.1f; //the slight space between args, etc. for visual appeal
    private const float PIXELS_PER_UNIT = 40.0f;

    private Expression myExpression;
    private ExpressionPiece[] myArguments;
    // private GameObject[] toDestroy;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    /**
     * Called when an ExpressionPiece is created by a Controller or something that isn't OnDrop()
     */
    public void SetExpression(Expression expression) {
        myExpression = expression;
        myArguments = new ExpressionPiece[expression.GetNumArgs()];

        int counter = 0;
        for (int i = 0; i < myArguments.Length; i++) {
            // the BUG is either in this block...
            if (expression.GetArg(i) == null && DRAW_ARGUMENT_TYPE) {
                GameObject exprPiece = Resources.Load("Piece") as GameObject;
                GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, -100), Quaternion.identity) as GameObject;
                ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
                exprPieceScript.myExpression = new Word(expression.GetInputType(counter), "_");
                exprPieceScript.myArguments = new ExpressionPiece[0];
                exprPieceInstance.transform.SetParent(this.transform);
                exprPieceScript.transform.SetParent(this.transform);
                exprPieceScript.expressionName = "_";
                myArguments[i] = exprPieceScript;
                counter++;
            }
        }

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

        // generate new Piece
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.myExpression = expr;
        exprPieceScript.myArguments = new ExpressionPiece[expr.GetNumArgs()];

        Debug.Log(expr);
        if (exprPieceScript.myArguments.Length == 3) {
            Debug.Log(" ...has 3 arguments as expected.");
        }
        
        
        if (exprPieceScript.myArguments.Length > 0) {
            exprPieceScript.myHeightInUnits = 2;    
        }
        
        exprPieceInstance.transform.SetParent(this.transform.parent.transform);
        exprPieceScript.expressionName = expr.ToString();
        exprPieceScript.myArguments = this.myArguments;

        exprPieceScript.myWidthInUnits = 1;
        exprPieceScript.myWidthInUnits += droppedexpression.myWidthInUnits;
        if (exprPieceScript.myHeightInUnits < (droppedexpression.myHeightInUnits + 1)) {
            exprPieceScript.myHeightInUnits = droppedexpression.myHeightInUnits + 1;
        }

        int index = 0; //TODO: actually compute index, based on where drop happened
        int counter = -1;
        
        for (int i = 0; i < myArguments.Length; i++) {
            if (this.myArguments[i] == null) {
                if (i == 2) {
                    Debug.Log("Third argument: 'twas NULL.");
                }
                counter++;
                exprPieceScript.myWidthInUnits++;
            } else {
                // or the BUG may be here...
                exprPieceScript.myArguments[i] = myArguments[i].DeepCopy();
                exprPieceScript.myWidthInUnits += exprPieceScript.myArguments[i].myWidthInUnits;

                if (exprPieceScript.myHeightInUnits < (exprPieceScript.myArguments[i].myHeightInUnits + 1)) {
                    exprPieceScript.myHeightInUnits = exprPieceScript.myArguments[i].myHeightInUnits + 1;
                }

                if (this.myArguments[i].expressionName.Equals("_")) {
                    counter++;
                }
            }
             
            if (counter == index) {
                // ... or here.
                // Destroy(this.myArguments[i].gameObject, 0.0f); no longer necessary!
                // Destroy(exprPieceScript.myArguments[i].gameObject, 0.0f); also no longer necessary!
                // Destroy(exprPieceScript.myArguments[i], 0.0f);
                exprPieceScript.myArguments[i] = droppedexpression.DeepCopy();
                counter++;
                exprPieceScript.myWidthInUnits--;
            }
        }

        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());

        int indexToOccupy = this.gameObject.transform.GetSiblingIndex();

        Destroy(this.gameObject, 0.0f);
        Destroy(droppedexpression.gameObject, 0.0f);

        exprPiece.transform.SetSiblingIndex(indexToOccupy);
    }

    public ExpressionPiece DeepCopy() {
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        
        exprPieceScript.expressionName = this.GetExpression().ToString();
        exprPieceScript.myExpression = this.GetExpression();
        exprPieceScript.myArguments = new ExpressionPiece[this.myArguments.Length];
        exprPieceScript.myHeightInUnits = this.myHeightInUnits;
        exprPieceScript.myWidthInUnits = this.myWidthInUnits;

        // exprPieceInstance.transform.SetParent(this.transform);

        for (int i = 0; i < this.myArguments.Length; i++) {
            if (this.myArguments[i] != null) {
                exprPieceScript.myArguments[i] = this.myArguments[i].DeepCopy();
            }
        }

        return exprPieceScript;
    }

    public GameObject GenerateVisual() {
        return GenerateVisual(0);
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
    public GameObject GenerateVisual(int layer) {
        // Debug.Log("Calling GenerateVisual on ^" + this.GetExpression() + "^");

        GameObject exprPiece = this.gameObject;

        RectTransform pieceRect = exprPiece.GetComponent<RectTransform>();
        float calculatedWidth  = PIXELS_PER_UNIT * this.myWidthInUnits;
        float calculatedHeight = PIXELS_PER_UNIT * this.myHeightInUnits;
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);

        float pieceCenterX = 0;
        float pieceCenterY = 0;
        float pieceTopLeftX = pieceCenterX - calculatedWidth / 2;
        float pieceTopLeftY = pieceCenterY + calculatedHeight / 2;

        SemanticType semType = this.myExpression.GetSemanticType();

        GameObject visualContainer = new GameObject();
        visualContainer.name = "VisualContainer";
        visualContainer.transform.SetParent(exprPiece.transform);
        visualContainer.layer = layer;

        float BUFFER_IN_PIXELS = PIXELS_PER_UNIT * BUFFER_IN_UNITS;

        RectTransform visContainerRectTransform = visualContainer.AddComponent<RectTransform>();
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth - PIXELS_PER_UNIT * BUFFER_IN_UNITS);
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight  - PIXELS_PER_UNIT * BUFFER_IN_UNITS);

        if (DRAW_SUBEXPRESSION_TYPE || layer == 0) {

            Image bgImage = visualContainer.AddComponent<Image>();
            bgImage.color = (layer == 0) ? GetColorOfOutputType(semType) : GetColorOfSemanticType(semType) + new Color(0.25f, 0.25f, 0.25f, 0f) - new Color(0, 0, 0, (1 - EXPRESSION_OPACITY));
            
            Color borderColor = GetColorOfSemanticType(semType);
            
            GameObject northBorder = new GameObject();
            northBorder.name = "NorthBorder";
            northBorder.transform.SetParent(visualContainer.transform);
            northBorder.layer = layer;
            RectTransform northBorderRectTransform = northBorder.AddComponent<RectTransform>();
            Image northBorderImage = northBorder.AddComponent<Image>();
            northBorderImage.color = borderColor;
            northBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
            northBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BUFFER_IN_PIXELS);
            northBorder.transform.Translate(new Vector3(0, calculatedHeight / 2 - BUFFER_IN_PIXELS / 2, 0));

            GameObject southBorder = new GameObject();
            southBorder.name = "SouthBorder";
            southBorder.transform.SetParent(visualContainer.transform);
            southBorder.layer = layer;
            RectTransform southBorderRectTransform = southBorder.AddComponent<RectTransform>();
            Image southBorderImage = southBorder.AddComponent<Image>();
            southBorderImage.color = borderColor;
            southBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
            southBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BUFFER_IN_PIXELS);
            southBorder.transform.Translate(new Vector3(0, -calculatedHeight / 2 + BUFFER_IN_PIXELS / 2, 0));

            GameObject westBorder = new GameObject();
            westBorder.name = "WestBorder";
            westBorder.transform.SetParent(visualContainer.transform);
            westBorder.layer = layer;
            RectTransform westBorderRectTransform = westBorder.AddComponent<RectTransform>();
            Image westBorderImage = westBorder.AddComponent<Image>();
            westBorderImage.color = borderColor;
            westBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BUFFER_IN_PIXELS);
            westBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);
            westBorder.transform.Translate(new Vector3(-calculatedWidth / 2 + BUFFER_IN_PIXELS / 2, 0, 0));

            GameObject eastBorder = new GameObject();
            eastBorder.name = "EastBorder";
            eastBorder.transform.SetParent(visualContainer.transform);
            eastBorder.layer = layer;
            RectTransform eastBorderRectTransform = eastBorder.AddComponent<RectTransform>();
            Image eastBorderImage = eastBorder.AddComponent<Image>();
            eastBorderImage.color = borderColor;
            eastBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BUFFER_IN_PIXELS);
            eastBorderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);
            eastBorder.transform.Translate(new Vector3(calculatedWidth / 2 - BUFFER_IN_PIXELS / 2, 0, 0));
        }

        GameObject headObject = new GameObject();
        headObject.name = "Head";
        headObject.transform.SetParent(visualContainer.transform);
        Image headImage = headObject.AddComponent<Image>();
        Expression expr = this.GetExpression();
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead());
        headImage.sprite = headSprite;
        headImage.transform.localScale *= .25f;
        headImage.transform.position = new Vector3(pieceTopLeftX + (.5f*PIXELS_PER_UNIT), pieceTopLeftY - (.5f*PIXELS_PER_UNIT));

        visualContainer.transform.position = new Vector3(0, 0, 0);

        int numArgs = this.myArguments.Length;
        int currentX = 1; //in units
        int currentY = 1;

        for (int i = 0; i < numArgs; i++) {
            ExpressionPiece arg = this.myArguments[i];

            if (arg == null) {
                currentX++;
            } else {
                currentX += arg.myWidthInUnits;

                if (!arg.expressionName.Equals("_") || (layer == 0)) {
                    float positionX = pieceTopLeftX + PIXELS_PER_UNIT * (currentX - ((.5f * arg.myWidthInUnits) + BUFFER_IN_UNITS));
                    float valToTopAlignArgs = (((this.myHeightInUnits - 1) - arg.myHeightInUnits)) * (PIXELS_PER_UNIT / 2);
                    float positionY = PIXELS_PER_UNIT * ((-0.5f * currentY) + BUFFER_IN_UNITS) + valToTopAlignArgs;

                    GameObject argVisual = arg.GenerateVisual(layer + 1);
                    argVisual.transform.SetParent(visualContainer.transform);
                    argVisual.transform.position = new Vector3(positionX, positionY);
                }
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
        // Debug.Log("^" + this.myExpression + "^ is being dragged");
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

    public static Color GetColorOfSemanticType(SemanticType semType) {
        if (semType.Equals(SemanticType.INDIVIDUAL)) {
            return new Color32(220, 20, 60,  255);
        }

        if (semType.Equals(SemanticType.TRUTH_VALUE)) {
            return new Color32(23, 108, 255,  255);
        }

        if (semType.Equals(SemanticType.PREDICATE)) {
            return new Color32(9, 128, 37, 255);
        }

        if (semType.Equals(SemanticType.RELATION_2)) {
            return new Color32(240, 240, 60, 255);
        }

        if (semType.Equals(SemanticType.RELATION_3)) {
            return new Color32(137, 132, 68, 255);
        }

        if (semType.Equals(SemanticType.RELATION_2_REDUCER)) {
            return new Color32(47, 79, 79, 255);
        }

        if (semType.Equals(SemanticType.DETERMINER)) {
            return new Color32(235, 168, 206, 255);
        }

        if (semType.Equals(SemanticType.QUANTIFIER)) {
            return new Color32(137, 28, 232, 255);
        }

        if (semType.Equals(SemanticType.TRUTH_FUNCTION_1)) {
            return new Color32(180, 180, 180, 255);
        }

        return new Color32(255, 255, 255, 255);
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

        return GetColorOfSemanticType(determiningType) + new Color(0.25f, 0.25f, 0.25f, 0f) - new Color(0, 0, 0, (1 - EXPRESSION_OPACITY));
    }
}
