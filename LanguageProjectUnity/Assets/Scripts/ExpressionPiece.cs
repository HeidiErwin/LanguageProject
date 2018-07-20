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

    public void Start() {
        Debug.Log(expression.GetHead() + " was just created at " + transform.position.x + ", " + transform.position.y);
    }

    public ExpressionPiece DeepCopy() {
        return DeepCopy(true);
    }

    public ExpressionPiece DeepCopy(bool isFirstCall) {
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        
        exprPieceScript.gameController = this.gameController;
        exprPieceScript.id = this.expression.ToString();
        exprPieceScript.expression = this.expression;
        exprPieceScript.arguments = new ExpressionPiece[this.arguments.Length];
        exprPieceScript.heightInUnits = this.heightInUnits;
        exprPieceScript.widthInUnits = this.widthInUnits;
        exprPieceScript.index = this.index;

        if (this.parentExpressionPiece != null) {
            exprPieceScript.parentExpressionPiece = isFirstCall ? this.parentExpressionPiece.DeepCopy(true) : this.parentExpressionPiece;
        }

        for (int i = 0; i < this.arguments.Length; i++) {
            if (this.arguments[i] != null) {
                exprPieceScript.arguments[i] = this.arguments[i].DeepCopy(false);
            }
        }

        return exprPieceScript;
    }

    /**
     * Called when an ExpressionPiece is created by a Controller or something that isn't OnDrop()
     */
    public void Initialize(Expression expr) {
        this.expression = expr;
        this.arguments = new ExpressionPiece[expr.GetNumArgs()];

        int counter = 0;
        int currentX = 1;
        int currentY = 1;
        for (int i = 0; i < arguments.Length; i++) {
            ExpressionPiece arg = this.arguments[i];
            if (arg == null)
            {
                currentX++;
            }
            else
            {
                currentX += arg.widthInUnits;
            }

            if (expr.GetArg(i) == null && DRAW_OPEN_ARGUMENT_TYPE) {
                GameObject exprPiece = Resources.Load("Piece") as GameObject;
                GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, -100), Quaternion.identity) as GameObject;
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
                float positionY = PIXELS_PER_UNIT * ((-0.5f * currentY) + BUFFER_IN_UNITS) + valToTopAlignArgs;

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

        if (exprPieceScript.arguments.Length == 3) {
            Debug.Log(" ...has 3 arguments as expected.");
        }
        
        
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
                        this.arguments[i].index--;
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

    /** The idea is to make an ExpressionPiece from
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
        Debug.Log("Calling GenerateVisual on ^" + this.expression + "^");

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
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead());
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
        generatedVisual.transform.SetParent(gameObject.transform);
    }

    /**
     * Loops through all the things the user could possibly be trying to click and
     * if the user is clicking an argument of this expression piece rather than this expression piece
     * itself, "forwards the click" i.e. calls the OnClick() method of the argument
     */
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        Debug.Log(expression.GetHead() + " just received a click");

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Debug.Log("currently clicking over " + results.Count + " items");
        ExpressionPiece argumentClicked = null;
        foreach (RaycastResult r in results)
        {
            Debug.Log("r is " + r.gameObject.name);
            if(r.gameObject.GetComponent<ExpressionPiece>() != null && r.gameObject.GetComponent<ExpressionPiece>().expression.GetHead().Equals("_"))
            {
                Debug.Log("empty arg piece!!");
                argumentClicked = r.gameObject.GetComponent<ExpressionPiece>();
                break;
            }
        }

        //if the user wasn't clicking any empty arguments, call OnClick() for this ExpressionPiece
        //otherwise, call OnClick() for the clicked empty arg
        if (argumentClicked == null) {
            OnClick();
        } else {
            argumentClicked.OnClick();
        }
    }

    public void OnClick() {
        InsertArgument();
    }

    private bool InsertArgument() {
        Debug.Log(this.expression);

        if (this.gameController.selectedExpression == null) {
            this.gameController.selectedExpression = this;
            return false;
        }

        if (this.gameController.selectedExpression.Equals(this)) {
            this.gameController.selectedExpression = null;
            return false;
        }

        // this.CombineWith(this.gameController.selectedExpression, 0);

        // THIS
        if (this.parentExpressionPiece != null) {
            if (!this.parentExpressionPiece.CombineWith(this.gameController.selectedExpression, 0)) {
                return false;
            }
        } else {
            this.index++;
            if (!this.arguments[this.index].InsertArgument()) {
                this.index--;
            }
        }
        // TO THIS
        // is fake code. What we need to do is to figure out how we can get the child gameobjects to trigger
        // the event listener, not the parents, so that the argument slots do it directly.
        // the fake code demonstrates that nothing bad is happening in Destroy() or DeepCopy() or whatever
        this.gameController.selectedExpression = null;
        return true;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        Debug.Log(expression.GetHead() + " just started being dragged");
    }

    public void OnDrag(PointerEventData eventData) {
        // Debug.Log("^" + this.expression + "^ is being dragged");
    }

    /**
    * Triggered anytime an object is released on top of this expression. 
    * The image of this expression is updated appropriately.
    */
    public void OnDrop(PointerEventData eventData) {
        ExpressionPiece droppedexpression = eventData.pointerDrag.GetComponent<ExpressionPiece>();
        
        // if (this.parentExpressionPiece != null) {
        //     Debug.Log(this.expression);
        //     Debug.Log(this.parentExpressionPiece);
        //     if (this.id.Equals("_")) {
        //         this.parentExpressionPiece.CombineWith(droppedexpression, this.index);
        //     }
        // } else {
        //     this.index++;
        //     this.arguments[index].OnDrop(eventData);
        // }
        // this.CombineWith(droppedexpression, 0); // TODO get rid of this when argument code is implemented
    }

    /**
    * When dragging this ExpressionPiece ends, change the preview sprites on screen back to 
    * their original sprites.
    */
    public void OnEndDrag(PointerEventData eventData) {}

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

    public void OnDestroy()
    {
        Debug.Log(this.expression.GetHead() + " was just destroyed");
    }
}
