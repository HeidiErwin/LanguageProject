using UnityEngine;

public class Fountain : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);

        npc.ReceivePercept(
            new Phrase(Expression.FOUNTAIN, new Phrase(Expression.THE, Expression.FOUNTAIN)),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), new Phrase(Expression.THE, Expression.FOUNTAIN)));
    }
}
