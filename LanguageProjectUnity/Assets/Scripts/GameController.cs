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
        GameObject runsSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject runsSpawnerInstance = Instantiate(runsSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        runsSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner runsSpawnerScript = runsSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression runsExpression = new Word(new Arrow(new List<SemanticType> { new E() }, new T()), "runs");
        runsSpawnerScript.SetUpSpawner(runsExpression);

        GameObject keySpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject keySpawnerInstance = Instantiate(keySpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        keySpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner keySpawnerScript = keySpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression keyExpression = new Word(new E(), "key");
        keySpawnerScript.SetUpSpawner(keyExpression);

        GameObject helpsSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject helpsSpawnerInstance = Instantiate(helpsSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        helpsSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner helpsSpawnerScript = helpsSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression helpsExpression = new Word(new Arrow(new List<SemanticType> { new E(), new E() }, new T()), "helps");
        helpsSpawnerScript.SetUpSpawner(helpsExpression);

        GameObject theSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject theSpawnerInstance = Instantiate(theSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        theSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner theSpawnerScript = theSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression theExpression = new Word(new Arrow(new List<SemanticType> {
            new Arrow(new List<SemanticType> { new E() }, new T()) }, new E()), "the");
        theSpawnerScript.SetUpSpawner(theExpression);
    }

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
