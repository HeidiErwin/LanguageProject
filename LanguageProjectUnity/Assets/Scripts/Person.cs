using UnityEngine;

public class Person : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPerceptualReport(NPC npc) {
        base.SendPerceptualReport(npc);
        Expression param = new Parameter(SemanticType.INDIVIDUAL, this.id);

        npc.ReceivePerceptualReport(
            new Phrase(Expression.PERSON, param),
            new Phrase((isActive ? Expression.ACTIVE : Expression.INACTIVE), param),
            new Phrase(Expression.CONTAINED_WITHIN, param, GetArea()));
    }

    private Expression GetArea() {
        if (this.area == 0) {
            return new Parameter(SemanticType.INDIVIDUAL, 7);
        }
        if (this.area == 1) {
            return new Parameter(SemanticType.INDIVIDUAL, 8);
        }
        if (this.area == 2) {
            return new Parameter(SemanticType.INDIVIDUAL, 9);
        }
        if (this.area == 3) {
            return new Parameter(SemanticType.INDIVIDUAL, 10);
        }

        return null;
    }
}
