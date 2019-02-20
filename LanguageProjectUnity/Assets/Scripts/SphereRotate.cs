using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRotate : MonoBehaviour {
    public float speed = 50f;
    public bool x = false;
    public bool y = true;
    public bool z = false;

    // Update is called once per frame
    void Update() {
        if (x) {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);    
        }
        if (y) {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
        }
        if (z) {
            transform.Rotate(Vector3.left, speed * Time.deltaTime);    
        }
    }
}
