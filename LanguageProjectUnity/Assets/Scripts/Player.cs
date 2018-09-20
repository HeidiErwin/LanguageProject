using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    int counter = 0;
    GameObject currentInteractObject; // the object you can currently interact with
    public const KeyCode INTERACT_KEY = KeyCode.E;

    protected override void Update() {
        GetInput();
        transform.position = new Vector2(0, 4);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        base.Update();
        if(Input.GetKeyDown(INTERACT_KEY) && currentInteractObject) {
            currentInteractObject.SendMessage("Interact");
        }
    }

    /**
    * Moves the Player based on arrow key input. Or rather, sets the
    * velocity field of the player so that Move() can update the player's 
    * location properly.
    */
    private void GetInput() {
        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            directionFacing = NORTH;
            velocity += Vector2.up;
        }

        if (Input.GetKey(KeyCode.D)) {
            directionFacing = EAST;
            velocity += Vector2.right;
        }

        if (Input.GetKey(KeyCode.S)) {
            directionFacing = SOUTH;
            velocity += Vector2.down;
        }
        
        if (Input.GetKey(KeyCode.A)) {
            directionFacing = WEST;
            velocity += Vector2.left;
        }
    }

    // called when Player enters the trigger collider of an object 
    public void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Interactable")) {
            currentInteractObject = other.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Interactable") && other.gameObject == currentInteractObject) {
            currentInteractObject = null;
        }
    }
}
