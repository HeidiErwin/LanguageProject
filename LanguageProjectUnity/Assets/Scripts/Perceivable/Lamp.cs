using UnityEngine;

public class Lamp : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);
        npc.ReceivePercept(
            new Phrase(Expression.LAMP, param),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), param));
    }
}
