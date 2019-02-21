using UnityEngine;

public class Cow : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);

        // Expression param = new Parameter(SemanticType.INDIVIDUAL, id);
        Expression theCow = new Phrase(Expression.THE, Expression.COW);

        npc.ReceivePercept(
            new Phrase(Expression.WHITE, theCow),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), theCow));
    }
}
