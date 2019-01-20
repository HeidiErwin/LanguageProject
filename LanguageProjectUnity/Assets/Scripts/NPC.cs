using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPC : Character {

    GameController controller;
    [SerializeField] protected String nameString;
    private Model model;
    [SerializeField] GameObject currentInteractObject; // the object the NPC can currently interact with
    private EnvironmentManager envManager;

    // Use this for initialization
    void Start() {
        envManager = GameObject.Find("EnvironmentManager").GetComponent<EnvironmentManager>();
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        if (nameString.Equals("Bob")) {
            model = CustomModels.BobModel();
        }

        if (nameString.Equals("Evan")) {
            model = CustomModels.EvanModel();
        }
    }

    private IEnumerator Do(Expression e) {
        Expression goal = e.GetArg(0);

        List<Expression> actionSequence = model.Plan(goal, new List<Expression>());

        if (actionSequence == null) {
            ShowSpeechBubble("idk");
            this.controller.lowClick.Play();
            yield return false;
        }

        // UNCOMMENT BELOW TO PRINT OUT THE ACTION SEUQNECE
        // StringBuilder s = new StringBuilder();
        // foreach (Expression a in actionSequence) {
        //     s.Append(a);
        //     s.Append("; ");
        // }

        // Debug.Log(s.ToString());

        bool isBob = nameString.Equals("Bob");

        // TODO: make the next action in the sequence wait until the previous
        // action has been completed.
        foreach (Expression action in actionSequence) {

            if (action.Equals(new Phrase(Expression.WOULD,
                new Phrase(Expression.NEAR, isBob ? Expression.BOB : Expression.EVAN,
                            isBob ? Expression.EVAN : Expression.BOB)))) {
                if (isBob) {
                    GoTo("Evan");
                } else {
                    GoTo("Bob");
                }
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR,
                isBob ? Expression.BOB : Expression.EVAN, Expression.THE_GREAT_DOOR)))) {
                GoTo("DoorFront");
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)))) {
                if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                    GameObject.Find("Door").GetComponent<Door>().Open();
                }
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)))) {
                if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                    GameObject.Find("Door").GetComponent<Door>().Close();
                }
            }

            if (isBob && action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR))))) {
                GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));
            }
        }

        this.controller.combineSuccess.Play();
        yield return true;
    }

    /**
     * if the game is in Speaking Mode (kept track of by a bool in GameController,
     * and this NPC is clicked, say the expression to the NPC.
     */
    private void OnMouseDown() {
        if (!EventSystem.current.IsPointerOverGameObject()) { // make sure not clicking canvas
            ExpressionPiece selectedExpr = controller.GetSelectedExpression(); 
            if (selectedExpr == null) {
                // Debug.Log("No selected expression to say to this NPC");
            } else {
                ReceiveExpression(selectedExpr.expression);
                Destroy(selectedExpr.gameObject);
                controller.HidePointer();
                controller.SetInSpeakingMode(false);
            }
        }
    }

    public void ReceivePerceptualReport(params Expression[] report) {
        if (this.model == null) {
            return;
        }

        foreach (Expression p in report) {
            this.model.Add(p);
        }
    }

    void ReceiveExpression(Expression utterance) {
        // Debug.Log(this.nameString + " is seeing '" + utterance + "'");
        if (utterance.headString.Equals("would")) {
            StartCoroutine(Do(utterance));
            return;
        }

        if (this.model == null) {
            // Debug.Log("No associated model.");
            ShowSpeechBubble("questionMark");
            this.controller.placeExpression.Play();
            return;
        }

        if (!utterance.type.Equals(SemanticType.TRUTH_VALUE)) {
            // Debug.Log("Semantic Type of utterance is not sentence/truth value.");
            ShowSpeechBubble("questionMark");
            this.controller.placeExpression.Play(); // TODO make a unique sound for this
            return;
        }

        if (this.model.Proves(utterance)) {
            ShowSpeechBubble("yes");
            this.controller.combineSuccess.Play(); // TODO make a unique sound effect for this
        } else if (this.model.Proves(new Phrase(Expression.NOT, utterance))) {
            ShowSpeechBubble("nope");
            this.controller.failure.Play();
        } else {
            ShowSpeechBubble("idk");
            this.controller.lowClick.Play();
        }
    }

    /**
     * imageName is the image to display in the speechbubble
     */
    public void ShowSpeechBubble(string imageName) {
        GameObject screenCanvas = GameObject.Find("ScreenCanvas");
        GameObject response = new GameObject();
        response.name = "Response";
        response.transform.SetParent(screenCanvas.transform);
        response.transform.position = new Vector3(Camera.main.WorldToScreenPoint(this.transform.position).x, Camera.main.WorldToScreenPoint(this.transform.position).y + 35);
        response.layer = 5;
        Image responseImage = response.AddComponent<Image>();
        Sprite headSprite = Resources.Load<Sprite>("PlaceholderSprites/" + imageName);
        responseImage.sprite = headSprite;
        responseImage.transform.localScale *= .25f;
        Destroy(response, 2.0f);

    }

    public void GoTo(String targetID) {
        GameObject targetObject = GameObject.Find(targetID);
        target = targetObject.transform;
        speed = 2;
        GoToTarget();
    }

    // called when Character enters the trigger collider of an object 
    public void OnTriggerEnter2D(Collider2D other) {
        Perceivable po = other.GetComponent<Perceivable>();
        if (po != null) {
            po.SendPerceptualReport(this);
        }
        if (other.CompareTag("Interactable")) {
            currentInteractObject = other.gameObject;
        }
    }

    // NOTE: objects are perceived both when NPC enters and exits their range of perceptability
    // public void OnTriggerExit2D(Collider2D other) {
    //     if (other.GetComponent<Perceivable>() != null) {
    //         envManager.ComputePerceptionalReport(this);
    //     }
    // }
}
