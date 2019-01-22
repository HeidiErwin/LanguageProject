using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * NPC and Player will both extend Character (those will likely be the only two types of Characters)
 */
public abstract class Character : MonoBehaviour {

    public const int NORTH = 0;
    public const int EAST = 1;
    public const int SOUTH = 2;
    public const int WEST = 3;
    protected bool walking = false;

    [SerializeField] protected float speed;
    protected Vector2 velocity;
    protected int directionFacing;
    [SerializeField] Sprite[] sprites;

    public Transform target;
    Vector3[] path;
    int targetIndex;

    public bool walkingComplete = false;

    protected virtual void Update() {
        Move();
    }

    /**
     * Updates the transform (position) of this Character based on the velocity field of
     * the character. 
     */
    public void Move() {
        // if (velocity.x != 0 || velocity.y != 0) {
        //     Debug.Log("moving");
        //     GetComponent<Animator>().SetBool("isMoving", true);
        // }
        // if (velocity.x > velocity.y) {
        //     if (velocity.x > 0) {
        //         GetComponent<Animator>().SetInteger("direction", 0);
        //     } else {
        //         GetComponent<Animator>().SetInteger("direction", 2);
        //     }
        // } else {
        //     if (velocity.y > 0) {
        //         GetComponent<Animator>().SetInteger("direction", 1);
        //     } else {
        //         GetComponent<Animator>().SetInteger("direction", 3);
        //     }
        // }
        transform.Translate(velocity * speed * Time.deltaTime);
    }

    public void GoToTarget() {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
        if (pathSuccessful) {
            path = newPath;

            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
            // FollowPath();
        }
    }

    public void OnDrawGizmos() { // draws path
        if (path != null) {
            for (int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(.5f,.5f,.5f));

                if (i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]);
                } else {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    // TODO change this back to an IEnumerator
    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0];
        walking = true;
        while (true) {
            if (transform.position == currentWaypoint) {
                targetIndex++;
                if (targetIndex >= path.Length) {
                    walking = false;
                    walkingComplete = true;
                    yield break; //exit coroutine when finished following path
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

}
