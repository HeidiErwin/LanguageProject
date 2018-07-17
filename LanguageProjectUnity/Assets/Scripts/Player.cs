using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

    protected override void Update() {
        GetInput();
        transform.rotation = Quaternion.Euler(0, 0, 0);
        base.Update();
    }

    /**
    * Moves the Player based on arrow key input. Or rather, sets the
    * velocity field of the player so that Move() can update the player's 
    * location properly.
    */
    private void GetInput() {
        velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            directionFacing = NORTH;
            velocity += Vector2.up;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            directionFacing = EAST;
            velocity += Vector2.right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            directionFacing = SOUTH;
            velocity += Vector2.down;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            directionFacing = WEST;
            velocity += Vector2.left;
        }
    }
}
