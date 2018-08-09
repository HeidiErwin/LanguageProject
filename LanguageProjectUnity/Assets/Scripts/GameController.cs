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
    public ExpressionPiece selectedExpression;

	void Start () {
        SetUpCanvas();
        SetUpKeyboard();
        SetUpPlayer();
    }

    public void Update()
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition), 100.0f);

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.transform.GetComponent<ExpressionPiece>() != null)
            {
                //Debug.Log(hit.transform.GetComponent<ExpressionPiece>().expression.headString + " was hit in the raycastall");
            }
        }
    }

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        GameObject canvas = Resources.Load("Canvas") as GameObject;
        canvasInstance = Instantiate(canvas, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyboardInstance = canvasInstance.transform.Find("Keyboard").gameObject as GameObject;
    }

    private void SetUpPlayer() {
        GameObject player = Resources.Load("Player") as GameObject;
        GameObject playerInstance = Instantiate(player, new Vector2(0, 0), Quaternion.identity) as GameObject;
        playerInstance.transform.SetParent(keyboardInstance.transform);
    }

    private void SetUpSpawner(Word w) {
        GameObject spawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject spawnerInstance = Instantiate(spawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        spawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner spawnerScript = spawnerInstance.GetComponent<ExpressionPieceSpawner>();
        spawnerScript.SetUpSpawner(w, this);
    }

    /** Creates the keyboard from which the user can click on ExpressionPieceSpawners,
    * which will create ExpressionPieces in the workspace.
    */
    public void SetUpKeyboard() {
        // LOGIC/FUNCTION WORDS
        // determiners
        SetUpSpawner(new Word(SemanticType.DETERMINER, "the"));
        //SetUpSpawner(new Word(SemanticType.DETERMINER, "a"));
        //SetUpSpawner(new Word(SemanticType.DETERMINER, "two"));
        //SetUpSpawner(new Word(SemanticType.DETERMINER, "three"));
        //SetUpSpawner(new Word(SemanticType.DETERMINER, "every"));

        // CONTENT WORDS   
        // proper names
        SetUpSpawner(new Word(SemanticType.INDIVIDUAL, "business-man"));
        SetUpSpawner(new Word(SemanticType.INDIVIDUAL, "evil-man"));

        // predicates
        SetUpSpawner(new Word(SemanticType.PREDICATE, "fountain"));
        SetUpSpawner(new Word(SemanticType.PREDICATE, "lamp"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "active"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "inactive"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "king"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "yellow"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "green"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "blue"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "red"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "near-you"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "in-yellow-area"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "in-green-area"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "in-blue-area"));
        //SetUpSpawner(new Word(SemanticType.PREDICATE, "in-red-area"));

        //for testing ExpressionPiece object placements, delete later:
        SetUpSpawner(new Word(SemanticType.RELATION_2, "help"));


        /** BELOW ARE PIECES NOT TO BE USED LATER, NOT IN DEMO:
          // truth value constants
              SetUpSpawner(new Word(SemanticType.TRUTH_VALUE, "verum"));

          // truth function contstants
              SetUpSpawner(new Word(SemanticType.TRUTH_FUNCTION_1, "not"));
              SetUpSpawner(new Word(SemanticType.TRUTH_FUNCTION_2, "or"));

          // quantifiers
              SetUpSpawner(new Word(SemanticType.QUANTIFIER, "some"));

          // 2-place relation reducers
              SetUpSpawner(new Word(SemanticType.RELATION_2_REDUCER, "itself"));

          // 2-place relations
              SetUpSpawner(new Word(SemanticType.RELATION_2, "help"));

          // 3-place relations
              SetUpSpawner(new Word(SemanticType.RELATION_3, "give"));

          */
    }
}
