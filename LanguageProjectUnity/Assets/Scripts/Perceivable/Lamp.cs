using UnityEngine;

public class Lamp : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);
        npc.ReceivePercept(
            new Phrase(Expression.LAMP, new Phrase(Expression.THE, Expression.LAMP)),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), new Phrase(Expression.THE, Expression.LAMP)));
    }
}
