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
        //instance.transform.SetParent(instance2.transform, false);

    }

    void Update () {
		
	}

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        GameObject canvas = Resources.Load("Canvas") as GameObject;
        canvasInstance = Instantiate(canvas, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyboardInstance = canvasInstance.transform.Find("Keyboard").gameObject;

        Debug.Log("helloooo" + keyboardInstance.GetComponent<Image>().name); //debug print, remove later
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

        ExpressionPiece keyPiece = keyInstance.GetComponent<ExpressionPiece>();
        keyPiece.SetExpression("key", true, new E(), null);

        ExpressionPiece xPiece = keyInstance.GetComponent<ExpressionPiece>();
        xPiece.SetExpression("x", true, new E(), null);

        ExpressionPiece getPiece = getInstance.GetComponent<ExpressionPiece>();
        getPiece.SetExpression("get", true, new Arrow(new List<SemanticType> { new E() }, new T()), null); 
        
    }

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
