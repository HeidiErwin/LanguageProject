using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : Character {

    GameController controller;
    [SerializeField] protected String nameString;
    private IModel model;

	// Use this for initialization
	void Start () {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
        if (nameString.Equals("Bob")) {
            model = Models.BobModel();
        }

        if (nameString.Equals("Evan")) {
            model = Models.EvanModel();
        }

        Debug.Log(model.ToString());

        HashSet<Expression> h = new HashSet<Expression>();
        h.Add(new Phrase(Expression.ACTIVE, Expression.BOB));

        Debug.Log(h.Contains(new Phrase(Expression.ACTIVE, Expression.BOB)));
	}

    /**
     * if the game is in Speaking Mode (kept track of by a bool in GameController,
     * and this NPC is clicked, say the expression to the NPC.
     */
    private void OnMouseDown() {
        ExpressionPiece selectedExpr = controller.GetSelectedExpression();
        if (selectedExpr == null) {
            Debug.Log("No selected expression to say to this NPC");
        } else {
            ReceiveExpression(selectedExpr);
            Destroy(selectedExpr.gameObject);
            controller.SetInSpeakingMode(false);
        }
    }

    /**
     * TODO: Bill
     * fill in w/ how NPC responds to the received expression
     */ 
    void ReceiveExpression (ExpressionPiece exprPiece) {
        Expression utterance = exprPiece.expression;

        Debug.Log(this.nameString + " is seeing '" + utterance + "'");

        if (this.model == null) {
            Debug.Log("No associated model.");
            // TODO "Huh?" animation
            return;
        }

        if (!utterance.type.Equals(SemanticType.TRUTH_VALUE)) {
            Debug.Log("Semantic Type of utterance is not sentence/truth value.");
            // TODO "Huh?" animation
            return;
        }

        if (this.model.Contains(utterance)) {
            Debug.Log("That's TRUE in their model.");
            // TODO "Yes!" animation
        } else {
            Debug.Log("That's FALSE in their model.");
            // TODO "No." animation
        }
    }
}
