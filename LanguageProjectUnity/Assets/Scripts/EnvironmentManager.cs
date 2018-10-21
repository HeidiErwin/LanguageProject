using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class sends perception messages to NPCs, communicating 
// between objects of the world and the NPCs perceiving them.
// There must be an object with an EnvironmentManager script in the scene in order
// for objects to be able to communicate with the Environment Manager.
public class EnvironmentManager : MonoBehaviour {
    [SerializeField] public bool[] lighting;
}
