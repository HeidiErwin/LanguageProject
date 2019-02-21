using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnter : MonoBehaviour {
    // called when Character enters the trigger collider of an object 
    public void OnTriggerEnter(Collider other) {
        Perceivable po = other.GetComponent<Perceivable>();
        
        if (po != null) {
            po.isInRoom = true;
        }

        Perceivable[] childrenPOs = GetComponentsInChildren<Perceivable>();
        for (int i = 0; i < childrenPOs.Length; i++) {
            childrenPOs[i].isInRoom = true;
        }

        NPC npc = other.GetComponent<NPC>();

        if (npc != null) {
            npc.ReceivePercept(new Phrase(Expression.IN_THE_ROOM, Expression.SELF));
        }
    }

    public void OnTriggerExit(Collider other) {
        Perceivable po = other.GetComponent<Perceivable>();
        
        if (po != null) {
            po.isInRoom = false;
        }

        Perceivable[] childrenPOs = GetComponentsInChildren<Perceivable>();
        for (int i = 0; i < childrenPOs.Length; i++) {
            childrenPOs[i].isInRoom = false;
        }

        NPC npc = other.GetComponent<NPC>();

        if (npc != null) {
            npc.ReceivePercept(new Phrase(Expression.IN_THE_ROOM, Expression.SELF));
        }
    }
}
