using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Orbit : MonoBehaviour {
    HashSet<Transform> inventory = new HashSet<Transform>();
    bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        bool update = false;
        foreach (Transform child in transform) {
            if (!inventory.Contains(child)) {
                update = true;
                break;
            }
        }
        if (update) {
            inventory.Clear();
            foreach (Transform child in transform) {
                inventory.Add(child);
            }


            if (transform.childCount != 0) {
                // for a regular polygon pattern with the objects.
                float i = 0;
                foreach (Transform child in transform) {
                    double angle = (i * 2 * Math.PI) / transform.childCount;
                    child.position += new Vector3((float) Math.Cos(angle), (float) Math.Sin(angle), 0) * 0.25f;
                    i++;
                }
            }
        }

        transform.Rotate(0, 0, 1f);
    }
}
