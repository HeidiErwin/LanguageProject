using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;

public class NPC : Character {

    GameController controller;
    protected Model model;
    [SerializeField] GameObject currentInteractObject; // the object the NPC can currently interact with
    // protected HashSet<Expression> primitiveAbilities;
    public Expression name;

    // Use this for initialization
    protected void Start() {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        model = DefaultModel.Make();
        // primitiveAbilities = new HashSet<Expression>();
    }

    private IEnumerator Do(Expression e) {
        Expression goal = e.GetArg(0);

        List<Expression> actionSequence = model.Plan(goal);

        if (actionSequence == null) {
            this.controller.lowClick.Play();
            yield return StartCoroutine(ShowSpeechBubble(Expression.REFUSE));
            yield break;
        }

        this.controller.combineSuccess.Play();
        yield return ShowSpeechBubble(Expression.ACCEPT);

        // // UNCOMMENT BELOW TO PRINT OUT THE ACTION SEUQNECE
        // StringBuilder s = new StringBuilder();
        // foreach (Expression a in actionSequence) {
        //     s.Append(a);
        //     s.Append("; ");
        // }
        // Debug.Log(s.ToString());

        // TODO: make the next action in the sequence wait until the previous
        // action has been completed.
        foreach (Expression action in actionSequence) {

            if (!controller.is2D) {
                if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("Bob").transform.position;
                    // yield return StartCoroutine(GoTo("Bob"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB));
                    continue;
                }

                if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("Evan").transform.position;
                    // yield return StartCoroutine(GoTo("Evan"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB));
                    continue;
                }
            }

            // StopCoroutine(GoTo("Bob"));
            // StopCoroutine(GoTo("Evan"));
            // StopCoroutine(GoTo("DoorFront"));

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)))) {
                    yield return StartCoroutine(GoTo("Bob"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB));
                    continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN)))) {
                    yield return StartCoroutine(GoTo("Evan"));
                    // this.model.Add(new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB));
                    continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))))) {
                yield return StartCoroutine(GoTo("DoorFront"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.COW))))) {
                yield return StartCoroutine(GoTo("Cow"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.COW))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.PLAYER)))) {
                yield return StartCoroutine(GoTo("Player(Clone)"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.PLAYER))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.TREE))))) {
                yield return StartCoroutine(GoTo("tree"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.TREE))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.LOG))))) {
                yield return StartCoroutine(GoTo("log"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.LOG))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE,  new Phrase(Expression.POSSESS, new Phrase(Expression.THE, Expression.LOG), 1)))))) {
                yield return StartCoroutine(GoTo("Woodcutter"));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE,  new Phrase(Expression.POSSESS, new Phrase(Expression.THE, Expression.LOG), 1)))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.EXISTS, new Phrase(Expression.THE, Expression.LOG))))) {
                GameObject.Find("tree").SetActive(false);
                controller.log.SetActive(true);
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))))) {
                this.model.UpdateBelief(new Phrase(Expression.MAKE,
                    Expression.SELF,
                    new Phrase(Expression.WEAR,
                        Expression.SELF,
                        new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))));

                controller.fakeCrown.SetActive(true);
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.NOT, new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))))))) {
                this.model.UpdateBelief(new Phrase(Expression.MAKE,
                    Expression.SELF,
                    new Phrase(Expression.NOT,
                        new Phrase(Expression.WEAR,
                            Expression.SELF,
                            new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))))));

                controller.fakeCrown.SetActive(false);
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.POSSESS, Expression.PLAYER, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))))) {
                this.model.UpdateBelief(new Phrase(Expression.MAKE,
                    Expression.SELF,
                    new Phrase(Expression.NOT,
                        new Phrase(Expression.POSSESS,
                            Expression.SELF,
                            new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))))));
                
                this.model.UpdateBelief(new Phrase(Expression.MAKE,
                    Expression.SELF,
                    new Phrase(Expression.POSSESS,
                        Expression.PLAYER,
                        new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))));

                GameObject player = GameObject.Find("Player(Clone)");

                controller.fakeCrown.transform.SetParent(player.transform);
                controller.fakeCrown.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.25f);
                Player playerScript = player.GetComponent<Player>();
                playerScript.currentWearObject = controller.fakeCrown;
                if (controller.fakeCrown.activeSelf) {
                    playerScript.isWearing = true;
                }

                continue;
            }

            // The second "if" clauses are commented out b/c without coroutines, they aren't activated in time.
            // TODO Uncomment when coroutine stuff is sorted out.

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))))) {
                //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                this.controller.lowClick.Play();
                GameObject.Find("Door").GetComponent<Door>().Open();
                // this.model.Remove(new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));
                // ShowSpeechBubble(new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)));
                //     }
               continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))))) {
                //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                this.controller.lowClick.Play();
                GameObject.Find("Door").GetComponent<Door>().Close();
                // this.model.Remove(new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));
                // ShowSpeechBubble(new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)));
                //     }
               continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)))))) {
                this.controller.placeExpression.Play();
                // the below code works with fromScratch, to a degree
                yield return ShowSpeechBubble(new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

                // yield return ShowSpeechBubble("would");

                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(this.name, new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));
                // this.model.Remove(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));
                // this.model.Add(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

                // this.model.Remove(new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

                // ShowSpeechBubble(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));
                continue;
            }

            if (action.Equals(new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)))))) {
                this.controller.placeExpression.Play();
                yield return ShowSpeechBubble(new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));
                // yield return ShowSpeechBubble("would");
                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(this.name, new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));
                // this.model.Remove(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));
                // this.model.Add(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));

                // this.model.Remove(new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)));
                this.model.UpdateBelief(new Phrase(Expression.MAKE, Expression.SELF, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));

                // ShowSpeechBubble(new Phrase(Expression.DESIRE, Expression.EVAN, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));
                continue;
            }

            MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
            MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);

            IPattern assertionSchema =
                new ExpressionPattern(Expression.WOULD,
                    new ExpressionPattern(Expression.BELIEVE, xi0, xt0));

            List<Dictionary<MetaVariable, Expression>> assertionBinding = assertionSchema.GetBindings(action);

            if (assertionBinding != null) {
                Expression assertion = new Phrase(Expression.ASSERT, assertionBinding[0][xt0]);
                yield return ShowSpeechBubble(assertion);
                Expression recipient = assertionBinding[0][xi0];

                if (recipient.Equals(Expression.BOB)) {
                    GameObject.Find("Bob").GetComponent<NPC>().ReceiveExpression(this.name, assertion);
                }

                if (recipient.Equals(Expression.EVAN)) {
                    GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(this.name, assertion);
                }
            }

            IPattern commandSchema =
                new ExpressionPattern(Expression.WOULD,
                    new ExpressionPattern(Expression.INTEND, xi0, xt0));

            List<Dictionary<MetaVariable, Expression>> commandBinding = commandSchema.GetBindings(action);

            if (commandBinding != null) {
                Expression command = new Phrase(Expression.WOULD, commandBinding[0][xt0]);
                yield return ShowSpeechBubble(command);
                Expression recipient = commandBinding[0][xi0];

                if (recipient.Equals(Expression.BOB)) {
                    GameObject.Find("Bob").GetComponent<NPC>().ReceiveExpression(this.name, command);
                }

                if (recipient.Equals(Expression.EVAN)) {
                    GameObject.Find("Evan").GetComponent<NPC>().ReceiveExpression(this.name, command);
                }

                if (recipient.Equals(new Phrase(Expression.THE,  new Phrase(Expression.POSSESS, new Phrase(Expression.THE, Expression.LOG), 1)))) {
                    GameObject.Find("Woodcutter").GetComponent<NPC>().ReceiveExpression(this.name, command);
                }
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
            ExpressionPiece selectedExpr = controller.selectedExpression; 
            if (selectedExpr == null) {
                // Debug.Log("No selected expression to say to this NPC");
            } else {
                ReceiveExpression(Expression.PLAYER, selectedExpr.expression);
                Destroy(selectedExpr.gameObject);
                controller.HidePointer();
                // scontroller.SetInSpeakingMode(false);
            }
        }
    }

    public void ReceivePercept(params Expression[] percept) {
        if (this.model == null) {
            return;
        }

        foreach (Expression p in percept) {
            this.model.UpdateBelief(new Phrase(Expression.PERCEIVE, Expression.SELF, p));
        }
    }

    public void ReceiveExpression(Expression utterer, Expression utterance) {
        // Debug.Log(this.nameString + " is seeing '" + utterance + "'");
        if (this.model == null) {
            // Debug.Log("No associated model.");
            this.controller.placeExpression.Play();
            StartCoroutine(ShowSpeechBubble("questionMark"));
            return;
        }

        if (utterance.type.Equals(SemanticType.CONFORMITY_VALUE)) {
            if (name.Equals(new Phrase(Expression.THE,  new Phrase(Expression.POSSESS, new Phrase(Expression.THE, Expression.LOG), 1)))) {
                if (!model.Proves(new Phrase(Expression.KING, utterer))) {
                    StartCoroutine(ShowSpeechBubble(Expression.REFUSE));
                    return;
                }
            }
            // StopCoroutine("Do");
            StartCoroutine(Do(utterance));
            return;
        }

        if (utterance.type.Equals(SemanticType.TRUTH_VALUE)) {
            if (this.model.Proves(utterance)) {
                this.controller.combineSuccess.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(Expression.ASSERT, Expression.AFFIRM)));
            } else if (this.model.Proves(new Phrase(Expression.NOT, utterance))) {
                this.controller.failure.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(Expression.ASSERT, Expression.DENY)));
            } else {
                this.controller.lowClick.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(Expression.ASSERT, new Phrase(Expression.OR, Expression.AFFIRM, Expression.DENY))));
            }
            return;
        }

        if (utterance.type.Equals(SemanticType.ASSERTION)) {
            Expression content = utterance.GetArg(0);
            if (this.model.UpdateBelief(new Phrase(Expression.BELIEVE, utterer, content))) {
                this.controller.combineSuccess.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(Expression.ASSERT, Expression.AFFIRM)));
            } else {
                this.controller.failure.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(Expression.ASSERT, Expression.DENY)));
            }
            return;
        }
        
        // Debug.Log("Semantic Type of utterance is not sentence/truth value.");
        this.controller.placeExpression.Play();
        StartCoroutine(ShowSpeechBubble("query"));
        return;
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

    public IEnumerator ShowSpeechBubble(Expression expr) {
        GameObject exprPiece = Resources.Load("Piece") as GameObject;
        GameObject exprPieceInstance = Instantiate(exprPiece, new Vector2(0, 0), Quaternion.identity) as GameObject;
        exprPieceInstance.name = "LIONKING";
        ExpressionPiece exprPieceScript = exprPieceInstance.GetComponent<ExpressionPiece>();
        exprPieceScript.FromScratch(expr, new Vector3(0, 0, 0));
        exprPieceScript.transform.SetParent(GameObject.Find("ResponseCanvas").transform);

        Camera cam;
        if (controller.is2D) {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        } else {
            cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
        }
        exprPieceScript.transform.position = cam.WorldToScreenPoint(this.transform.position);
        exprPieceScript.transform.position =
                new Vector3(exprPieceScript.transform.position.x, exprPieceScript.transform.position.y + (exprPieceScript.heightInUnits * ExpressionPiece.PIXELS_PER_UNIT / 2) + 16);
        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());
        Destroy(exprPieceInstance, 2.0f);
        yield return new WaitForSeconds(2.0f);
    }

    public IEnumerator GoTo(String targetID) {
        GameObject targetObject = GameObject.Find(targetID);
        if (targetObject) {
            target = targetObject.transform;
            speed = 2;
            GoToTarget();
            while (!walkingComplete) {
                yield return null;
            }
        }
        yield break;
    }

    // called when Character enters the trigger collider of an object 
    public void OnTriggerEnter2D(Collider2D other) {
        Perceivable po = other.GetComponent<Perceivable>();
        
        if (po != null) {
            po.SendPercept(this);
        }

        Perceivable[] childrenPOs = GetComponentsInChildren<Perceivable>();
        for (int i = 0; i < childrenPOs.Length; i++) {
            Debug.Log(childrenPOs[i]);
            childrenPOs[i].SendPercept(this);
        }

        if (other.CompareTag("Interactable")) {
            currentInteractObject = other.gameObject;
        }
    }

    // NOTE: objects are perceived both when NPC enters and exits their range of perceptability
    // public void OnTriggerExit2D(Collider2D other) {
    //     if (other.GetComponent<Perceivable>() != null) {
    //     }
    // }
}
