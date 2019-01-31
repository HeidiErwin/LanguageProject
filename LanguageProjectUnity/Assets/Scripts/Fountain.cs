using UnityEngine;

public class Fountain : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        base.SendPercept(npc);

        // Expression param = new Parameter(SemanticType.INDIVIDUAL, id);

        npc.ReceivePercept(
            new Phrase(Expression.FOUNTAIN, param),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), param),
            new Phrase(GetArea(), param));
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
