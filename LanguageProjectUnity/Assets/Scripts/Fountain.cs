using UnityEngine;

public class Fountain : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);

        // Expression param = new Parameter(SemanticType.INDIVIDUAL, id);

        npc.ReceivePercept(
            new Phrase(Expression.FOUNTAIN, param),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), param));
    }
}
