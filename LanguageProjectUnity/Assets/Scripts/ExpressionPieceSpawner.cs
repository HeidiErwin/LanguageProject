using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A script for the expressions on the keyboard which aren't technically ExpressionPieces,
 * but rather create ExpressionPieces when the user attempts to drag from them.
 */
public class ExpressionPieceSpawner : MonoBehaviour, IPointerClickHandler {

    private Expression myExpression;

    /**
     * Sets the name and Expression of this ExpressionPieceSpawner.
     * This code would be put in a constructor, but unfortunately Unity does not 
     * allow instantiating new scripts and then attaching those to MonoBehaviour 
     * scripts (scripts that attach to GameObjects). 
     * So SetUpSpawner must be called once the ExpressionPieceSpawner is created 
     * and this script can be accessed.
     */
    public void SetUpSpawner(Expression expr) {
        this.myExpression = expr;
        SetUpSpawnerVisual(expr);
    }

    /**
     * Creates an new ExpressionPiece based on this ExpressionPieceSpawner
     */
    public ExpressionPiece MakeNewExpressionPiece() {
        GameObject workspace = this.transform.parent.parent.Find("Tabletop").gameObject;

        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        exprPieceInstance.transform.SetParent(workspace.transform);
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.SetExpression(myExpression);
        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual(exprPieceScript));

        return exprPieceInstance.GetComponent<ExpressionPiece>();
    }

    /**
    * When the user tries to drag this ExpressionPieceSpawner, an ExpressionPiece 
    * will get created based on the name of this ExpressionPieceSpawner.
    */
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        ExpressionPiece exprPiece = MakeNewExpressionPiece();
    }

    /*
     * Set up the visual for this spawner
     */
    public void SetUpSpawnerVisual(Expression expr) {
        RectTransform pieceRect = gameObject.GetComponent<RectTransform>();
        float pieceTopLeftY = gameObject.transform.position.y + pieceRect.rect.height / 2;
        float pieceTopLeftX = gameObject.transform.position.x - pieceRect.rect.width / 2;
        GameObject nameObject = new GameObject();
        nameObject.name = "Name";
        nameObject.transform.SetParent(gameObject.transform);
        Image headImage = nameObject.AddComponent<Image>();
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + expr.GetHead() + "Symbol");
        headImage.sprite = headSprite;
        headImage.transform.localScale = headImage.transform.localScale * .25f;
        headImage.transform.position = new Vector3(pieceTopLeftX + 15, pieceTopLeftY - 15);

        //set color
        Image[] bgImage = gameObject.GetComponents<Image>();
        bgImage[0].color = ExpressionPiece.GetColorOfOutputType(expr.GetSemanticType());
    }
}