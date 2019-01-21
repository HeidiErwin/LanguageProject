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
        npc.ReceivePercept(
            new Phrase(locked ? Expression.INACTIVE : Expression.ACTIVE, Expression.THE_GREAT_DOOR),
            new Phrase(open ? Expression.OPEN : Expression.CLOSED, Expression.THE_GREAT_DOOR),
            new Phrase(GetArea(), Expression.THE_GREAT_DOOR));
    }

        private Expression GetArea() {
        if (this.area == 0) {
            return new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 7), 1);
        }
        if (this.area == 1) {
            return new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 8), 1);
        }
        if (this.area == 2) {
            return new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 9), 1);
        }
        if (this.area == 3) {
            return new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 10), 1);
        }

        return null;
    }
}
