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

        if (Input.GetKeyDown(KeyCode.Space)) {
            canvasInstance.SetActive(!keyboardInstance.activeInHierarchy);
        }
    }

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        canvasInstance.SetActive(true);
        //GameObject canvas = Resources.Load("Canvas") as GameObject;
        //canvasInstance = Instantiate(canvas, new Vector2(100, 100), Quaternion.identity) as GameObject;
        keyboardInstance = canvasInstance.transform.Find("Keyboard").gameObject as GameObject;
    }

    private void SetUpPlayer() {
        GameObject player = Resources.Load("Player") as GameObject;
        GameObject playerInstance = Instantiate(player, new Vector2(0, 0), Quaternion.identity) as GameObject;
        playerInstance.transform.SetParent(canvasInstance.transform);
    }

    private void SetUpSpawner(Expression e) {
        GameObject spawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject spawnerInstance = Instantiate(spawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        spawnerInstance.transform.SetParent(keyboardInstance.transform);
        ExpressionPieceSpawner spawnerScript = spawnerInstance.GetComponent<ExpressionPieceSpawner>();
        spawnerScript.SetUpSpawner(e, this);
    }

    /** Creates the keyboard from which the user can click on ExpressionPieceSpawners,
    * which will create ExpressionPieces in the workspace.
    */
    public void SetUpKeyboard() {
        // LOGIC/FUNCTION WORDS
        // determiners
        SetUpSpawner(Expression.NO);
        //SetUpSpawner(Expression.A);
        //SetUpSpawner(Expression.TWO);
        //SetUpSpawner(Expression.THREE);
        //SetUpSpawner(Expression.EVERY);

        // CONTENT WORDS   
        // proper names
        SetUpSpawner(Expression.BOB);
        SetUpSpawner(Expression.EVAN);

        // predicates
        SetUpSpawner(Expression.FOUNTAIN);
        SetUpSpawner(Expression.LAMP);
        //SetUpSpawner(Expression.ACTIVE);
        //SetUpSpawner(Expression.INACTIVE);
        //SetUpSpawner(Expression.KING);
        //SetUpSpawner(Expression.YELLOW);
        //SetUpSpawner(Expression.GREEN);
        //SetUpSpawner(Expression.BLUE);
        //SetUpSpawner(Expression.RED);
        //SetUpSpawner(Expression.IN_YOUR_AREA);
        //SetUpSpawner(Expression.IN_YELLOW_AREA);
        //SetUpSpawner(Expression.IN_GREEN_AREA);
        //SetUpSpawner(Expression.IN_BLUE_AREA);
        //SetUpSpawner(Expression.IN_RED_AREA);

        //for testing ExpressionPiece object placements, delete later:
        SetUpSpawner(new Word(SemanticType.RELATION_2, "help"));


        /** BELOW ARE PIECES TO BE USED LATER, NOT IN DEMO:
          // truth value constants
              SetUpSpawner(Expression.VERUM);

          // truth function contstants
              SetUpSpawner(Expression.NOT);
              SetUpSpawner(Expression.OR);

          // quantifiers
              SetUpSpawner(Expression.SOME);

          // 2-place relation reducers
              SetUpSpawner(Expression.ITSELF);

          // 2-place relations
              SetUpSpawner(Expression.HELP);

          // 3-place relations
              SetUpSpawner(Expression.GIVE);

          */
    }

    // updates the keyboard so that the tabToDisplayIndex-th tab is active,
    // and all other tabs become inactive
    public void SwitchKeyboardTab(int tabToDisplayIndex) {
        Debug.Log(tabToDisplayIndex + " was pressssssed");
        if (tabToDisplayIndex == 0) { //Individual
            keyboardInstance.gameObject.GetComponent<Image>().color = new Color32(220, 20, 60, 255);
        } else if (tabToDisplayIndex == 1) { // Determiner
            keyboardInstance.gameObject.GetComponent<Image>().color = new Color32(255, 203, 0, 255);
        } else if (tabToDisplayIndex == 2) { // Predicate
            keyboardInstance.gameObject.GetComponent<Image>().color = new Color32(9, 128, 37, 255);
        }
    }
}
