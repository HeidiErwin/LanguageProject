using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public bool open = false;
    public bool locked = false;

    [SerializeField] Sprite openDoorSprite;
    [SerializeField] Sprite closedDoorSprite;
    [SerializeField] GameObject key; // item needed to unlock door

    public void Interact() {
        if (open) {
            Close();
        } else if (!open && !locked){
            Open();
        }
    }

    public void Open() {
        GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        open = true;
    }

    public void Close() {
        GetComponent<SpriteRenderer>().sprite = closedDoorSprite;
        open = false;
    }
}
