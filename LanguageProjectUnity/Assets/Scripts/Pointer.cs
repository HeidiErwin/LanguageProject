using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {
    [SerializeField] private GameObject gameController;
    
    private GameController gc;
    // Start is called before the first frame update
    void Start() {
        gc = gameController.GetComponent<GameController>();
    }

    static void SetOutlineColor(GameObject o, Color c) {
        Renderer r = o.GetComponent<Renderer>();
        if (r == null) {
            r = o.GetComponentsInChildren<Renderer>()[0];
        }
        if (r != null) {
            r.material.SetColor("_OutlineColor", c);
        }
    }

    // Update is called once per frame
    void Update() {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8 | 1 << 9;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3f, layerMask)) {
            Color c = hit.transform.gameObject.tag == "Interactable" ? Color.blue : Color.red;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, c);
            if (hit.transform.gameObject.tag == "Interactable") {
                gc.currentInteractObject = hit.transform.gameObject;
                SetOutlineColor(hit.transform.gameObject, new Color(0, 0.6f, 1, 1));
                // hit.transform.gameObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(0, 0.6f, 1, 1));
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)) {
                    if (gc.usableExpression) {
                        NPC addressee = hit.transform.gameObject.GetComponent<NPC>();
                        if (addressee != null) {
                            addressee.ReceiveExpression(Expression.PLAYER, gc.usableExpression.expression);
                            Destroy(gc.usableExpression.gameObject, 0f);
                            gc.usableExpression = null;
                        }                    
                    } else if (hit.transform.gameObject.name.Equals("Button")) {
                        gc.door.SetActive(!gc.door.activeSelf);
                    } else if (hit.transform.gameObject.name.Equals("Prize")) {
                        GameObject prize = hit.transform.gameObject;
                        prize.transform.SetParent(GameObject.Find("FirstPersonCharacter").transform);
                        gc.currentUseObject = prize;
                    }
                }
            } else if (gc.currentInteractObject) {
                SetOutlineColor(gc.currentInteractObject, new Color(0, 0, 0, 1));
                // gc.currentInteractObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
                gc.currentInteractObject = null;
            }
        } else if (gc.currentInteractObject) {
            SetOutlineColor(gc.currentInteractObject, new Color(0, 0, 0, 1));
            // gc.currentInteractObject.GetComponent<Renderer>().material.SetColor("_OutlineColor", new Color(0, 0, 0, 0));
            gc.currentInteractObject = null;
        }
    }
}
