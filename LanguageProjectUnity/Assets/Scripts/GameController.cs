using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/**
 * The main Controller class. Sets up the Scene, creates ExpressionPieces, etc.
 */
public class GameController : MonoBehaviour {
    // the subsection of the keyboard we're currently showing (e.g. Determiners)
    private GameObject currentKeyboard;
    public GameObject canvasInstance;
    // arrow pointing to selected expression
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject individualKeyboard;
    [SerializeField] private GameObject quantifierKeyboard;
    [SerializeField] private GameObject predicateKeyboard;
    [SerializeField] private GameObject relation2Keyboard;
    [SerializeField] private GameObject truthFunction1Keyboard;
    [SerializeField] private GameObject truthFunction2Keyboard;
    [SerializeField] private GameObject individualTruthRelationKeyboard;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private String wordsPath;
    private bool keyboardOnBeforeHelpShown = true;

    public AudioSource highClick;
    public AudioSource lowClick;
    public AudioSource combineSuccess;
    public AudioSource placeExpression;
    public AudioSource failure;

    public const int PIECES_PER_ROW = 14;

    // true only right after user has submitted an expression (pressed checkmark button)
    // and before user has selected an NPC to speak to
    private bool inSpeakingMode = false;

    public ExpressionPiece selectedExpression;

	void Start() {
        // Debug.Log(Screen.height/20.0f + " is the screen height");
        SetUpCanvas();
        SetUpKeyboard();
        SetUpPlayer();
        currentKeyboard = individualTruthRelationKeyboard;
        canvasInstance.SetActive(false);
        currentKeyboard.SetActive(true);

        // // FOR TESTING PURPOSES, @TODO COMMENT OUT LATER
        // GameObject workspace = GameObject.Find("Workspace");
        // GameObject exprPiece = Resources.Load("Piece") as GameObject;
        // GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        // exprPieceInstance.transform.SetParent(workspace.transform);
        // ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        // exprPieceScript.FromScratch(
        //     new Phrase(Expression.WOULD,
        //         new Phrase(Expression.NOT,
        //             new Phrase(Expression.CLOSED,
        //                 new Phrase(Expression.THE, Expression.DOOR)))), new Vector3(0f, 0f, 0f));
        // exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());
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

        GameObject firstRow = individualTruthRelationKeyboard.transform.GetChild(0).gameObject;
        GameObject secondRow = individualTruthRelationKeyboard.transform.GetChild(1).gameObject;
        GameObject thirdRow = individualTruthRelationKeyboard.transform.GetChild(2).gameObject;
        if (firstRow.transform.childCount < PIECES_PER_ROW) {
            spawnerInstance.transform.SetParent(firstRow.transform);
        } else if (secondRow.transform.childCount < PIECES_PER_ROW) {
            spawnerInstance.transform.SetParent(secondRow.transform);
        } else {
            spawnerInstance.transform.SetParent(thirdRow.transform);
        }

        ExpressionPieceSpawner spawnerScript = spawnerInstance.GetComponent<ExpressionPieceSpawner>();
        spawnerScript.SetUpSpawner(e, this);
    }

    /** Creates the keyboard (and sub-keyboards) from which the user can highClick on ExpressionPieceSpawners,
    * which will create ExpressionPieces in the workspace.
    */
    public void SetUpKeyboard() {
        StreamReader reader = new StreamReader("Assets/Data/Words/" + wordsPath + ".lexicon");
        String line = reader.ReadLine();

        SemanticType currentType = SemanticType.INDIVIDUAL;
        while (line != null) {
            line = line.Trim();
            if (!(line.StartsWith("//") || line.Equals(""))) {
                if (line.Equals("#INDIVIDUAL")) {
                    currentType = SemanticType.INDIVIDUAL;
                } else if (line.Equals("#PREDICATE")) {
                    currentType = SemanticType.PREDICATE;
                } else if (line.Equals("#RELATION_2")) {
                    currentType = SemanticType.RELATION_2;
                } else if (line.Equals("#DETERMINER")) {
                    currentType = SemanticType.DETERMINER;
                } else if (line.Equals("#QUANTIFIER")) {
                    currentType = SemanticType.QUANTIFIER;
                } else if (line.Equals("#TRUTH_FUNCTION_1")) {
                    currentType = SemanticType.TRUTH_FUNCTION_1;
                } else if (line.Equals("#TRUTH_FUNCTION_2")) {
                    currentType = SemanticType.TRUTH_FUNCTION_2;
                } else if (line.Equals("#TRUTH_CONFORMITY_FUNCTION")) {
                    currentType = SemanticType.TRUTH_CONFORMITY_FUNCTION;
                } else if (line.Equals("#TRUTH_ASSERTION_FUNCTION")) {
                    currentType = SemanticType.TRUTH_ASSERTION_FUNCTION;
                } else if (line.Equals("#INDIVIDUAL_TRUTH_RELATION")) {
                    currentType = SemanticType.INDIVIDUAL_TRUTH_RELATION;
                } else {
                    SetUpSpawner(new Word(currentType, line));
                }
            }
            line = reader.ReadLine();
        }
    }

    // // updates the keyboard so that the tabToDisplayIndex-th tab is active,
    // // and all other tabs become inactive
    // public void SwitchKeyboardTab(int tabToDisplayIndex) {
    //     currentKeyboard.SetActive(false);
    //     if (tabToDisplayIndex == 0) { // e
    //         currentKeyboard = individualKeyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 1) { // (e -> t), (e -> t) -> t
    //         currentKeyboard = quantifierKeyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 2) { // e -> t
    //         currentKeyboard = predicateKeyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 3) { // e, e -> t
    //         currentKeyboard = relation2Keyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 4) { // t -> t
    //         currentKeyboard = truthFunction1Keyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 5) { // t, t -> t
    //         currentKeyboard = truthFunction2Keyboard;
    //         highClick.Play();
    //     } else if (tabToDisplayIndex == 6) { // e, t -> t
    //         currentKeyboard = individualTruthRelationKeyboard;
    //         highClick.Play();
    //     }
    //     currentKeyboard.SetActive(true);
    // }

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
