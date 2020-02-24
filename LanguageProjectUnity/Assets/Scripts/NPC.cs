using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.AI;
using static Expression;

public class NPC : Character {
    GameController controller;
    protected Model model;
    [SerializeField] GameObject currentInteractObject; // the object the NPC can currently interact with
    public Expression name;
    protected bool locked = false;
    protected bool actionInProgress = false;

    // Use this for initialization
    protected void Start() {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        model = DefaultModel.Make();
        // primitiveAbilities = new HashSet<Expression>();
    }

    protected void Update() {
        if (!actionInProgress) {
            if (model.currentPlan == null && !model.decisionLock) {
                model.DecideGoal();
            }
            StartCoroutine(Do(model.currentPlan));
        }
    }

    protected IEnumerator Do(Expression e) {
        Expression goal = e.GetArg(0);

        List<Expression> actionSequence = model.Plan(goal);
        yield return Do(actionSequence);
    }

    protected IEnumerator Do(List<Expression> actionSequence) {
        actionInProgress = true;
        if (actionSequence == null) {

            actionInProgress = false;
            yield break;
        }

        // // UNCOMMENT BELOW TO PRINT OUT THE ACTION SEUQNECE
        // StringBuilder s = new StringBuilder();
        // foreach (Expression a in actionSequence) {
        //     s.Append(a);
        //     s.Append("; ");
        // }
        // Debug.Log(s.ToString());

        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xi1 = new MetaVariable(SemanticType.INDIVIDUAL, 1);

        // TODO: make the next action in the sequence wait until the previous
        // action has been completed.
        foreach (Expression action in actionSequence) {
            if (!controller.is2D) {
                if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, BOB)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("bob").transform.position;

                    yield return null;
                    while (GetComponent<NavMeshAgent>().remainingDistance > 1) {
                        yield return null;
                    }
                    this.model.UpdateBelief(
                        new Phrase(MAKE, SELF,
                            new Phrase(AT, SELF, BOB)));

                    continue;
                }

                if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, EVAN)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("evan").transform.position;

                    yield return null;
                    while (GetComponent<NavMeshAgent>().remainingDistance > 1) {
                        yield return null;
                    }

                    this.model.UpdateBelief(
                        new Phrase(MAKE, SELF,
                            new Phrase(AT, SELF, EVAN)));

                    continue;
                }

                if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, GOAL)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("Prize").transform.position;

                    yield return null;
                    while (GetComponent<NavMeshAgent>().remainingDistance > 1) {
                        yield return null;
                    }

                    this.model.UpdateBelief(
                        new Phrase(MAKE, SELF,
                            new Phrase(AT, SELF, GOAL)));

                    continue;
                }

                if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, DOOR)))) {
                    GetComponent<NavMeshAgent>().destination = GameObject.Find("Door").transform.position;
                    
                    yield return null;
                    while (GetComponent<NavMeshAgent>().remainingDistance > 1) {
                        yield return null;
                    }

                    continue;
                }

                IPattern locationPattern = new ExpressionPattern(WOULD, new ExpressionPattern(AT, SELF, new ExpressionPattern(LOCATION, xi0, xi1)));
                List<Dictionary<MetaVariable, Expression>> locationBindings = locationPattern.GetBindings(action);
                if (locationBindings != null) {
                    Expression xCoord = locationBindings[0][xi0];
                    Expression yCoord = locationBindings[0][xi1];

                    float xCoordUnity = -1f;
                    float yCoordUnity = -1f;

                    if (xCoord.Equals(ZERO)) {
                        xCoordUnity = 0f;
                    }

                    if (xCoord.Equals(ONE)) {
                        xCoordUnity = 1f;
                    }

                    if (xCoord.Equals(TWO)) {
                        xCoordUnity = 2f;
                    }

                    if (xCoord.Equals(THREE)) {
                        xCoordUnity = 3f;
                    }

                    if (xCoord.Equals(THREE)) {
                        xCoordUnity = 4f;
                    }

                    if (xCoord.Equals(FIVE)) {
                        xCoordUnity = 5f;
                    }
                    if (xCoord.Equals(ZERO)) {
                        xCoordUnity = 0f;
                    }

                    if (yCoord.Equals(ONE)) {
                        yCoordUnity = 1f;
                    }

                    if (yCoord.Equals(TWO)) {
                        yCoordUnity = 2f;
                    }

                    if (yCoord.Equals(THREE)) {
                        yCoordUnity = 3f;
                    }

                    if (yCoord.Equals(THREE)) {
                        yCoordUnity = 4f;
                    }

                    if (yCoord.Equals(FIVE)) {
                        yCoordUnity = 5f;
                    }

                    xCoordUnity *= 3f;
                    xCoordUnity += 1.3f;

                    yCoordUnity *= 2f;
                    yCoordUnity += -5f;

                    GetComponent<NavMeshAgent>().destination = new Vector3(xCoordUnity, 0f, yCoordUnity);

                    yield return null;
                    while (GetComponent<NavMeshAgent>().remainingDistance > 1) {
                        yield return null;
                    }

                    this.model.UpdateBelief(new Phrase(MAKE, SELF, action.GetArg(0)));

                    continue;
                }

                if (action.Equals(new Phrase(WOULD, new Phrase(OPEN, DOOR)))) {
                    controller.door.SetActive(false);
                    continue;
                }

                if (action.Equals(new Phrase(WOULD, new Phrase(CLOSED, DOOR)))) {
                    controller.door.SetActive(true);
                    continue;
                }

                if (action.Equals(new Phrase(WOULD,
                    new Phrase(NOT, new Phrase(POSSESS, SELF, RUBY))))) {
                    // Debug.Log("GIVE UP RUBY!!!");
                    continue;
                }
            }

            // StopCoroutine(GoTo("bob"));
            // StopCoroutine(GoTo("evan"));
            // StopCoroutine(GoTo("DoorFront"));

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, BOB)))) {
                    yield return StartCoroutine(GoTo("bob"));
                    // this.model.Add(new Phrase(AT, EVAN, BOB));
                    continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, EVAN)))) {
                    yield return StartCoroutine(GoTo("evan"));
                    // this.model.Add(new Phrase(AT, EVAN, BOB));
                    continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, DOOR)))) {
                yield return StartCoroutine(GoTo("DoorFront"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, DOOR)));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, new Phrase(THE, COW))))) {
                yield return StartCoroutine(GoTo("Cow"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, new Phrase(THE, COW))));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, PLAYER)))) {
                yield return StartCoroutine(GoTo("Player(Clone)"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, new Phrase(PLAYER))));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, new Phrase(THE, TREE))))) {
                yield return StartCoroutine(GoTo("tree"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, new Phrase(THE, TREE))));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, new Phrase(THE, LOG))))) {
                yield return StartCoroutine(GoTo("log"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, new Phrase(THE, LOG))));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(AT, SELF, new Phrase(THE,  new Phrase(POSSESS, new Phrase(THE, LOG), 1)))))) {
                yield return StartCoroutine(GoTo("Woodcutter"));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(AT, SELF, new Phrase(THE,  new Phrase(POSSESS, new Phrase(THE, LOG), 1)))));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(EXISTS, new Phrase(THE, LOG))))) {
                GameObject.Find("tree").SetActive(false);
                controller.log.SetActive(true);
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(WEAR, SELF, new Phrase(THE, new Phrase(FAKE, CROWN)))))) {
                this.model.UpdateBelief(new Phrase(MAKE,
                    SELF,
                    new Phrase(WEAR,
                        SELF,
                        new Phrase(THE, new Phrase(FAKE, CROWN)))));

                controller.fakeCrown.SetActive(true);
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(NOT, new Phrase(WEAR, SELF, new Phrase(THE, new Phrase(FAKE, CROWN))))))) {
                this.model.UpdateBelief(new Phrase(MAKE,
                    SELF,
                    new Phrase(NOT,
                        new Phrase(WEAR,
                            SELF,
                            new Phrase(THE, new Phrase(FAKE, CROWN))))));

                controller.fakeCrown.SetActive(false);
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(POSSESS, PLAYER, new Phrase(THE, new Phrase(FAKE, CROWN)))))) {
                this.model.UpdateBelief(new Phrase(MAKE,
                    SELF,
                    new Phrase(NOT,
                        new Phrase(POSSESS,
                            SELF,
                            new Phrase(THE, new Phrase(FAKE, CROWN))))));
                
                this.model.UpdateBelief(new Phrase(MAKE,
                    SELF,
                    new Phrase(POSSESS,
                        PLAYER,
                        new Phrase(THE, new Phrase(FAKE, CROWN)))));

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

            if (action.Equals(new Phrase(WOULD, new Phrase(OPEN, DOOR)))) {
                //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                this.controller.lowClick.Play();
                GameObject.Find("Door").GetComponent<Door>().Open();
                // this.model.Remove(new Phrase(CLOSED, DOOR));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(OPEN, DOOR)));
                // ShowSpeechBubble(new Phrase(OPEN, DOOR));
                //     }
               continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(CLOSED, DOOR)))) {
                //     if (currentInteractObject != null && currentInteractObject.name.Equals("DoorFront")) {
                this.controller.lowClick.Play();
                GameObject.Find("Door").GetComponent<Door>().Close();
                // this.model.Remove(new Phrase(OPEN, DOOR));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(CLOSED, DOOR)));
                // ShowSpeechBubble(new Phrase(CLOSED, DOOR));
                //     }
               continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(INTEND, EVAN, new Phrase(OPEN, DOOR))))) {
                this.controller.placeExpression.Play();
                // the below code works with fromScratch, to a degree
                yield return ShowSpeechBubble(new Phrase(WOULD, new Phrase(OPEN, DOOR)));

                // yield return ShowSpeechBubble("would");

                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("evan").GetComponent<NPC>().ReceiveExpression(this.name, new Phrase(WOULD, new Phrase(OPEN, DOOR)));
                // this.model.Remove(new Phrase(INTEND, EVAN, new Phrase(CLOSED, DOOR)));
                // this.model.Add(new Phrase(INTEND, EVAN, new Phrase(OPEN, DOOR)));

                // this.model.Remove(new Phrase(CLOSED, DOOR));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(OPEN, DOOR)));

                // ShowSpeechBubble(new Phrase(INTEND, EVAN, new Phrase(OPEN, DOOR)));
                continue;
            }

            if (action.Equals(new Phrase(WOULD, new Phrase(INTEND, EVAN, new Phrase(CLOSED, DOOR))))) {
                this.controller.placeExpression.Play();
                yield return ShowSpeechBubble(new Phrase(WOULD, new Phrase(CLOSED, DOOR)));
                // yield return ShowSpeechBubble("would");
                // yield return new WaitForSeconds(2.0f);
                GameObject.Find("evan").GetComponent<NPC>().ReceiveExpression(this.name, new Phrase(WOULD, new Phrase(CLOSED, DOOR)));
                // this.model.Remove(new Phrase(INTEND, EVAN, new Phrase(OPEN, DOOR)));
                // this.model.Add(new Phrase(INTEND, EVAN, new Phrase(CLOSED, DOOR)));

                // this.model.Remove(new Phrase(OPEN, DOOR));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(CLOSED, DOOR)));

                // ShowSpeechBubble(new Phrase(INTEND, EVAN, new Phrase(CLOSED, DOOR)));
                continue;
            }

            MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);

            IPattern assertionSchema =
                new ExpressionPattern(WOULD,
                    new ExpressionPattern(BELIEVE, xi0, xt0));

            List<Dictionary<MetaVariable, Expression>> assertionBinding = assertionSchema.GetBindings(action);

            if (assertionBinding != null) {
                Expression assertion = new Phrase(ASSERT, assertionBinding[0][xt0]);
                yield return ShowSpeechBubble(assertion);
                Expression recipient = assertionBinding[0][xi0];

                if (recipient.Equals(BOB)) {
                    GameObject.Find("bob").GetComponent<NPC>().ReceiveExpression(this.name, assertion);
                }

                if (recipient.Equals(EVAN)) {
                    GameObject.Find("evan").GetComponent<NPC>().ReceiveExpression(this.name, assertion);
                }
            }

            MetaVariable xc0 = new MetaVariable(SemanticType.CONFORMITY_VALUE, 0);

            IPattern conformitySchema =
                new ExpressionPattern(WOULD,
                    new ExpressionPattern(EXPRESS_CONFORMITY, SELF, xi0, xc0));

            List<Dictionary<MetaVariable, Expression>> conformityBinding = conformitySchema.GetBindings(action);

            if (conformityBinding != null) {
                Expression conformity = conformityBinding[0][xc0];
                yield return ShowSpeechBubble(conformity);
                Expression recipient = conformityBinding[0][xi0];

                if (recipient.Equals(BOB)) {
                    GameObject.Find("bob").GetComponent<NPC>().ReceiveExpression(this.name, conformity);
                }

                if (recipient.Equals(EVAN)) {
                    GameObject.Find("evan").GetComponent<NPC>().ReceiveExpression(this.name, conformity);
                }

                if (recipient.Equals(new Phrase(THE,  new Phrase(POSSESS, new Phrase(THE, LOG), 1)))) {
                    GameObject.Find("Woodcutter").GetComponent<NPC>().ReceiveExpression(this.name, conformity);
                }

                this.model.UpdateBelief(new Phrase(MAKE, SELF,
                    new Phrase(EXPRESS_CONFORMITY, SELF, recipient, conformity)));
            }

            IPattern possessionSchema =
                new ExpressionPattern(WOULD,
                    new ExpressionPattern(POSSESS, xi0, xi1));

            List<Dictionary<MetaVariable, Expression>> possessionBinding = possessionSchema.GetBindings(action);

            if (possessionBinding != null) {
                Expression possessor = possessionBinding[0][xi0];
                Expression item = possessionBinding[0][xi1];
                GameObject inventory = GameObject.Find(this.gameObject.name + "/Inventory");
                foreach (Transform t in inventory.GetComponentsInChildren<Transform>()) {
                    if (t.gameObject.name.ToLower().Equals(item.ToString())) {
                        t.gameObject.transform.SetParent(GameObject.Find(possessor.ToString() + "/Inventory").transform);
                        t.gameObject.transform.position = GameObject.Find(possessor.ToString() + "/Inventory").transform.position;
                        // TODO UNTESTED
                        GameObject possessorObject = GameObject.Find(possessor.ToString());
                        Debug.Log(possessorObject);
                        NPC possessorNPC = possessorObject.GetComponent<NPC>();
                        Debug.Log(possessorNPC.name);
                        Debug.Log(possessorNPC.model);
                        possessorNPC.model.UpdateBelief(new Phrase(POSSESS, possessor, item));
                        // t.position = new Vector3(5f, 1f, 0f);
                    }
                }

                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(NOT, new Phrase(POSSESS, SELF, item))));
                this.model.UpdateBelief(new Phrase(MAKE, SELF, new Phrase(POSSESS, possessor, item)));
            }
        }
        // this.controller.combineSuccess.Play();
        // yield return ShowSpeechBubble("yes");
        model.ClearGoal();
        actionInProgress = false;
        
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
                ReceiveExpression(PLAYER, selectedExpr.expression);
                Destroy(selectedExpr.gameObject);
                controller.HidePointer();
                // scontroller.SetInSpeakingMode(false);
            }
        }
    }

    // @NOTE turned off for testing
    public void ReceivePercept(params Expression[] percept) {
        if (this.model == null) {
            return;
        }

        foreach (Expression p in percept) {
            this.model.UpdateBelief(new Phrase(PERCEIVE, SELF, p));
        }
    }

    public void ReceiveExpression(Expression utterer, Expression utterance) {
        // Debug.Log(this.name + " is seeing '" + utterance + "'");
        if (this.model == null) {
            // Debug.Log("No associated model.");
            this.controller.placeExpression.Play();
            StartCoroutine(ShowSpeechBubble("questionMark"));
            return;
        }

        if (utterance.type.Equals(SemanticType.CONFORMITY_VALUE)) {
            // if (name.Equals(new Phrase(THE,  new Phrase(POSSESS, new Phrase(THE, LOG), 1)))) {
            //     if (!model.Proves(new Phrase(KING, utterer))) {
            //         StartCoroutine(ShowSpeechBubble(REFUSE));
            //         return;
            //     }
            // }

            // TODO figure out conditions of refusal, etc.
            if (utterance.GetHead().Equals(CONTRACT)) {
                if (!this.model.Proves(new Phrase(NOT, new Phrase(TRUSTWORTHY, utterer)))) {
                    // TODO: check if utilities work out, and if you can uphold your end of the deal.
                    // For now, just accept by default
                    // uphold your end of the bargain
                    model.Add(new Phrase(BETTER, utterance.GetArg(1), NEUTRAL));
                    // model.AddUtility(utterance.GetArg(1), 10f);
                    
                    // hold the other person to account
                    model.UpdateBelief(new Phrase(BOUND, utterer, utterance.GetArg(0)));

                    // // hold yourself to account??
                    // model.UpdateBelief(new Phrase(BOUND, utterer, utterance.GetArg(1)));

                    this.controller.combineSuccess.Play();
                    StartCoroutine(ShowSpeechBubble(new Phrase(ACCEPT)));

                    return;
                }

                this.controller.lowClick.Play();
                StartCoroutine(ShowSpeechBubble(REFUSE));

                return;
            }

            if (utterance.GetHead().Equals(WOULD)) {
                if (this.model.Proves(new Phrase(NOT, new Phrase(TRUSTWORTHY, utterer)))) {
                    this.controller.lowClick.Play();
                    StartCoroutine(ShowSpeechBubble(new Phrase(REFUSE)));

                    return;
                }

                this.controller.combineSuccess.Play();
                StartCoroutine(ShowSpeechBubble(ACCEPT));

                model.Add(new Phrase(BETTER, utterance.GetArg(0), NEUTRAL));
                model.decisionLock = false;
                // model.AddUtility(utterance.GetArg(0), 10f);
            
                return;
            }
        }

        if (utterance.type.Equals(SemanticType.ASSERTION)) {
            Expression content = utterance.GetArg(0);
            // if (this.model.UpdateBelief(new Phrase(BELIEVE, utterer, content))) {
            if (this.model.UpdateBelief(content)) {
                this.controller.combineSuccess.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(ASSERT, AFFIRM)));
            } else {
                this.controller.failure.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(ASSERT, DENY)));
            }
            return;
        }

        if (utterance.type.Equals(SemanticType.TRUTH_VALUE)) {
            if (this.model.Proves(utterance)) {
                this.controller.combineSuccess.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(ASSERT, AFFIRM)));
            } else if (this.model.Proves(new Phrase(NOT, utterance))) {
                this.controller.failure.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(ASSERT, DENY)));
            } else {
                this.controller.lowClick.Play();
                StartCoroutine(ShowSpeechBubble(new Phrase(ASSERT, new Phrase(OR, AFFIRM, DENY))));
            }
            return;
        }

        if (utterance.type.Equals(SemanticType.INDIVIDUAL)) {
            // I imagine this will be an invitation to attend to the individual named
            // but I'll leave this as a kind of puzzlement for now.
            this.controller.placeExpression.Play();
            StartCoroutine(ShowSpeechBubble("query"));
            return;
        }

        SemanticType outputType = utterance.type.GetOutputType();
        if (outputType != null && outputType.Equals(SemanticType.TRUTH_VALUE)) {
            IPattern[] args = new IPattern[utterance.GetNumArgs()];
            Dictionary<SemanticType, int> places = new Dictionary<SemanticType, int>();
            int counter = 0;
            for (int i = 0; i < utterance.GetNumArgs(); i++) {
                if (utterance.GetArg(i) == null) {
                    SemanticType argType = utterance.type.GetInputType(counter);

                    int place = 0;
                
                    if (places.ContainsKey(argType)) {
                        place = places[argType];
                    }
                    places[argType] = place + 1;

                    args[i] = new MetaVariable(argType, place);

                    counter++;
                } else {
                    args[i] = utterance.GetArg(i);
                }
            }
            // let's see if this doesn't break things.
            // might have to cycle through the utterance's open args.
            IPattern question = new ExpressionPattern(utterance.GetHead(), args);

            List<Dictionary<MetaVariable, Expression>> found = model.Find(false, new List<Expression>(), question);
            if (found != null) {
                List<IPattern> bound = question.Bind(found);
                Expression[] answers = new Expression[bound.Count];
                counter = 0;
                foreach (IPattern p in bound) {
                    Expression answer = new Phrase(ASSERT, p.ToExpression());
                    answers[counter] = answer;
                    counter++;
                }
                StartCoroutine(ShowSpeechBubbles(answers));
                return;
            }
        }
        
        // this leaves functions with e as output. Not sure what this would amount to.
        this.controller.placeExpression.Play();
        StartCoroutine(ShowSpeechBubble("query"));
    }

    public IEnumerator ShowSpeechBubbles(params Expression[] exprs) {
        foreach (Expression expr in exprs) {
            yield return ShowSpeechBubble(expr);
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
            exprPieceScript.transform.position = cam.WorldToScreenPoint(this.transform.position);
            exprPieceScript.transform.position =
                new Vector3(exprPieceScript.transform.position.x,
                    exprPieceScript.transform.position.y +
                    (exprPieceScript.heightInUnits * ExpressionPiece.PIXELS_PER_UNIT / 2) +
                    16);
        } else {
            cam = GameObject.Find("FirstPersonCharacter").GetComponent<Camera>();
            exprPieceScript.transform.position = cam.WorldToScreenPoint(this.transform.position);
            exprPieceScript.transform.position =
                new Vector3(exprPieceScript.transform.position.x,
                    exprPieceScript.transform.position.y +
                    (exprPieceScript.heightInUnits * ExpressionPiece.PIXELS_PER_UNIT / 2));
        }

        exprPieceScript.SetVisual(exprPieceScript.GenerateVisual());
        Destroy(exprPieceInstance, 10.0f);
        yield return new WaitForSeconds(10.0f);
    }

    // public IEnumerator GoToLocation(Expression location) {
    //     if (!location.GetHead().Equals(LOCATION)) {
    //         yield break;
    //     }



    //     target = null; //transform
    //     speed = 2;
    //     GoToTarget();
    //     while (!walkingComplete) {
    //         yield return null;
    //     }
    //     yield break;
    // }

    public IEnumerator GoTo(String targetID) {
        // deprecated: objective unity coordinate from object
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
        if (!locked) {
            Perceivable po = other.GetComponent<Perceivable>();
        
            if (po != null) {
                po.SendPercept(this);
            }

            Perceivable[] childrenPOs = GetComponentsInChildren<Perceivable>();
            for (int i = 0; i < childrenPOs.Length; i++) {
                childrenPOs[i].SendPercept(this);
            }

            if (other.CompareTag("Interactable")) {
                currentInteractObject = other.gameObject;
            }

            locked = true;
            StartCoroutine("Waiting");
        }
    }


    // //NOTE: objects are perceived both when NPC enters and exits their range of perceptability
    // public void OnTriggerExit2D(Collider2D other) {
    // }

    // called when Character enters the trigger collider of an object 
    public void OnTriggerEnter(Collider other) {
        if (!locked) {
            Perceivable po = other.GetComponent<Perceivable>();
            
            if (po != null) {
                // Debug.Log(name + " sees " + po);
                po.SendPercept(this);
            }

            Perceivable[] childrenPOs = GetComponentsInChildren<Perceivable>();
            for (int i = 0; i < childrenPOs.Length; i++) {
                // Debug.Log(name + " sees " + childrenPOs[i]);
                childrenPOs[i].SendPercept(this);
            }

            if (other.CompareTag("Interactable")) {
                currentInteractObject = other.gameObject;
            }

            locked = true;
            StartCoroutine("Waiting");
        }
    }

    public IEnumerator Waiting() {
        yield return new WaitForSeconds(1f);
        locked = false;
        yield return null;
    }

    // // NOTE: objects are perceived both when NPC enters and exits their range of perceptability
    // public void OnTriggerExit(Collider other) {
    // }
}
