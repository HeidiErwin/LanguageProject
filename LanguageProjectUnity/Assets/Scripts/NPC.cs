using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : Character{

    GameController controller;

	// Use this for initialization
	void Start () {
        controller = GameObject.Find("GameController").GetComponent<GameController>();
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
        Debug.Log("NPC is hearing " + exprPiece.expression.ToString());
    }
}
