using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

    public bool walkable;
    public Vector3 worldPosition;

    public Node parent; //p revious node in the shortest path; used by Pathfinding script

    public int gCost;
    public int hCost;
    public int gridX; // node keeps track of own pos in grid
    public int gridY;
    int heapIndex;

    public Node(bool walk, Vector3 worldPos, int _gridX, int _gridY) {
        walkable = walk;
        worldPosition = worldPos;
        gridX = _gridX;
        gridY = _gridY;

    }

    // getter; returns the f cost of this node
    public int fCost {
        get {
            return gCost + hCost;
        }
    }

    public int HeapIndex {
        get {
            return heapIndex;
        }
        set {
            heapIndex = value;
        }
    }

    public int CompareTo (Node nodeToCompare) {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
