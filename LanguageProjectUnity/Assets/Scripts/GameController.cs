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
        // LOGIC/FUNCTION WORDS
        
        // truth value constants
        GameObject verumSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject verumSpawnerInstance = Instantiate(verumSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        verumSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner verumSpawnerScript = verumSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression verumExpression = new Word(SemanticType.TRUTH_VALUE, "verum");
        verumSpawnerScript.SetUpSpawner(verumExpression);

        // truth function contstants
        GameObject notSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject notSpawnerInstance = Instantiate(notSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        notSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner notSpawnerScript = notSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression notExpression = new Word(SemanticType.TRUTH_FUNCTION_1, "not");
        notSpawnerScript.SetUpSpawner(notExpression);

        GameObject orSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject orSpawnerInstance = Instantiate(orSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        orSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner orSpawnerScript = orSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression orExpression = new Word(SemanticType.TRUTH_FUNCTION_2, "or");
        orSpawnerScript.SetUpSpawner(orExpression);

        // determiners
        GameObject theSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject theSpawnerInstance = Instantiate(theSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        theSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner theSpawnerScript = theSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression theExpression = new Word(SemanticType.DETERMINER, "the");
        theSpawnerScript.SetUpSpawner(theExpression);

        // quantifiers
        GameObject someSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject someSpawnerInstance = Instantiate(someSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        someSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner someSpawnerScript = someSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression someExpression = new Word(SemanticType.QUANTIFIER, "some");
        someSpawnerScript.SetUpSpawner(someExpression);

        // 2-place relation reducers
        GameObject itselfSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject itselfSpawnerInstance = Instantiate(itselfSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        itselfSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner itselfSpawnerScript = itselfSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression itselfExpression = new Word(SemanticType.RELATION_2_REDUCER, "itself");
        itselfSpawnerScript.SetUpSpawner(itselfExpression);
        
        // proper names
        GameObject aliceSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject aliceSpawnerInstance = Instantiate(aliceSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        aliceSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner aliceSpawnerScript = aliceSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression aliceExpression = new Word(SemanticType.INDIVIDUAL, "alice");
        aliceSpawnerScript.SetUpSpawner(aliceExpression);

        GameObject bobSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject bobSpawnerInstance = Instantiate(bobSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        bobSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner bobSpawnerScript = bobSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression bobExpression = new Word(SemanticType.INDIVIDUAL, "bob");
        bobSpawnerScript.SetUpSpawner(bobExpression);

        // predicates
        GameObject runSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject runSpawnerInstance = Instantiate(runSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        runSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner runSpawnerScript = runSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression runExpression = new Word(SemanticType.PREDICATE, "run");
        runSpawnerScript.SetUpSpawner(runExpression);

        GameObject keySpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject keySpawnerInstance = Instantiate(keySpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        keySpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner keySpawnerScript = keySpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression keyExpression = new Word(SemanticType.PREDICATE, "key");
        keySpawnerScript.SetUpSpawner(keyExpression);

        GameObject appleSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject appleSpawnerInstance = Instantiate(appleSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        appleSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner appleSpawnerScript = appleSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression appleExpression = new Word(SemanticType.PREDICATE, "apple");
        appleSpawnerScript.SetUpSpawner(appleExpression);

        // 2-place relations
        GameObject helpSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject helpSpawnerInstance = Instantiate(helpSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        helpSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner helpSpawnerScript = helpSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression helpExpression = new Word(SemanticType.RELATION_2, "help");
        helpSpawnerScript.SetUpSpawner(helpExpression);

        // 3-place relations
        GameObject giveSpawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject giveSpawnerInstance = Instantiate(giveSpawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        giveSpawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner giveSpawnerScript = giveSpawnerInstance.GetComponent<ExpressionPieceSpawner>();
        Expression giveExpression = new Word(SemanticType.RELATION_3, "give");
        giveSpawnerScript.SetUpSpawner(giveExpression);
    }

    public static Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
