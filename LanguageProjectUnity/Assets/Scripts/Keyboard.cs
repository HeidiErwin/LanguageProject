using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {

    public void ChangeColor(Color32 color) {
        this.gameObject.GetComponentInChildren<Image>().color = color;
    }
}
