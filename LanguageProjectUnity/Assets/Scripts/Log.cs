using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Log : MonoBehaviour {
    bool equipped = false;

    public void Interact() {
        if (equipped) {
            this.transform.parent = null;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.25f, -1f);
            GameObject.Find("Player(Clone)").GetComponent<Player>().currentHoldObject = null;
            equipped = !equipped;
        } else {
            GameObject player = GameObject.Find("Player(Clone)");
            this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 0.25f, -1f);
            this.transform.SetParent(player.transform);
            player.GetComponent<Player>().currentHoldObject = this.gameObject;
            equipped = !equipped;
        }
    }
}
