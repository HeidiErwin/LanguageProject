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

        this.controller.combineSuccess.Play();
        yield return ShowSpeechBubble("yes");

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
            // StopCoroutine(GoTo("Bob"));
            // StopCoroutine(GoTo("Evan"));
            // StopCoroutine(GoTo("DoorFront"));

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF,
                            isBob ? Expression.EVAN : Expression.BOB)))) {
                if (isBob) {
                    yield return StartCoroutine(GoTo("Evan"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.BOB, Expression.EVAN));
                } else {
                    yield return StartCoroutine(GoTo("Bob"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB));
                }
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.THE_GREAT_DOOR)))) {
                yield return StartCoroutine(GoTo("DoorFront"));
                this.model.Add(new Phrase(Expression.NEAR, Expression.SELF, Expression.THE_GREAT_DOOR));
            }

            // The second "if" clauses are commented out b/c without coroutines, they aren't activated in time.
            // TODO Uncomment when coroutine stuff is sorted out.

            if (!isBob && action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)))) {
            //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                    this.controller.lowClick.Play();
                    GameObject.Find("Door").GetComponent<Door>().Open();
                    this.model.Remove(new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR));
                    this.model.Add(new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR));
            //     }
            }

            if (!isBob && action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)))) {
            //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                    this.controller.lowClick.Play();
                    GameObject.Find("Door").GetComponent<Door>().Close();
                    this.model.Remove(new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR));
                    this.model.Add(new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR));
            //     }
            }

            if (isBob && action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR))))) {
                this.controller.placeExpression.Play();
                yield return ShowSpeechBubble("would");
                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));
                // this.model.Remove(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)));
                // this.model.Add(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));

                this.model.Remove(new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR));
                this.model.Add(new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR));
            }

            if (isBob && action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR))))) {
                this.controller.placeExpression.Play();
                yield return ShowSpeechBubble("would");
                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)));
                // this.model.Remove(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));
                // this.model.Add(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)));

                this.model.Remove(new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR));
                this.model.Add(new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR));
            }
        }
        // this.controller.combineSuccess.Play();
        // yield return ShowSpeechBubble("yes");
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

    public void ReceivePercept(params Expression[] report) {
        if (this.model == null) {
            return;
        }

        foreach (Expression p in report) {
            this.model.Add(p);
        }
    }

    void ReceiveExpression(Expression utterance) {
        // Debug.Log(this.nameString + " is seeing '" + utterance + "'");
        if (utterance.type.Equals(SemanticType.CONFORMITY_VALUE)) {
            // StopCoroutine("Do");
            StartCoroutine(Do(utterance));
            return;
        }

        if (this.model == null) {
            // Debug.Log("No associated model.");
            this.controller.placeExpression.Play();
            StartCoroutine(ShowSpeechBubble("questionMark"));
            
            return;
        }

        if (!utterance.type.Equals(SemanticType.TRUTH_VALUE)) {
            // Debug.Log("Semantic Type of utterance is not sentence/truth value.");
            this.controller.placeExpression.Play();
            StartCoroutine(ShowSpeechBubble("questionMark"));
             // TODO make a unique sound for this
            return;
        }

        if (this.model.Proves(utterance)) {
            this.controller.combineSuccess.Play();
            StartCoroutine(ShowSpeechBubble("yes"));
             // TODO make a unique sound effect for this
        } else if (this.model.Proves(new Phrase(Expression.NOT, utterance))) {
            this.controller.failure.Play();
            StartCoroutine(ShowSpeechBubble("nope"));
            
        } else {
            this.controller.lowClick.Play();
            StartCoroutine(ShowSpeechBubble("idk"));
            
        }
    }

    /**
     * imageName is the image to display in the speechbubble
     */
    public IEnumerator ShowSpeechBubble(string imageName) {
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
        yield return new WaitForSeconds(2.0f);
    }

    public IEnumerator GoTo(String targetID) {
        GameObject targetObject = GameObject.Find(targetID);
        target = targetObject.transform;
        speed = 2;
        GoToTarget();

        while (!walkingComplete) {
            yield return null;
        }

        yield break;
    }

    // called when Character enters the trigger collider of an object 
    public void OnTriggerEnter2D(Collider2D other) {
        Perceivable po = other.GetComponent<Perceivable>();
        if (po != null) {
            po.SendPercept(this);
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
