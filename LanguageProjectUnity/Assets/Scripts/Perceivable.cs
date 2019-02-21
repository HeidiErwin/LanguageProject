using System;
using UnityEngine;
/**
* This script gets attached to any object that the player
* can perceive. Currently (9/19) serving as a container to store color.
*/
public class Perceivable : MonoBehaviour {
    [SerializeField] protected String nameString;

    public bool isInRoom = false;

    public virtual void SendPercept(NPC npc) {
        Expression o = new Word(SemanticType.INDIVIDUAL, nameString);

        if (isInRoom) {
            npc.ReceivePercept(new Phrase(Expression.IN_THE_ROOM, o));
            return;
        }
        
        npc.ReceivePercept(new Phrase(Expression.NOT, new Phrase(Expression.IN_THE_ROOM, o)));
        return;
    }
}
