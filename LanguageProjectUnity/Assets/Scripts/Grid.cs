using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public LayerMask unwalkableMask;
    public bool displayGridGizmos;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    private float nodeDiameter;
    int gridSizeX, gridSizeY;

    private void Awake() {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    private void OnDrawGizmos() {
        // Debug.Log("drawing gizmos");
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

        if (grid != null && displayGridGizmos) {
            Node playerNode = NodeFromWorldPoint(GameObject.Find("Player(Clone)").transform.position);
            foreach(Node n in grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (n == playerNode)
                Gizmos.color = Color.green;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    private void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft +
                    Vector3.right * (x * nodeDiameter + nodeRadius) +
                    Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // returns the node of the grid that a world point is in 
    // can be used to, for instance, determine which Node of the grid an NPC is in
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; // 0 if far left, .5 if center x, 1 if far right
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y; // 0 if bottom, .5 if center x, 1 if top
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY); // clamp so we don't somehow get values < 0 or > 1

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX); // subtract 1 so not outside of array (indexing starts at 0)
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Vector3 WorldPointFromNode (Node n) {
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        
        return worldBottomLeft +
                    Vector3.right * (n.gridX * nodeDiameter + nodeRadius) +
                    Vector3.up * (n.gridY * nodeDiameter + nodeRadius);
    }

    public List<Node> GetNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // make sure checkX & checkY actually in grid:
                if(checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) {
                    neighbors.Add(grid[checkX, checkY]);
                }

            }
        }

        return neighbors;
    }

    public int MaxSize {
        get {
            return gridSizeX * gridSizeY;
        }
    }
}
