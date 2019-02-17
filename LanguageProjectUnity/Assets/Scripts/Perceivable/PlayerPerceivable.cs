using UnityEngine;

public class PlayerPerceivable : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        // base.SendPercept(npc);
        if (gameObject.GetComponent<Player>().currentWearObject) {
            npc.ReceivePercept(new Phrase(Expression.POSSESS, Expression.PLAYER, new Phrase(Expression.THE, Expression.CROWN)));
        }
    }
}
