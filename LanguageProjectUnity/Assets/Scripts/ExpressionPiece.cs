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

    private bool isKeyboardPiece;

    private Expression myExpression;

    //the expressions on screen that can accept this expression
    List<ExpressionPiece> compatibleAcceptingExpressions;

    public ExpressionPiece(string expressionName, Sprite defaultSprite, Sprite currentSprite, bool isKeyboardPiece) {
        this.expressionName = expressionName;
        this.defaultSprite = defaultSprite;
        this.currentSprite = currentSprite;
        this.isKeyboardPiece = isKeyboardPiece;
    }

    /**
     * This method should be called after the ExpressionPiece is made, so that 
     * this piece can remember if it is part of the keyboard
     * This is important because, for instance, the parent of a piece originally 
     * in the Keyboard changes from the Keyboard to the Canvas once the piece is 
     * picked up [in the Draggable script], isKeyboardPiece will indicate that this
     * Piece belongs in the keyboard.
     */
    public void SetKeyboardPiece(bool isKeyboardPiece) {
        this.isKeyboardPiece = isKeyboardPiece;
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

    public void SetExpression(Expression expression) {
        myExpression = expression;
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
     * shape that shows that this E can be inserted into the shape.
     * 
     * If a piece is part of the Keyboard, it does not get moved, but rather a copy of it is made, which
     * gets moved.
     */
    public void OnBeginDrag(PointerEventData eventData) {
        if (isKeyboardPiece) {
           Debug.Log("a keyboard piece!");
            GameObject keyboard = this.transform.parent.Find("Keyboard").gameObject;

            //remove any occurences of "(Clone)" from this object's name
            int indexOfParentheses = this.gameObject.name.IndexOf('('); 
            string trimmedObjectName;
            if (indexOfParentheses != -1) {
                trimmedObjectName = this.gameObject.name.Substring(0, indexOfParentheses);
                Debug.Log("huh " + trimmedObjectName);
            } else {
                trimmedObjectName = gameObject.name;
            }

            GameObject copy = Resources.Load(trimmedObjectName) as GameObject;
            Debug.Log("this.gameObject.name is " + this.gameObject.name);
            GameObject copyInstance = Instantiate(copy, new Vector2(100, 100), Quaternion.identity) as GameObject;
            copyInstance.transform.SetParent(keyboard.transform);
            ExpressionPiece copiedPiece = copyInstance.GetComponent<ExpressionPiece>();
            copiedPiece.SetExpression(myExpression);
            copiedPiece.SetKeyboardPiece(true);

            this.isKeyboardPiece = false;
        }

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
