using System;
using UnityEngine;
/**
* This script gets attached to any object that the player
* can perceive. Currently (9/19) serving as a container to store color.
*/
public class Perceivable : MonoBehaviour {
    [SerializeField] int id;
    Expression[] properties;
    [SerializeField] bool[] reflectance;
    [SerializeField] EnvironmentManager em;

    void Start() {
        Expression param = new Parameter(SemanticType.INDIVIDUAL, id);
        properties = new Expression[]{
            new Phrase(Expression.FOUNTAIN, param),
            new Phrase(Expression.ACTIVE, param),
            new Phrase(Expression.IN_RED_AREA, param)};
    }

    public void SendPerceptualReport(NPC npc) {
        Expression[] reports = new Expression[properties.Length + 1];
        for (int i = 0; i < properties.Length; i++) {
            reports[i] = properties[i];
        }

        Expression colorReport;

        bool[] reflectedLight = new bool[3];

        reflectedLight[0] = em.lighting[0] && reflectance[0];
        reflectedLight[1] = em.lighting[1] && reflectance[1];
        reflectedLight[2] = em.lighting[2] && reflectance[2];

        // black/no light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.BLACK, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // red light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && !reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.RED, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // green light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.GREEN, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // blue light reflected off an object
        if (!reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.WHITE, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // yellow light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && !reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.YELLOW, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // magenta light reflected off an object
        if (reflectedLight[0] && !reflectedLight[1] && reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.MAGENTA, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // cyan light reflected off an object
        if (!reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.CYAN, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }

        // white light reflected off an object
        if (reflectedLight[0] && reflectedLight[1] && reflectedLight[2]) {
            reports[properties.Length] = new Phrase(Expression.WHITE, new Parameter(SemanticType.INDIVIDUAL, id));
            npc.ReceivePerceptualReport(reports);
        }
    }
}
