using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;

public class GameController : MonoBehaviour {
    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject mergePointer;
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

    public ExpressionPieceSpawner selectedSpawner;
    public ExpressionPiece highlightedExpression;
    public ExpressionPiece selectedExpression;
    public ExpressionPiece usableExpression;

    public const int PIECES_PER_ROW = 12;

    public bool is2D;
    public GameObject log;
    public GameObject fakeCrown;

    public GameObject door;

    private bool isInKeyboard = true;
    private int wordIndex = -1;
    public int expressionIndex = 0;

    void Start() {
        HidePointer();
        HideMergePointer();
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

        if (Input.GetButtonDown("Menu")) {
            if (selectedExpression != null) {
                if (!is2D) {
                    selectedExpression.transform.SetParent(GameObject.Find("ScreenCanvas").transform);
                    selectedExpression.transform.position = new Vector3(Screen.width / 2, Screen.height / 2);
                    usableExpression = selectedExpression;
                    // highlightedExpression = null;
                }
                HidePointer();
                HideMergePointer();
                selectedExpression = null;
            }

            canvas.SetActive(!canvas.activeInHierarchy);
            if (fpc) {
                FirstPersonController fpcScript = fpc.GetComponent<FirstPersonController>();
                if (canvas.activeInHierarchy) {
                    fpcScript.enabled = false;
                    highClick.Play();
                    if (isInKeyboard) {
                        if (wordIndex == -1) {
                            SetWordIndex(0);    
                        }
                        ShowPointer(wordIndex);
                    } else {
                        if (expressionIndex == -1 && highlightedExpression != null) {
                            SetExpressionIndex(0);
                        } else {
                            ShowPointer(highlightedExpression);
                        }
                    }
                } else {
                    fpcScript.enabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    lowClick.Play();
                    HidePointer();
                    HideMergePointer();
                }
            }
        }

        // buttons when keyboard is out
        if (canvas.activeInHierarchy) {
            if (Input.GetButtonDown("TabForward")) {
                isInKeyboard = false;
                SetExpressionIndex(expressionIndex);
            }

            if (Input.GetButtonDown("TabBack")) {
                isInKeyboard = true;
            }

            if (Input.GetButtonDown("Right")) {
                if (isInKeyboard) {
                    SetWordIndex(wordIndex + 1);
                } else {
                    SetExpressionIndex(expressionIndex + 1);
                }
            }

            if (Input.GetButtonDown("Left")) {
                if (isInKeyboard) {
                    SetWordIndex(wordIndex - 1);
                } else {
                    SetExpressionIndex(expressionIndex - 1);
                }
            }

            if (Input.GetButtonDown("Submit")) {
                if (isInKeyboard) {
                    selectedSpawner.MakeNewExpressionPiece();
                } else {
                    if (selectedExpression == null) {
                        selectedExpression = highlightedExpression;
                        ShowMergePointer(selectedExpression);
                    } else {
                        ExpressionPiece functionPiece = highlightedExpression.parentExpressionPiece;
                        if (functionPiece.CombineWith(selectedExpression, highlightedExpression.index)) {
                            combineSuccess.Play();
                            SetExpressionIndex(expressionIndex - 1);
                        } else {
                            failure.Play();
                        }
                        selectedExpression = null;
                        HideMergePointer();
                    }
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

    public void SetWordIndex(int index) {
        wordIndex = index;

        GameObject keyboard = canvas.transform.GetChild(0).gameObject;
        GameObject row = keyboard.transform.GetChild(index / PIECES_PER_ROW).gameObject;
        ExpressionPieceSpawner spawner = row.GetComponentsInChildren<ExpressionPieceSpawner>()[index % PIECES_PER_ROW];

        selectedSpawner = spawner;

        ShowPointer(index);
    }

    public void SetExpressionIndex(int index) {
        expressionIndex = index;

        ExpressionPiece[] workspace = canvas.transform.GetChild(1).gameObject.GetComponentsInChildren<ExpressionPiece>();
        if (workspace.Length == 0) {
            index = -1;
        } else {
            highlightedExpression = workspace[index];
            ShowPointer(workspace[index]);
        }
    }

    public void HidePointer() {
        pointer.SetActive(false);
    }

    public void HideMergePointer() {
        mergePointer.SetActive(false);
    }

    // TODO change this to ExpressionPieceSpawner
    // once you can figure out the UI stuff
    public void ShowPointer(int index) {
        int x = index % PIECES_PER_ROW;
        int y = index / PIECES_PER_ROW;
        pointer.SetActive(true);
        pointer.transform.position = new Vector3((x * 40f) + 30f,
            canvas.transform.GetChild(0).gameObject.transform.GetChild(y).gameObject.transform.position.y + 40f);
    }

    public void ShowMergePointer(ExpressionPiece exprPiece) {
        mergePointer.SetActive(true);
        float pieceHeight = exprPiece.GetHeightInUnits() * ExpressionPiece.PIXELS_PER_UNIT;
        mergePointer.transform.position = new Vector3(exprPiece.transform.position.x,
                                                 exprPiece.transform.position.y + pieceHeight / 2 + 15);
    }

    public void ShowPointer(ExpressionPiece exprPiece) {
        pointer.SetActive(true);
        float pieceHeight = exprPiece.GetHeightInUnits() * ExpressionPiece.PIXELS_PER_UNIT;
        pointer.transform.position = new Vector3(exprPiece.transform.position.x,
                                                 exprPiece.transform.position.y + pieceHeight / 2 + 15);
    }
}
