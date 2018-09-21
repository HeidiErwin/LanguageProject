using UnityEngine;

public class Fountain : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPerceptualReport(NPC npc) {
        base.SendPerceptualReport(npc);
        Expression param = new Parameter(SemanticType.INDIVIDUAL, this.id);

        npc.ReceivePerceptualReport(
            new Phrase(Expression.FOUNTAIN, param),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), param),
            new Phrase(GetArea(), param));
    }

    private Expression GetArea() {
        if (this.area == 0) {
            return Expression.IN_RED_AREA;
        }
        if (this.area == 1) {
            return Expression.IN_BLUE_AREA;
        }
        if (this.area == 2) {
            return Expression.IN_YELLOW_AREA;
        }
        if (this.area == 3) {
            return Expression.IN_GREEN_AREA;
        }

        return null;
    }
}
