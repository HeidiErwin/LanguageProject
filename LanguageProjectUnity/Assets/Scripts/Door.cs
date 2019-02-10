using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Perceivable {

    public bool open = false;
    public bool locked = false;
    [SerializeField] int area;

    [SerializeField] Sprite openDoorSprite;
    [SerializeField] Sprite closedDoorSprite;
    [SerializeField] GameObject key; // item needed to unlock door

    public void Interact() {
        if (open) {
            Close();
        } else if (!open && !locked){
            Open();
        }
    }

    public void Open() {
        GetComponent<SpriteRenderer>().sprite = openDoorSprite;
        open = true;
    }

    public void Close() {
        GetComponent<SpriteRenderer>().sprite = closedDoorSprite;
        open = false;
    }

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);
        // Expression param = new Phrase(Expression.THE, Expression.DOOR);
        npc.ReceivePercept(
            new Phrase(locked ? Expression.INACTIVE : Expression.ACTIVE, param),
            new Phrase(open ? Expression.OPEN : Expression.CLOSED, param));
    }
}
