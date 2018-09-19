
using System;
using UnityEngine;
/**
* This script gets attached to any object that the player
* can perceive. Currently (9/19) serving as a container to store color.
*/
public class Perceivable {

    Color color;

    public void SetColor(Color color) {
        this.color = color;
    }

    public Color GetColor() {
        return this.color;
    }
}