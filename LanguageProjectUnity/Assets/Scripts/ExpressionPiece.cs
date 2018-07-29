using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/**
 * A script to be attached to any Expression objects.
 */
public class ExpressionPiece : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
    private const bool DRAW_SUBEXPRESSION_TYPE = true;
    private const bool DRAW_OPEN_ARGUMENT_TYPE = true;
    public const float EXPRESSION_OPACITY = 0.4f;
    private const float BUFFER_IN_UNITS = 0.1f; // the slight space between args, etc. for visual appeal
    private const float PIXELS_PER_UNIT = 40.0f;
    private readonly static float BUFFER_IN_PIXELS = BUFFER_IN_UNITS * PIXELS_PER_UNIT;

    public GameController gameController;

    public string id; // the string representation of the expression (e.g. key, the(run), helps(_, bob) etc.)

    public Expression expression;

    private int widthInUnits = 1;
    private int heightInUnits = 1;

    private ExpressionPiece[] arguments;

    private int index = -1;
    private ExpressionPiece parentExpressionPiece = null;

    public ExpressionPiece DeepCopy() {
        return DeepCopy(true);
    }

    public ExpressionPiece DeepCopy(bool isFirstCall) {
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        exprPieceInstance.transform.position = this.transform.position;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        
        exprPieceScript.gameController = this.gameController;
        exprPieceScript.id = this.expression.ToString();
        exprPieceScript.expression = this.expression;
        exprPieceScript.arguments = new ExpressionPiece[this.arguments.Length];
        exprPieceScript.heightInUnits = this.heightInUnits;
        exprPieceScript.widthInUnits = this.widthInUnits;
        exprPieceScript.index = this.index;

        if (this.parentExpressionPiece == null) {
            this.transform.SetParent(this.transform.parent);
        } else {
            exprPieceScript.parentExpressionPiece = isFirstCall ? this.parentExpressionPiece.DeepCopy(true) : this.parentExpressionPiece;
            this.transform.SetParent(this.parentExpressionPiece.transform);
        }

        for (int i = 0; i < this.arguments.Length; i++) {
            if (this.arguments[i] != null) {
                exprPieceScript.arguments[i] = this.arguments[i].DeepCopy(false);
            }
        }

        return exprPieceScript;
    }

    // Called when an ExpressionPiece is created by a Controller or something that isn't OnDrop()
    public void Initialize(Expression expr) {
        this.expression = expr;
        this.arguments = new ExpressionPiece[expr.GetNumArgs()];

        int counter = 0;
        int currentX = 1;
        int currentY = 1;
        for (int i = 0; i < arguments.Length; i++) {
            ExpressionPiece arg = this.arguments[i];
            if (arg == null) {
                currentX++;
            } else {
                currentX += arg.widthInUnits;
            }

            if (expr.GetArg(i) == null && DRAW_OPEN_ARGUMENT_TYPE) {
                GameObject exprPiece = Resources.Load("Piece") as GameObject;
                exprPiece.name = "Argument";
                GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
                ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
                exprPieceScript.gameController = gameController;
                exprPieceScript.expression = new Word(expr.GetInputType(counter), "_");
                exprPieceScript.arguments = new ExpressionPiece[0];

                //lines 111 to 121 adapted from GenerateVisual to set x and y of empty args so that they can be clicked
                float calculatedWidth = PIXELS_PER_UNIT * this.widthInUnits;
                float calculatedHeight = PIXELS_PER_UNIT * this.heightInUnits;
                float pieceTopLeftX = 0 - calculatedWidth / 2;
                float pieceTopLeftY = 0 + calculatedHeight / 2;
                heightInUnits = 1; // currently a placeholder; can't calculate heightInUnits for real unless all arg heights are known
                float positionX = pieceTopLeftX + 
                    PIXELS_PER_UNIT * 
                    (currentX - ((.5f * (exprPieceScript.expression.GetNumArgs() + 1)) + 
                    BUFFER_IN_UNITS));
                float valToTopAlignArgs = (((this.heightInUnits - 1) - exprPieceScript.heightInUnits)) * (PIXELS_PER_UNIT / 2);
                float positionY = 0; // PIXELS_PER_UNIT * ((-0.5f * currentY) + BUFFER_IN_UNITS) + valToTopAlignArgs;

                exprPieceInstance.transform.position = new Vector3(positionX, positionY); //TODO: make this the actual position of the argument; Heidi's code rn just has it in the general vicinity

                exprPieceInstance.transform.SetParent(this.transform);
                exprPieceScript.transform.SetParent(this.transform);
                exprPieceScript.id = "_";
                exprPieceScript.index = counter;
                exprPieceScript.parentExpressionPiece = this;
                arguments[i] = exprPieceScript;
                counter++;
            }
        }

        if (arguments.Length > 0) {
            this.heightInUnits = 2;
            this.widthInUnits = expr.GetNumArgs() + 1;
        }
    }

    public bool CombineWith(ExpressionPiece inputExpression, int index) {
        Expression expr = null;
        //try to create new Expression
        try {
            expr = new Phrase(this.expression, inputExpression.expression, index);
        } catch (Exception e) {
            Debug.LogException(e);
            return false;
        }

        // generate new Piece
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.expression = expr;
        exprPieceScript.arguments = new ExpressionPiece[expr.GetNumArgs()];
        
        if (exprPieceScript.arguments.Length > 0) {
            exprPieceScript.heightInUnits = 2;    
        }
        
        exprPieceInstance.transform.SetParent(this.transform.parent.transform);
        exprPieceScript.gameController = gameController;
        exprPieceScript.id = expr.ToString();
        exprPieceScript.arguments = this.arguments;

        exprPieceScript.index = this.index;
        if (this.parentExpressionPiece != null) {
            exprPieceScript.parentExpressionPiece = this.parentExpressionPiece.DeepCopy();
        }
        exprPieceScript.widthInUnits = 1;
        exprPieceScript.widthInUnits += inputExpression.widthInUnits;

        exprPieceScript.heightInUnits = Max(exprPieceScript.heightInUnits, inputExpression.heightInUnits + 1);

        int counter = -1;
        for (int i = 0; i < arguments.Length; i++) {
            if (this.arguments[i] == null) {
                counter++;
                exprPieceScript.widthInUnits++;
            } else {
                exprPieceScript.arguments[i] = arguments[i].DeepCopy();
                exprPieceScript.widthInUnits += exprPieceScript.arguments[i].widthInUnits;

                exprPieceScript.heightInUnits = Max(exprPieceScript.heightInUnits, exprPieceScript.arguments[i].heightInUnits + 1);

                if (this.arguments[i].id.Equals("_")) {
                    if (counter > index) {
                        exprPieceScript.arguments[i].index--;
                    }
                    counter++;
                }
            }
             
            if (counter == index) {
                exprPieceScript.arguments[i] = inputExpression.DeepCopy();
                counter++;
                exprPieceScript.widthInUnits--;
            }
        }

        for (int i = 0; i < arguments.Length; i++) {
            exprPieceScript.arguments[i].parentExpressionPiece = exprPieceScript;
        }

        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());

        int indexToOccupy = this.gameObject.transform.GetSiblingIndex();

        Destroy(this.gameObject, 0.0f);
        Destroy(inputExpression.gameObject, 0.0f);

        exprPiece.transform.SetSiblingIndex(indexToOccupy);

        return true;
    }

    /**
     * The idea is to make an ExpressionPiece from
     * scratch; not as the result of combining them on the
     * workspace. This is for down-the-road when NPCs
     * can make expressions, and when users can save
     * expressions onto their keyboard
     */
    public void FromScratch(Expression expr) {
        FromScratch(expr, true);
    }

    private void FromScratch(Expression expr, bool isTopLayer) {
        for (int i = 0; i < expr.GetNumArgs(); i++) {
            Expression arg = expr.GetArg(i);
            if (expr.GetArg(i) != null) {
                // TODO do something here to recursively make the argument ExpressionPieces,
                // and then make the ExpressionPiece for the whole expression idk
            }
        }
        if (isTopLayer) {
            Initialize(expr);    
        }
    }

    /**
     * Creates a GameObject that will be a child of this ExpressionPiece, and will
     * have children GameObjects which hold Images (the different visuals of this 
     * ExpressionPiece).
     * This ExpressionPiece will later set the returned GameObject as a child of itself.
     */
    public GameObject GenerateVisual() {
        return GenerateVisual(true);
    }
    private GameObject GenerateVisual(bool isFirstLevel) {
        GameObject exprPiece = this.gameObject;

        RectTransform pieceRect = exprPiece.GetComponent<RectTransform>();
        float calculatedWidth  = PIXELS_PER_UNIT * this.widthInUnits;
        float calculatedHeight = PIXELS_PER_UNIT * this.heightInUnits;
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth);
        pieceRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight);

        float pieceCenterX = 0;
        float pieceCenterY = 0;
        float pieceTopLeftX = pieceCenterX - calculatedWidth / 2;
        float pieceTopLeftY = pieceCenterY + calculatedHeight / 2;

        GameObject visualContainer = new GameObject();
        visualContainer.name = "VisualContainer";
        visualContainer.transform.SetParent(exprPiece.transform);
        visualContainer.layer = 0;

        RectTransform visContainerRectTransform = visualContainer.AddComponent<RectTransform>();
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, calculatedWidth - BUFFER_IN_PIXELS);
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight  - BUFFER_IN_PIXELS);

        if (DRAW_SUBEXPRESSION_TYPE || isFirstLevel) {

            Image bgImage = visualContainer.AddComponent<Image>();
            bgImage.color = isFirstLevel ? this.expression.type.outputColor : this.expression.type.color + new Color(0.25f, 0.25f, 0.25f, 0f) - new Color(0, 0, 0, (1 - EXPRESSION_OPACITY));
            
            Color borderColor = this.expression.type.color;

            GenerateBorder(borderColor, visualContainer, calculatedWidth, calculatedHeight, "North");
            GenerateBorder(borderColor, visualContainer, calculatedWidth, calculatedHeight, "South");
            GenerateBorder(borderColor, visualContainer, calculatedWidth, calculatedHeight, "West");
            GenerateBorder(borderColor, visualContainer, calculatedWidth, calculatedHeight, "East");
        }

        GameObject headObject = new GameObject();
        headObject.name = "Head";
        headObject.transform.SetParent(visualContainer.transform);
        Image headImage = headObject.AddComponent<Image>();
        Expression expr = this.expression;
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.headString);
        headImage.sprite = headSprite;
        headImage.transform.localScale *= .25f;
        headImage.transform.position = new Vector3(pieceTopLeftX + (.5f*PIXELS_PER_UNIT), pieceTopLeftY - (.5f*PIXELS_PER_UNIT));

        visualContainer.transform.position = new Vector3(0, 0, 0);

        int numArgs = this.arguments.Length;
        int currentX = 1; //in units
        int currentY = 1;

        for (int i = 0; i < numArgs; i++) {
            ExpressionPiece arg = this.arguments[i];

            if (arg == null) {
                currentX++;
            } else {
                currentX += arg.widthInUnits;

                if (!arg.id.Equals("_") || isFirstLevel) {
                    float positionX = pieceTopLeftX + PIXELS_PER_UNIT * (currentX - ((.5f * arg.widthInUnits) + BUFFER_IN_UNITS));
                    float valToTopAlignArgs = (((this.heightInUnits - 1) - arg.heightInUnits)) * (PIXELS_PER_UNIT / 2);
                    float positionY = PIXELS_PER_UNIT * ((-0.5f * currentY) + BUFFER_IN_UNITS) + valToTopAlignArgs;

                    GameObject argVisual = arg.GenerateVisual(false);
                    argVisual.transform.SetParent(visualContainer.transform);
                    argVisual.transform.position = new Vector3(positionX, positionY);
                }
            }
        }
        return visualContainer;
    }

    public void SetVisual(GameObject generatedVisual) {
        generatedVisual.transform.SetParent(this.gameObject.transform);
    }

    /**
     * Loops through all the things the user could possibly be trying to click and
     * if the user is clicking an argument of this expression piece rather than this expression piece
     * itself, "forwards the click" i.e. calls the OnClick() method of the argument
     */
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        // Debug.Log(expression.headString + " just received a click");

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        // Debug.Log("currently clicking over " + results.Count + " items");
        ExpressionPiece argumentClicked = null;
        foreach (RaycastResult r in results) {
            // Debug.Log("r is " + r.gameObject.name);
            if(r.gameObject.GetComponent<ExpressionPiece>() != null && r.gameObject.GetComponent<ExpressionPiece>().id.Equals("_")) {
                // Debug.Log("empty arg piece!!");
                argumentClicked = r.gameObject.GetComponent<ExpressionPiece>();
                // Debug.Log("Is argument null? => " + (argumentClicked == null));
                break;
            }
        }

        //if the user wasn't clicking any empty arguments, call OnClick() for this ExpressionPiece;
        //otherwise, call OnClick() for the clicked empty arg
        if (argumentClicked == null) {
            Debug.Log("No argument clicked");
            this.OnClick();
        } else {
            Debug.Log("argument is clicked");
            argumentClicked.OnClick();
        }
    }

    void IDropHandler.OnDrop(PointerEventData eventData) {}
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) {}
    void IDragHandler.OnDrag(PointerEventData eventData) {}
    void IEndDragHandler.OnEndDrag(PointerEventData eventData) {}

    public void OnClick() {
        if(this.gameController.selectedExpression == null) {
            Debug.Log("before insertarg, selected expression null");
        }
        else {
            Debug.Log("before insertarg, selected expression is " + this.gameController.selectedExpression.expression.headString);
            if (this.gameController.selectedExpression.expression.GetNumArgs() > 0) {
                Debug.Log("the selected expression has " + this.gameController.selectedExpression.expression.GetNumArgs() + " arguments");
                int counter = 1;
                foreach (ExpressionPiece arg in this.gameController.selectedExpression.arguments) {
                    Debug.Log("Argument number " + counter + " is " + arg.expression.headString + " and located at " + arg.gameObject.transform.position.x + ", " + arg.gameObject.transform.position.y);
                    counter++;
                }
            }
            else {
                Debug.Log(this.gameController.selectedExpression.expression.headString + " is located at " + this.gameObject.transform.position.x + ", " + this.gameObject.transform.position.y);
            }
        }
        InsertArgument();
        if (this.gameController.selectedExpression == null) {
            Debug.Log("after insertarg, selected expression null");
        }
        else {
            Debug.Log("after insertarg, selected expression is " + this.gameController.selectedExpression.expression.headString);
            if (this.gameController.selectedExpression.expression.GetNumArgs() > 0) {
                Debug.Log("the selected expression has " + this.gameController.selectedExpression.expression.GetNumArgs() + " arguments");
                int counter = 1;
                foreach (ExpressionPiece arg in this.gameController.selectedExpression.arguments) {
                    Debug.Log("Argument number " + counter + " is " + arg.expression.headString + " and located at " + arg.gameObject.transform.position.x + ", " + arg.gameObject.transform.position.y);
                    counter++;
                }
            } else {
                Debug.Log(this.gameController.selectedExpression.expression.headString + " is located at " + this.gameObject.transform.position.x + ", " + this.gameObject.transform.position.y);
            }

        }
    }

    private bool InsertArgument() {
        // if the game controller has no selected expression,
        // make this expression the selected expression (unless it's an empty argument slot)
        if (this.gameController.selectedExpression == null) {
            if (!this.id.Equals("_")) {
                this.gameController.selectedExpression = this;
            }
            return false;
        }

        // if we're selecting the same expression, then deselect it
        if (this.gameController.selectedExpression.Equals(this)) {
            this.gameController.selectedExpression = null;
            return false;
        }

        // if one expression is selected and we click another, try to
        // combine the two expressions. If it works, return true.
        if (this.parentExpressionPiece != null) {
            bool toReturn = this.parentExpressionPiece.CombineWith(this.gameController.selectedExpression, this.index);
            this.gameController.selectedExpression = null;
            return toReturn;
        }

        return false;
    }

    private static int Max(int a, int b) {
        return a >= b ? a : b;
    }

    private static void GenerateBorder(Color color, GameObject visualContainer, float width, float height, String direction) {
        GameObject border = new GameObject();
        border.name = direction + "Border";
        border.transform.SetParent(visualContainer.transform);
        border.layer = 0;
        RectTransform borderRectTransform = border.AddComponent<RectTransform>();
        Image borderImage = border.AddComponent<Image>();
        borderImage.color = color;

        if (direction == "North") {
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BUFFER_IN_PIXELS);
            border.transform.Translate(new Vector3(0, (height - BUFFER_IN_PIXELS) / 2, 0));
            return;
        }

        if (direction == "South") {
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BUFFER_IN_PIXELS);
            border.transform.Translate(new Vector3(0, (-height + BUFFER_IN_PIXELS) / 2, 0));
            return;
        }

        if (direction == "West") {
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BUFFER_IN_PIXELS);
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            border.transform.Translate(new Vector3((-width + BUFFER_IN_PIXELS) / 2, 0, 0));
            return;
        }

        if (direction == "East") {
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BUFFER_IN_PIXELS);
            borderRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            border.transform.Translate(new Vector3((width - BUFFER_IN_PIXELS) / 2, 0, 0));
            return;
        }
    }

    public override String ToString() {
        return expression.ToString();
    }
}
