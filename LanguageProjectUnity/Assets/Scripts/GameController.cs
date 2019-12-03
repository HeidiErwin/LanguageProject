using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject canvas;
    [SerializeField] private String wordsPath;
    private GameObject fpc;

    public AudioSource highClick;
    public AudioSource lowClick;
    public AudioSource combineSuccess;
    public AudioSource placeExpression;
    public AudioSource failure;

    public GameObject currentInteractObject;
    public GameObject currentUseObject;  

    public ExpressionPiece selectedExpression;
    public ExpressionPiece usableExpression;

    public const int PIECES_PER_ROW = 12;

    public bool is2D;
    public GameObject log;
    public GameObject fakeCrown;

    public GameObject door;

    void Start() {
        HidePointer();
        fpc = GameObject.Find("FPSController");
        if (is2D) {
            fakeCrown.SetActive(false);
            GameObject player = Resources.Load("Player") as GameObject;
            GameObject playerInstance = Instantiate(player, new Vector2(0f, 0f), Quaternion.identity);
        }
        SetUpKeyboard();
        canvas.SetActive(false);

        // Assembly asm = typeof(GameController).Assembly;
        // Debug.Log(asm.GetType("Expression1"));
    }

    void Update() {
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Interact")) {
            if (currentUseObject != null) {
                currentUseObject.transform.parent = null;
                currentUseObject = null;                
            }

        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (currentInteractObject != null) {
                GameObject thatInventory = GameObject.Find(currentInteractObject.name + "/Inventory");
                GameObject playerInventory = GameObject.Find("FPSController/Inventory");
                if (thatInventory != null && playerInventory.transform.childCount > 0) {
                    Transform item = playerInventory.transform.GetChild(0);
                    item.transform.SetParent(thatInventory.transform);
                    item.transform.position = thatInventory.transform.position;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab) || Input.GetButtonDown("Submit")) {
            if (selectedExpression != null) {
                if (!is2D) {
                    selectedExpression.transform.SetParent(GameObject.Find("ScreenCanvas").transform);
                    selectedExpression.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
                    usableExpression = selectedExpression;
                    // selectedExpression = null;
                }
                HidePointer();
                selectedExpression = null;
            }

            canvas.SetActive(!canvas.activeInHierarchy);
            if (fpc) {
                FirstPersonController fpcScript = fpc.GetComponent<FirstPersonController>();
                if (canvas.activeInHierarchy) {
                    fpcScript.enabled = false;
                    highClick.Play();
                } else {
                    fpcScript.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    lowClick.Play();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void SetUpSpawner(Expression e) {
        SemanticType type = e.type;
        GameObject spawner = Resources.Load("PieceSpawner") as GameObject;
        GameObject spawnerInstance = Instantiate(spawner, new Vector2(0, 0), Quaternion.identity) as GameObject;

        GameObject keyboard = canvas.transform.GetChild(0).gameObject;

        GameObject firstRow = keyboard.transform.GetChild(0).gameObject;
        GameObject secondRow = keyboard.transform.GetChild(1).gameObject;
        GameObject thirdRow = keyboard.transform.GetChild(2).gameObject;
        if (firstRow.transform.childCount < PIECES_PER_ROW) {
            spawnerInstance.transform.SetParent(firstRow.transform);
        } else if (secondRow.transform.childCount < PIECES_PER_ROW) {
            spawnerInstance.transform.SetParent(secondRow.transform);
        } else {
            spawnerInstance.transform.SetParent(thirdRow.transform);
        }

        ExpressionPieceSpawner spawnerScript = spawnerInstance.GetComponent<ExpressionPieceSpawner>();
        spawnerScript.SetUpSpawner(e);
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
                } else if (line.Equals("#INDIVIDUAL_FUNCTION_1")) {
                    currentType = SemanticType.INDIVIDUAL_FUNCTION_1;
                } else if (line.Equals("#INDIVIDUAL_FUNCTION_2")) {
                    currentType = SemanticType.INDIVIDUAL_FUNCTION_2;
                } else if (line.Equals("#PREDICATE")) {
                    currentType = SemanticType.PREDICATE;
                } else if (line.Equals("#PREDICATE_MODIFIER")) {
                    currentType = SemanticType.PREDICATE_MODIFIER;
                } else if (line.Equals("#PREDICATE_MODIFIER_2")) {
                    currentType = SemanticType.PREDICATE_MODIFIER_2;
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
                } else if (line.Equals("#TRUTH_CONFORMITY_FUNCTION_2")) {
                    currentType = SemanticType.TRUTH_CONFORMITY_FUNCTION_2;
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

        // // FOR TEST PURPOSES
        // GameObject workspace = GameObject.Find("Workspace");
        // GameObject exprPiece = Resources.Load("Piece") as GameObject;
        // GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        // exprPieceInstance.transform.SetParent(workspace.transform);
        // ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        // exprPieceScript.FromScratch(new Phrase(Expression.CONTRACT,
        //     new Phrase(Expression.POSSESS, Expression.SELF, Expression.EMERALD),
        //     new Phrase(Expression.AT, Expression.SELF, Expression.GOAL)), new Vector3(0f, 0f, 0f));
        // exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());
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
