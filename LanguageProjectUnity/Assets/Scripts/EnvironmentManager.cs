using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class sends perception messages to NPCs, communicating 
// between objects of the world and the NPCs perceiving them.
// There must be an object with an EnvironmentManager script in the scene in order
// for objects to be able to communicate with the Environment Manager.
public class EnvironmentManager : MonoBehaviour {

    List<GameObject> environmentObjects;
    Color environmentColor = Color.blue;

    public void Start() {
        environmentObjects = new List<GameObject>(FindObjectsOfType<GameObject>());
    }

    /**
     * Based on all the objects in the environment, determines what to tell an NPC to perceive. 
     * @param npc - the NPC to send the perceptional report to
     */
    public void ComputePerceptionalReport(NPC npc) {
        //TODO: in NPC class, given NPCs a rangeOfPerception field, so that in the future
        //      when this is more streamlined, NPCs see only what is around them
        // Actually, no... will instead give objects Perceivable scripts, with a rangeOfPerceivability, 
        // as trigger colliders will be attached to these objects rather than to the NPC.

        List<GameObject> objectsToPerceive = new List<GameObject>();

        //TODO: once rangeOfPercevability implemented, only loop through objects in Npc's view
        foreach (GameObject gameObj in environmentObjects) {
            objectsToPerceive.Add(ComputePerception(gameObj));
        }

        npc.Perceive(objectsToPerceive); // is this what we want to pass to the NPC?
    }

    // Returns an object that is how the object obj should be perceived 
    // e.g. if obj is a white sheet of paper, but the environment is dark, a 
    // dark grey/black sheet of paper will be returned.
    // For now, (9/19), this method only deals with combining 8 basic colors.
    private GameObject ComputePerception(GameObject obj) {
        GameObject perceivedObj = Instantiate(obj) as GameObject; //make clone
        perceivedObj.GetComponent<Perceivable>().SetColor(CombineColors(environmentColor, perceivedObj.GetComponent<Perceivable>().GetColor()));

        return perceivedObj;
    }

    /**
     * Currently using an Additive Color system, which describes the way the eye detects color
     */
    public Color CombineColors(Color color1, Color color2) {
        //TODO: @Bill

        return new Color(0,0,0); //placeholder, change later
    }

}
