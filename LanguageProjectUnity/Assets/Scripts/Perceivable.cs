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

    protected Expression name;

    public virtual void SendPerceptualReport(NPC npc) {
        bool[] reflectedLight = new bool[3];
        
        reflectedLight[0] = em.lighting[0] && reflectance[0];
        reflectedLight[1] = em.lighting[1] && reflectance[1];
        reflectedLight[2] = em.lighting[2] && reflectance[2];

        Expression currentName = null; 
        if (name == null) {
            currentName = new Parameter(SemanticType.INDIVIDUAL, id);
        } else {
            currentName = name;
        }

        Expression[] reports = new Expression[]{
            new Phrase(Expression.NOT, new Phrase(Expression.BLACK, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.RED, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.GREEN, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.BLUE, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.YELLOW, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.MAGENTA, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.CYAN, currentName)),
            new Phrase(Expression.NOT, new Phrase(Expression.WHITE, currentName)),
        };

        // black/no light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[0] = new Phrase(Expression.BLACK, currentName);
        }

        // red light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[1] = new Phrase(Expression.RED, currentName);
        }

        // green light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[2] = new Phrase(Expression.GREEN, currentName);
        }

        // blue light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[3] = new Phrase(Expression.BLUE, currentName);
        }

        // yellow light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[4] = new Phrase(Expression.YELLOW, currentName);
        }

        // magenta light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[5] = new Phrase(Expression.MAGENTA, currentName);
        }

        // cyan light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[6] = new Phrase(Expression.CYAN, currentName);
        }

        // white light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[7] = new Phrase(Expression.WHITE, currentName);
        }

        npc.ReceivePerceptualReport(reports);
    }
}
