using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The main Controller class. Sets up the Scene, creates ExpressionPieces, etc.
 */
public class GameController : MonoBehaviour {

    public GameObject keyboardInstance;
    public GameObject canvasInstance;

	void Start () {
        SetUpCanvas();
        SetUpKeyboard();
    }

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        GameObject canvas = Resources.Load("Canvas") as GameObject;
        canvasInstance = Instantiate(canvas, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyboardInstance = canvasInstance.transform.Find("Keyboard").gameObject as GameObject;
    }

    /**
     * Creates the keyboard form which the user can click on ExpressionPieceSpawners which will create
     * ExpressionPieces in the workspace.
     */
    public void SetUpKeyboard() {
        GameObject fSpawner = Resources.Load("FPieceSpawner") as GameObject;
        GameObject fSpawnerInstance = Instantiate(fSpawner, new Vector2(100, 100), Quaternion.identity) as GameObject;
        fSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner fSpawnerScript = fSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression fExpression = new Word(new Arrow(new List<SemanticType> { new E() }, new T()), "f");
        fSpawnerScript.SetUpSpawner("FPiece", fExpression);

        GameObject xSpawner = Resources.Load("XPieceSpawner") as GameObject;
        GameObject xSpawnerInstance = Instantiate(xSpawner, new Vector2(100, 100), Quaternion.identity) as GameObject;
        xSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner xSpawnerScript = xSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression xExpression = new Word(new E(), "x");
        xSpawnerScript.SetUpSpawner("XPiece", xExpression);

        //GameObject x = Resources.Load("XPiece") as GameObject;
        //GameObject xInstance = Instantiate(x, new Vector2(100, 100), Quaternion.identity) as GameObject;
        //xInstance.transform.SetParent(keyboardInstance.transform);
        //ExpressionPiece xPiece = xInstance.GetComponent<ExpressionPiece>();
        //xPiece.SetExpression("x", true, new E(), null);

        //GameObject get = Resources.Load("GetPiece") as GameObject;
        //GameObject getInstance = Instantiate(get, new Vector2(100, 100), Quaternion.identity) as GameObject;
        //getInstance.transform.SetParent(keyboardInstance.transform);

        //GameObject key = Resources.Load("KeyPiece") as GameObject;
        //GameObject keyInstance = Instantiate(key, new Vector2(100, 100), Quaternion.identity) as GameObject;
        //keyInstance.transform.SetParent(keyboardInstance.transform);

        //ExpressionPiece getPiece = getInstance.GetComponent<ExpressionPiece>();
        //getPiece.SetExpression("get", true, new Arrow(new List<SemanticType> { new E() }, new T()), null);

        //ExpressionPiece keyPiece = keyInstance.GetComponent<ExpressionPiece>();
        //keyPiece.SetExpression("key", true, new E(), null);
    }

    //myExpression = new Phrase(phraseParts[0], phraseParts[1]);

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
