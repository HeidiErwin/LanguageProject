using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The main Controller class. Sets up the Scene, creates ExpressionPieces, etc.
 */
public class GameController : MonoBehaviour {

    private GameObject currentKeyboard; // the subsection of the keyboard we're currently showing (e.g. Determiners)
    public GameObject canvasInstance;
    [SerializeField] private GameObject pointer; // arrow pointing to selected expression
    [SerializeField] private GameObject individualKeyboard;
    [SerializeField] private GameObject quantifierKeyboard;
    [SerializeField] private GameObject predicateKeyboard;
    [SerializeField] private GameObject relation2Keyboard;
    [SerializeField] private GameObject truthFunction1Keyboard;
    [SerializeField] private GameObject truthFunction2Keyboard;
    [SerializeField] private GameObject individualTruthRelationKeyboard;
    [SerializeField] private GameObject helpScreen;
    private bool keyboardOnBeforeHelpShown = true;

    public AudioSource highClick;
    public AudioSource lowClick;
    public AudioSource combineSuccess;
    public AudioSource placeExpression;
    public AudioSource failure;

    public const int PIECES_PER_ROW = 10;

    private bool inSpeakingMode = false; // true only right after user has submitted an expression (pressed checkmark button) and before user has selected an NPC to speak to

    public ExpressionPiece selectedExpression;

	void Start() {
        // Debug.Log(Screen.height/20.0f + " is the screen height");
        SetUpCanvas();
        SetUpKeyboard();
        SetUpPlayer();
        currentKeyboard = individualKeyboard;
        canvasInstance.SetActive(false);
        currentKeyboard.SetActive(true);
    }

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !helpScreen.activeInHierarchy) {
            canvasInstance.SetActive(!canvasInstance.activeInHierarchy);
            helpScreen.SetActive(helpScreen.activeInHierarchy);
            if (canvasInstance.activeInHierarchy) {
                highClick.Play();
            } else {
                lowClick.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            ShowOrHideHelpScreen();
        }
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void ShowOrHideHelpScreen() {
        bool helpScreenShowing = helpScreen.activeInHierarchy;
        if (helpScreenShowing) {
            helpScreen.SetActive(false);
            canvasInstance.SetActive(keyboardOnBeforeHelpShown);
            lowClick.Play();
        } else {
            keyboardOnBeforeHelpShown = canvasInstance.activeInHierarchy;
            canvasInstance.SetActive(false);
            helpScreen.SetActive(true);
            highClick.Play();
        }
    }

    /**
     * Creates the Canvas which holds the keyboard and workspace
     */
    public void SetUpCanvas() {
        canvasInstance.SetActive(true);
    }

    private void SetUpPlayer() {
        GameObject player = Resources.Load("Player") as GameObject;
        GameObject playerInstance = Instantiate(player, new Vector2(0, 0), Quaternion.identity) as GameObject;
    }

    private void SetUpSpawner(Expression e) {
        SemanticType type = e.type;
        GameObject spawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject spawnerInstance = Instantiate(spawner, new Vector2(0, 0), Quaternion.identity) as GameObject;
        if (type.Equals(SemanticType.INDIVIDUAL)) {
            GameObject firstRow = individualKeyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = individualKeyboard.transform.GetChild(1).gameObject;
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.QUANTIFIER)) {
            GameObject firstRow = quantifierKeyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = quantifierKeyboard.transform.GetChild(1).gameObject;
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.PREDICATE)) {
            GameObject firstRow = predicateKeyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                // Debug.Log("1st row's name is " + firstRow.name);
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = predicateKeyboard.transform.GetChild(1).gameObject;
                // Debug.Log("second row's name is " + secondRow.name);
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.RELATION_2)) {
            GameObject firstRow = relation2Keyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                // Debug.Log("1st row's name is " + firstRow.name);
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = relation2Keyboard.transform.GetChild(1).gameObject;
                // Debug.Log("second row's name is " + secondRow.name);
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.TRUTH_FUNCTION_1)) {
            GameObject firstRow = truthFunction1Keyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = truthFunction1Keyboard.transform.GetChild(1).gameObject;
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.TRUTH_FUNCTION_2)) {
            GameObject firstRow = truthFunction2Keyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = truthFunction2Keyboard.transform.GetChild(1).gameObject;
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        } else if (type.Equals(SemanticType.INDIVIDUAL_TRUTH_RELATION)) {
            GameObject firstRow = individualTruthRelationKeyboard.transform.GetChild(0).gameObject;
            if (firstRow.transform.childCount < PIECES_PER_ROW) {
                spawnerInstance.transform.SetParent(firstRow.transform);
            } else {
                GameObject secondRow = individualTruthRelationKeyboard.transform.GetChild(1).gameObject;
                spawnerInstance.transform.SetParent(secondRow.transform);
            }
        }
        ExpressionPieceSpawner spawnerScript = spawnerInstance.GetComponent<ExpressionPieceSpawner>();
        spawnerScript.SetUpSpawner(e, this);
    }

    /** Creates the keyboard (and sub-keyboards) from which the user can highClick on ExpressionPieceSpawners,
    * which will create ExpressionPieces in the workspace.
    */
    public void SetUpKeyboard() {
        // LOGIC/FUNCTION WORDS
        // truth functions
        SetUpSpawner(Expression.TRUE);
        SetUpSpawner(Expression.NOT);
        SetUpSpawner(Expression.AND);
        SetUpSpawner(Expression.OR);

        // variables and variable functions
        // SetUpSpawner(Expression.INDIVIDUAL_VARIABLE);
        // SetUpSpawner(Expression.NEXT_VARIABLE);

        // quantifiers
        SetUpSpawner(Expression.NO);
        // SetUpSpawner(Expression.SOME);
        // SetUpSpawner(Expression.TWO);
        // SetUpSpawner(Expression.THREE);
        SetUpSpawner(Expression.EVERY);

        // CONTENT WORDS   
        // proper names
        SetUpSpawner(Expression.BOB);
        SetUpSpawner(Expression.EVAN);
        SetUpSpawner(Expression.WAYSIDE_PARK);
        SetUpSpawner(Expression.THE_GREAT_DOOR);
        //SetUpSpawner(Expression.I);

        // predicates
        SetUpSpawner(Expression.BLACK);
        SetUpSpawner(Expression.RED);
        SetUpSpawner(Expression.GREEN);
        SetUpSpawner(Expression.BLUE);
        SetUpSpawner(Expression.YELLOW);
        SetUpSpawner(Expression.MAGENTA);
        SetUpSpawner(Expression.CYAN);
        SetUpSpawner(Expression.WHITE);
        SetUpSpawner(Expression.FOUNTAIN);
        SetUpSpawner(Expression.LAMP);
        SetUpSpawner(Expression.ACTIVE);
        SetUpSpawner(Expression.INACTIVE);
        SetUpSpawner(Expression.KING);
        SetUpSpawner(Expression.COW);
        SetUpSpawner(Expression.PERSON);
        SetUpSpawner(Expression.ANIMAL);
        SetUpSpawner(Expression.EXISTS);
        SetUpSpawner(Expression.OPEN);
        SetUpSpawner(Expression.CLOSED);

        // 2-place relations
        SetUpSpawner(Expression.IDENTITY);
        SetUpSpawner(Expression.CONTAINED_WITHIN);
        SetUpSpawner(Expression.HELP);
        SetUpSpawner(Expression.NEAR);

        // individual-truth relations
        SetUpSpawner(Expression.MAKE);

        // SetUpSpawner(Expression.OVERLAPS_WITH);
    }

    // updates the keyboard so that the tabToDisplayIndex-th tab is active,
    // and all other tabs become inactive
    public void SwitchKeyboardTab(int tabToDisplayIndex) {
        currentKeyboard.SetActive(false);
        if (tabToDisplayIndex == 0) { // e
            currentKeyboard = individualKeyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 1) { // (e -> t), (e -> t) -> t
            currentKeyboard = quantifierKeyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 2) { // e -> t
            currentKeyboard = predicateKeyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 3) { // e, e -> t
            currentKeyboard = relation2Keyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 4) { // t -> t
            currentKeyboard = truthFunction1Keyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 5) { // t, t -> t
            currentKeyboard = truthFunction2Keyboard;
            highClick.Play();
        } else if (tabToDisplayIndex == 6) { // e, t -> t
            currentKeyboard = individualTruthRelationKeyboard;
            highClick.Play();
        }
        currentKeyboard.SetActive(true);
    }

    // Called when user presses button to submit the selected expression.
    // This method takes the selected expression and unparents it from the canvas,
    // then hides canvas (keyboard + workspace)
    public void SubmitSelectedExpression() {
        if(selectedExpression != null) {
            GameObject screenCanvas = GameObject.Find("ScreenCanvas");// a Canvas always on screen; never hidden
            selectedExpression.gameObject.transform.SetParent(screenCanvas.transform);
            canvasInstance.SetActive(!canvasInstance.activeInHierarchy);

            inSpeakingMode = true;

            combineSuccess.Play();
        } else {
            // Debug.Log("no selected expression to submit!");
            failure.Play();
        }
    }

    // getter
    public bool InSpeakingMode() {
        return inSpeakingMode;
    }

    public void SetInSpeakingMode(bool speakMode) {
        inSpeakingMode = speakMode;
    }

    public ExpressionPiece GetSelectedExpression() {
        return selectedExpression;
    }

    public void HidePointer() {
        pointer.SetActive(false);
    }

    public void ShowPointer() {
        pointer.SetActive(true);
        float selectedPieceHeight = selectedExpression.GetHeightInUnits() * ExpressionPiece.PIXELS_PER_UNIT;
        pointer.transform.position = new Vector3(selectedExpression.transform.position.x,
                                                 selectedExpression.transform.position.y + selectedPieceHeight / 2 + 15);
    }
}
