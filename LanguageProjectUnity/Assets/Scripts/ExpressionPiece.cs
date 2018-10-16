using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

// TODO: 

/**
 * A script to be attached to any Expression objects.
 */
public class ExpressionPiece : MonoBehaviour, IPointerClickHandler {
    #region variables
    private const bool DRAW_SUBEXPRESSION_TYPE = false;
    private const bool DRAW_OPEN_ARGUMENT_TYPE = true;
    public const float EXPRESSION_OPACITY = 0.4f;
    private const float BUFFER_IN_UNITS = 0.1f; // the slight space between args, etc. for visual appeal
    public const float PIXELS_PER_UNIT = 40.0f;
    private readonly static float BUFFER_IN_PIXELS = BUFFER_IN_UNITS * PIXELS_PER_UNIT;

    public GameController gameController;
    public string id; // string representation of the expression (e.g. key, the(run), helps(_, bob) etc.)
    public Expression expression;

    private int widthInUnits = 1;
    private int heightInUnits = 1;

    private ExpressionPiece[] arguments;
    private int index = -1;
    private ExpressionPiece parentExpressionPiece;
    #endregion

    /**
     * Called after an ExpressionPiece is created for the first time (all
     * arguments are empty) to set up the piece. I.e. called when an 
     * ExpressionPiece is created by a Spawner, Controller or something that isn't OnDrop().
     */
    public void Initialize(Expression expr) {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        this.expression = expr;
        this.arguments = new ExpressionPiece[expr.GetNumArgs()];
        this.gameObject.transform.position = GetStartingPosition();
        GameObject tabletop = GameObject.Find("Tabletop");
        this.gameObject.transform.SetParent(this.transform.parent.transform);

        if (arguments.Length > 0) {
            this.heightInUnits = 2;
            this.widthInUnits = expr.GetNumArgs() + 1;
        }

        int counter = 0;
        int currentX = 1;
        float calculatedWidth = PIXELS_PER_UNIT * this.widthInUnits;
        float calculatedHeight = PIXELS_PER_UNIT * this.heightInUnits;

        // build empty arguments
        for (int i = 0; i < arguments.Length; i++) {
            Expression argExpression = expr.GetArg(i);
            if (DRAW_OPEN_ARGUMENT_TYPE) {
                GameObject argumentPiece = Resources.Load("Piece") as GameObject;
                argumentPiece.name = "Argument";
                GameObject argumentPieceInstance =
                    Instantiate(argumentPiece, this.transform.position, Quaternion.identity) as GameObject;
                ExpressionPiece argumentPieceScript = argumentPieceInstance.GetComponent<ExpressionPiece>();
                argumentPieceScript.gameController = gameController;
                argumentPieceScript.expression = new Word(expr.GetInputType(counter), "_");
                argumentPieceScript.arguments = new ExpressionPiece[0];

                //NOTE: arg positions are set relative to the center of their container expressions, by the center of the arg
                float overallPieceHeight = calculatedHeight;
                float overallPieceWidth = calculatedWidth;
                float overallPieceTopLeftX = this.transform.position.x - calculatedWidth / 2.0f;
                float overallPieceTopLeftY = this.transform.position.y + calculatedHeight / 2.0f;

                float argTopLeftX = overallPieceTopLeftX + PIXELS_PER_UNIT * currentX;
                float argTopLeftY = overallPieceTopLeftY - 1 * PIXELS_PER_UNIT;
                float argCenterX = argTopLeftX + (PIXELS_PER_UNIT * .5f) - BUFFER_IN_PIXELS; //.5f b/c center of arg w/ width & height 1
                float argCenterY = argTopLeftY - (PIXELS_PER_UNIT * .5f) + BUFFER_IN_PIXELS;

                argumentPieceInstance.transform.SetParent(this.gameObject.transform);
                argumentPieceInstance.transform.position = new Vector3(argCenterX, argCenterY);

                argumentPieceScript.id = "_";
                argumentPieceScript.index = counter;
                argumentPieceScript.SetParentExpressionPiece(this);
                this.arguments[i] = argumentPieceScript;
                counter++;
            }
            currentX++;
        }
    }

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

        for (int i = 0; i < this.arguments.Length; i++) {
            if (this.arguments[i] != null) {
                exprPieceScript.arguments[i] = this.arguments[i].DeepCopy(false);
                exprPieceScript.arguments[i].SetParentExpressionPiece(exprPieceScript);
            }
        }
        return exprPieceScript;
    }

    /**
    * returns false if 2 expressions shouldn't combine with each other
    */
    public bool CombineWith(ExpressionPiece inputExpression, int index) {
        Expression expr = null;

        //try to create new Expression
        try {
            expr = new Phrase(this.expression, inputExpression.expression, index);
        } catch (Exception e) {
            Debug.LogException(e);
            this.gameController.failure.Play();
            return false;
        }

        // generate new Piece
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, GetSpawnPosition(), Quaternion.identity) as GameObject;
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.expression = expr;
        exprPieceScript.arguments = new ExpressionPiece[expr.GetNumArgs()];

        if (exprPieceScript.arguments.Length > 0) {
            exprPieceScript.heightInUnits = 2;
        }

        float originalPieceX = this.transform.position.x;
        float originalPieceY = this.transform.position.y;

       // exprPieceInstance.transform.position = new Vector3(originalPieceX, originalPieceY, 0);

        exprPieceScript.gameController = gameController;
        exprPieceScript.id = expr.ToString();

        exprPieceScript.index = this.index;

        if (this.parentExpressionPiece != null) {
            exprPieceScript.SetParentExpressionPiece(this.parentExpressionPiece.DeepCopy()); // DeepCopy here bc 'this' will get destroyed
        }

        exprPieceScript.widthInUnits = 1;
        exprPieceScript.widthInUnits += inputExpression.widthInUnits;

        exprPieceScript.heightInUnits = Max(exprPieceScript.heightInUnits, inputExpression.heightInUnits + 1);

        //Add the arguments to the new piece
        int counter = -1;
        for (int i = 0; i < arguments.Length; i++) {
            if (this.arguments[i] == null) {
                counter++;
                exprPieceScript.widthInUnits++;
            } else {
                // this happens to every argument, whether blank or filled
                float originalArgLocalX = this.arguments[i].transform.localPosition.x;
                float originalArgLocalY = this.arguments[i].transform.localPosition.y;

                // place a copy of the old argument in the same position in the new expression.
                exprPieceScript.arguments[i] = this.arguments[i].DeepCopy();
                exprPieceScript.arguments[i].transform.SetParent(exprPieceInstance.transform);
                exprPieceScript.arguments[i].gameObject.transform.localPosition = new Vector3(originalArgLocalX, originalArgLocalY, 0);

                // changing the width and height of the new expression
                exprPieceScript.widthInUnits += exprPieceScript.arguments[i].widthInUnits;
                exprPieceScript.heightInUnits = Max(exprPieceScript.heightInUnits, exprPieceScript.arguments[i].heightInUnits + 1);

                // if it's an empty argument slot, then move forward in the
                // argument index, and decrement the argument slot's index by 1 if
                // the argument has already been placed to preserve the new indexing.
                if (this.arguments[i].id.Equals("_")) {
                    if (counter > index) {
                        exprPieceScript.arguments[i].index--;
                    }
                    counter++;
                }
            }

            // place the input expression in the appropriate argument position.
            if (counter == index) {
                float originalArgLocalX = this.arguments[i].transform.localPosition.x;
                float originalArgLocalY = this.arguments[i].transform.localPosition.y;

                //need to destroy the empty arg that this piece is replacing
                Destroy(exprPieceScript.arguments[i].gameObject, 0.0f);

                exprPieceScript.arguments[i] = inputExpression.DeepCopy();
                exprPieceScript.arguments[i].gameObject.transform.localPosition =
                    new Vector3(originalArgLocalX, originalArgLocalY, 0);
                exprPieceScript.arguments[i].transform.SetParent(exprPieceInstance.transform);

                counter++;
                exprPieceScript.widthInUnits--;
            }
        }

        int xPositionInUnits = 1;

        // this is setting the parentexpressionpiece and parent of all of the new
        // expression's arguments as the new expression. This doesn't happen in
        // deepcopy because deepcopy makes a new copy of the expression to set.
        for (int i = 0; i < arguments.Length; i++) {
            float argLocalX = exprPieceScript.arguments[i].transform.localPosition.x;
            float argLocalY = exprPieceScript.arguments[i].transform.localPosition.y;

            float changeY = ((exprPieceScript.heightInUnits - 2) / 2f) * PIXELS_PER_UNIT;

            if (exprPieceScript.arguments[i].id.Equals("_")) {
                exprPieceScript.arguments[i].gameObject.transform.localPosition =
                    new Vector3(
                        ((-0.5f * exprPieceScript.widthInUnits) + xPositionInUnits + 0.5f - BUFFER_IN_UNITS) * PIXELS_PER_UNIT,
                        argLocalY + changeY);
            }

            exprPieceScript.arguments[i].SetParentExpressionPiece(exprPieceScript);
            exprPieceInstance.transform.SetParent(this.transform.parent.transform);

            // Debug.Log(exprPieceScript.expression + "'s width is " + exprPieceScript.widthInUnits);
            // Debug.Log(exprPieceScript.expression + "[" + i + "]'s width is " + arguments[i].widthInUnits);
            xPositionInUnits += exprPieceScript.arguments[i].widthInUnits;
        }

        exprPieceInstance.transform.SetParent(this.transform.parent.transform);
        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());

        int indexToOccupy = this.gameObject.transform.GetSiblingIndex();

        Destroy(this.gameObject, 0.0f);
        Destroy(inputExpression.gameObject, 0.0f);

        exprPiece.transform.SetSiblingIndex(indexToOccupy);

        return true;
    }

    public void SetParentExpressionPiece(ExpressionPiece parent) {
        parentExpressionPiece = parent;
    }

    //TODO: fill this in for the demo w/ a calculated position
    //(horizontal layout group likely will be out of the picture)
    public Vector3 GetStartingPosition() { 
        return new Vector3(300, 50, 0);
    }

    //TODO: fill this in for the demo w/ a calculated position
    //(horizontal layout group likely will be out of the picture)
    public Vector3 GetSpawnPosition() {
        return new Vector3(0, 0, 0);
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
        float calculatedWidth = PIXELS_PER_UNIT * this.widthInUnits;
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
        visContainerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, calculatedHeight - BUFFER_IN_PIXELS);

        if (DRAW_SUBEXPRESSION_TYPE || this.expression.headString.Equals("_") || isFirstLevel) {

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
        headImage.transform.position = new Vector3(pieceTopLeftX + (.5f * PIXELS_PER_UNIT), pieceTopLeftY - (.5f * PIXELS_PER_UNIT));

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
        generatedVisual.transform.position = this.gameObject.transform.position;
        generatedVisual.transform.SetParent(this.gameObject.transform);
    }

    /**
     * Loops through all the things the user could possibly be trying to click 
     * and if the user is clicking an argument of this expression piece rather 
     * than this expression piece itself, "forwards the click" i.e. calls the 
     * OnClick() method of the argument.
     */
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        ExpressionPiece argumentClicked = null;
        foreach (RaycastResult r in results) {
            if (r.gameObject.GetComponent<ExpressionPiece>() != null && r.gameObject.GetComponent<ExpressionPiece>().id.Equals("_")) {
                argumentClicked = r.gameObject.GetComponent<ExpressionPiece>();
                break;
            }
        }

        //if user wasn't clicking any empty arguments, call OnClick() for this ExpressionPiece;
        //otherwise, call OnClick() for the clicked empty arg
        if (argumentClicked == null) {
            if (this.gameController.selectedExpression != null && !this.gameController.selectedExpression.Equals(this)) {
                this.gameController.failure.Play();
            }
            this.OnClick();
        } else {
            argumentClicked.OnClick();
        }
    }

    public void OnClick() {
        // Debug.Log(expression.headString + " just received a click");

        HandleClickSelection();

        // if (this.gameController.selectedExpression == null) {
        //     Debug.Log("selected expression null");
        // } else {
        //     Debug.Log("selected expression is " + this.gameController.selectedExpression.expression.headString);
        //     if (this.gameController.selectedExpression.expression.GetNumArgs() > 0) {
        //         Debug.Log("the selected expression has " + this.gameController.selectedExpression.expression.GetNumArgs() + " arguments");
        //         int counter = 1;
        //         foreach (ExpressionPiece arg in this.gameController.selectedExpression.arguments) {
        //             Debug.Log("Argument number " + counter + " is " + arg.expression.headString + " and located at " + arg.gameObject.transform.position.x + ", " + arg.gameObject.transform.position.y);
        //             counter++;
        //         }
        //     } else {
        //         Debug.Log(this.gameController.selectedExpression.expression.headString + " is located at " + this.gameObject.transform.position.x + ", " + this.gameObject.transform.position.y);
        //     }
        // }

        // if (this.gameController.selectedExpression != null && this.gameController.selectedExpression.expression.GetNumArgs() > 0) {
        //     Debug.Log("the selected expression has " + this.gameController.selectedExpression.expression.GetNumArgs() + " arguments");
        //     int counter = 1;
        //     foreach (ExpressionPiece arg in this.gameController.selectedExpression.arguments) {
        //         Debug.Log("argument number " + counter + " is " + arg.expression.headString + " and located at " + arg.gameObject.transform.position.x + ", " + arg.gameObject.transform.position.y);
        //         counter++;
        //     }
        // }
    }

    /**
     * This method handles setting and removing the gameController's selectedExpression.
     * Also calls CombineWith if appropriate.
     */
    private bool HandleClickSelection() {
        // if the game controller has no selected expression,
        // make this expression the selected expression (unless it's an empty argument slot)
        if (this.gameController.selectedExpression == null) {
            if (!this.id.Equals("_")) {
                this.gameController.selectedExpression = this;
                gameController.ShowPointer();
                this.gameController.highClick.Play();
            } else { // clicking an empty argument, want to make the parent piece selected
                this.gameController.selectedExpression = this.parentExpressionPiece;
                gameController.ShowPointer();
                this.gameController.highClick.Play();
            }
            return false;
        }

        // if we're selecting the same expression, then deselect it
        if (this.gameController.selectedExpression == this && !this.gameController.InSpeakingMode()) {
            this.gameController.selectedExpression = null;
            gameController.HidePointer();
            this.gameController.lowClick.Play();
            return false;
        }

        // if one expression is selected and we click another, try to
        // combine the two expressions. If it works, return true.
        if (this.parentExpressionPiece != null && this.id.Equals("_") && !this.gameController.InSpeakingMode()) {
            bool successfulCombination = this.parentExpressionPiece.CombineWith(this.gameController.selectedExpression, this.index);
            this.gameController.selectedExpression = null;
            gameController.HidePointer();
            
            if (successfulCombination) {
                this.gameController.combineSuccess.Play();
            } else {
                this.gameController.failure.Play();
            }

            return successfulCombination;
        }

        return false;
    }

    private static int Max(int a, int b) {
        return a >= b ? a : b;
    }

    /**
     * Adds a visual border to the passed in visualContainer GameObject.
     */
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

    public int GetHeightInUnits() {
        return heightInUnits;
    }
}
