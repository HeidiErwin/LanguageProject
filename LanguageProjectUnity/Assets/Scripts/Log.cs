using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour {
    public void Interact() {
        GameObject player = GameObject.Find("Player(Clone)");
        this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.25f, -1f);
        this.transform.SetParent(player.transform);
        this.gameObject.layer = 9;
    }
}
