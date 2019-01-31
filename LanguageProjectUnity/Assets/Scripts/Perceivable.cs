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
            param = new Phrase(Expression.THE, new Phrase(Expression.GEACH_TF2, Expression.AND, Expression.BLACK, Expression.LAMP));
        }

        if (id == 1) {
            param = new Phrase(Expression.THE, new Phrase(Expression.GEACH_TF2, Expression.AND, Expression.GREEN, Expression.FOUNTAIN));
        }

        if (id == 2) {
            param = new Phrase(Expression.THE, new Phrase(Expression.GEACH_TF2, Expression.AND, Expression.RED, Expression.LAMP));
        }

        if (id == 3) {
            param = new Phrase(Expression.THE,
                new Phrase(Expression.GEACH_TF2, Expression.AND,
                    new Phrase(Expression.GEACH_TF2, Expression.AND, Expression.BLUE, Expression.ACTIVE),
                    Expression.FOUNTAIN));
        }

        if (id == 4) {
            param = new Phrase(Expression.THE, new Phrase(Expression.GEACH_TF2, Expression.AND, Expression.INACTIVE, Expression.FOUNTAIN));
        }

        if (id == 5) {
            param = new Phrase(Expression.THE, Expression.DOOR);
        }

        if (nameString != null && !nameString.Equals("")) {
            param = new Word(SemanticType.INDIVIDUAL, nameString);
        }

        Expression[] reports = new Expression[]{
            new Phrase(Expression.NOT, new Phrase(Expression.BLACK, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.RED, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.GREEN, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.BLUE, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.YELLOW, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.MAGENTA, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.CYAN, param)),
            new Phrase(Expression.NOT, new Phrase(Expression.WHITE, param)),
        };

        // black/no light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[0] = new Phrase(Expression.BLACK, param);
        }

        // red light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[1] = new Phrase(Expression.RED, param);
        }

        // green light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[2] = new Phrase(Expression.GREEN, param);
        }

        // blue light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[3] = new Phrase(Expression.BLUE, param);
        }

        // yellow light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[4] = new Phrase(Expression.YELLOW, param);
        }

        // magenta light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[5] = new Phrase(Expression.MAGENTA, param);
        }

        // cyan light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[6] = new Phrase(Expression.CYAN, param);
        }

        // white light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[7] = new Phrase(Expression.WHITE, param);
        }

        npc.ReceivePercept(reports);
    }
}
