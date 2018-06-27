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

    void Update () { }

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        GameObject canvas = Resources.Load("Canvas") as GameObject;
        canvasInstance = Instantiate(canvas, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyboardInstance = canvasInstance.transform.Find("Keyboard").gameObject as GameObject;
    }

    /**
     * Creates the keyboard form which the user can select and drag ExpressionPieces
     */
    public void SetUpKeyboard() {
        GameObject fx = Resources.Load("FPiece") as GameObject;
        GameObject fInstance = Instantiate(fx, new Vector2(100, 100), Quaternion.identity) as GameObject;
        fInstance.transform.SetParent(keyboardInstance.transform);

        GameObject x = Resources.Load("XPiece") as GameObject;
        GameObject xInstance = Instantiate(x, new Vector2(100, 100), Quaternion.identity) as GameObject;
        xInstance.transform.SetParent(keyboardInstance.transform);

        GameObject get = Resources.Load("GetPiece") as GameObject;
        GameObject getInstance = Instantiate(get, new Vector2(100, 100), Quaternion.identity) as GameObject;
        getInstance.transform.SetParent(keyboardInstance.transform);

        GameObject key = Resources.Load("KeyPiece") as GameObject;
        GameObject keyInstance = Instantiate(key, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyInstance.transform.SetParent(keyboardInstance.transform);

        ExpressionPiece fPiece = fInstance.GetComponent<ExpressionPiece>();
        fPiece.SetExpression("f", true, new Arrow(new List<SemanticType> { new E() }, new T()), null);
        fPiece.SetKeyboardPiece(true);

        ExpressionPiece xPiece = xInstance.GetComponent<ExpressionPiece>();
        xPiece.SetExpression("x", true, new E(), null);
        xPiece.SetKeyboardPiece(true);

        ExpressionPiece getPiece = getInstance.GetComponent<ExpressionPiece>();
        getPiece.SetExpression("get", true, new Arrow(new List<SemanticType> { new E() }, new T()), null);
        getPiece.SetKeyboardPiece(true);

        ExpressionPiece keyPiece = keyInstance.GetComponent<ExpressionPiece>();
        keyPiece.SetExpression("key", true, new E(), null);
        keyPiece.SetKeyboardPiece(true);
    }

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
