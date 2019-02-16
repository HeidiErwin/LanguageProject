using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    GameObject currentInteractObject; // the object you can currently interact with
    public GameObject currentWearObject;
    public bool isWearing = false;
    public const KeyCode INTERACT_KEY = KeyCode.E;
    public const KeyCode WEAR_KEY = KeyCode.Q;

    protected void Start() {
        transform.position = new Vector2(-4, -4);
        Animator anim = gameObject.AddComponent(typeof(Animator)) as Animator;
    }

    protected override void Update() {
        GetInput();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        base.Update();


        
        if (Input.GetKeyDown(INTERACT_KEY) && currentInteractObject) {
            Debug.Log(currentInteractObject);
            currentInteractObject.SendMessage("Interact");
        }

        if (Input.GetKeyDown(WEAR_KEY)) {
            isWearing = !isWearing;
            if (currentWearObject) {
                currentWearObject.SetActive(isWearing);    
            }
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
