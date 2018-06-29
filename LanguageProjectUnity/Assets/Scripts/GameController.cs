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
        GameObject fSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject fSpawnerInstance = Instantiate(fSpawner, new Vector2(100, 100), Quaternion.identity) as GameObject;
        fSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner fSpawnerScript = fSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression fExpression = new Word(new Arrow(new List<SemanticType> { new E() }, new T()), "f");
        fSpawnerScript.SetUpSpawner(fExpression);

        GameObject xSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject xSpawnerInstance = Instantiate(xSpawner, new Vector2(100, 100), Quaternion.identity) as GameObject;
        xSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner xSpawnerScript = xSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression xExpression = new Word(new E(), "x");
        xSpawnerScript.SetUpSpawner(xExpression);

        GameObject helpsSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject helpsSpawnerInstance = Instantiate(helpsSpawner, new Vector2(100, 100), Quaternion.identity) as GameObject;
        helpsSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner helpsSpawnerScript = helpsSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression helpsExpression = new Word(new Arrow(new List<SemanticType> { new E(), new E() }, new T()), "helps");
        helpsSpawnerScript.SetUpSpawner(helpsExpression);
    }

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
