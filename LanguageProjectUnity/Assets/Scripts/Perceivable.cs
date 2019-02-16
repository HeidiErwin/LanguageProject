using System;
using UnityEngine;
/**
* This script gets attached to any object that the player
* can perceive. Currently (9/19) serving as a container to store color.
*/
public class Perceivable : MonoBehaviour {
    [SerializeField] protected int id;
    [SerializeField] bool[] reflectance;
    [SerializeField] EnvironmentManager em;
    [SerializeField] protected String nameString;
    protected Expression param;

    public virtual void SendPercept(NPC npc) {
        bool[] reflectedLight = new bool[3];
        
        reflectedLight[0] = em.lighting[0] && reflectance[0];
        reflectedLight[1] = em.lighting[1] && reflectance[1];
        reflectedLight[2] = em.lighting[2] && reflectance[2];

        param = new Parameter(SemanticType.INDIVIDUAL, id);

        if (id == 0) {
            param = new Phrase(Expression.THE, new Phrase(Expression.MOD, Expression.BLACK, Expression.LAMP));
        }

        if (id == 1) {
            param = new Phrase(Expression.THE, new Phrase(Expression.MOD, Expression.GREEN, Expression.FOUNTAIN));
        }

        if (id == 2) {
            param = new Phrase(Expression.THE, new Phrase(Expression.MOD, Expression.RED, Expression.LAMP));
        }

        if (id == 3) {
            param = new Phrase(Expression.THE,
                new Phrase(Expression.MOD,
                    new Phrase(Expression.MOD, Expression.BLUE, Expression.ACTIVE),
                    Expression.FOUNTAIN));
        }

        if (id == 4) {
            param = new Phrase(Expression.THE, new Phrase(Expression.MOD, Expression.INACTIVE, Expression.FOUNTAIN));
        }

        if (id == 5) {
            param = new Phrase(Expression.THE, Expression.DOOR);
        }

        if (id == 6) {
            param = new Phrase(Expression.THE, Expression.COW);
        }

        if (nameString != null && !nameString.Equals("")) {
            param = new Word(SemanticType.INDIVIDUAL, nameString);
        }

        // black/no light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.BLACK, param));
            return;
        }

        // red light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.RED, param));
            return;
        }

        // green light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.GREEN, param));
            return;
        }

        // blue light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.BLUE, param));
            return;
        }

        // yellow light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.YELLOW, param));
            return;
        }

        // magenta light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.MAGENTA, param));
            return;
        }

        // cyan light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.CYAN, param));
            return;
        }

        // white light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            npc.ReceivePercept(new Phrase(Expression.WHITE, param));
            return;
        }
    }
}
