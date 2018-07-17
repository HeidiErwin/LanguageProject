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

    [SerializeField]
    protected float speed;
    protected Vector2 velocity;
    protected int directionFacing;

    protected virtual void Update() {
        Move();
    }

    /**
     * Updates the transform (position) of this Character based on the velocity field of
     * the character. 
     */
    public void Move() {
        transform.Translate(velocity * speed * Time.deltaTime);
    }

}
