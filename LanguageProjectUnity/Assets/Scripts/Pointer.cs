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

    // Update is called once per frame
    void Update() {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask)) {
            Color c = hit.transform.gameObject.GetComponent<NPC>() == null ? Color.red : Color.green;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, c);

            if (Input.GetMouseButtonUp(0) && gc.usableExpression) {
                NPC addressee = hit.transform.gameObject.GetComponent<NPC>();
                if (addressee != null) {
                    addressee.ReceiveExpression(Expression.PLAYER, gc.usableExpression.expression);
                    Destroy(gc.usableExpression.gameObject, 0f);
                    gc.usableExpression = null;
                }
            }
            // Debug.Log("Did Hit");
        }
        else {
            // Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            // Debug.Log("Did not Hit");
        }
    }
}
