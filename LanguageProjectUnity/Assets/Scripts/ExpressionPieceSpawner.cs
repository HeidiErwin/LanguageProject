using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A script for the expressions on the keyboard which aren't technically ExpressionPieces,
 * but rather create ExpressionPieces when the user attempts to drag from them.
 */
public class ExpressionPieceSpawner : MonoBehaviour /*, IPointerClickHandler */ {
    private Expression expression;
    /**
     * Sets the name and Expression of this ExpressionPieceSpawner.
     * This code would be put in a constructor, but unfortunately Unity does not 
     * allow instantiating new scripts and then attaching those to MonoBehaviour 
     * scripts (scripts that attach to GameObjects). 
     * So SetUpSpawner must be called once the ExpressionPieceSpawner is created 
     * and this script can be accessed.
     */
    public void SetUpSpawner(Expression expr) {
        this.expression = expr;
        SetUpSpawnerVisual();
    }

    /**
     * Creates an new ExpressionPiece based on this ExpressionPieceSpawner
     */
    public ExpressionPiece MakeNewExpressionPiece() {
        GameObject workspace = GameObject.Find("Workspace");
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        exprPieceInstance.transform.SetParent(workspace.transform);
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.Initialize(expression);
        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());

        return exprPieceInstance.GetComponent<ExpressionPiece>();
    }

    // *
    // * When the user tries to drag this ExpressionPieceSpawner, an ExpressionPiece 
    // * will get created based on the name of this ExpressionPieceSpawner.
    
    // void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
    //     ExpressionPiece exprPiece = MakeNewExpressionPiece();
    // }

    /*
     * Set up the visual for this spawner
     */
    public void SetUpSpawnerVisual() {
        // RectTransform pieceRect = gameObject.GetComponent<RectTransform>();
        GameObject nameObject = new GameObject();
        nameObject.name = "Name";
        nameObject.transform.SetParent(gameObject.transform);
        Image headImage = nameObject.AddComponent<Image>();
        // Sprite headSprite = Resources.Load<Sprite>("Symbols/" + this.expression.headString);
        Sprite headSprite = Resources.Load<Sprite>("English/" + this.expression.headString);
        if (headSprite == null) {
            headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + this.expression.headString);
        }
        headImage.sprite = headSprite;
        headImage.transform.localScale = headImage.transform.localScale * .3f;
        headImage.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y);

        //set color
        Image[] bgImage = gameObject.GetComponents<Image>();
        bgImage[0].rectTransform.sizeDelta = new Vector2(40f, 40f);
        bgImage[0].color = this.expression.type.color - (new Color(0, 0, 0, 0.5f));
    }
}
