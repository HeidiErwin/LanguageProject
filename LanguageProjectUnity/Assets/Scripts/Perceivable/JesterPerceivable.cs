using UnityEngine;

public class JesterPerceivable : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        // base.SendPercept(npc);
        if (gameObject.GetComponentInChildren<Crown>()) {
            npc.ReceivePercept(new Phrase(Expression.POSSESS, new Word(SemanticType.INDIVIDUAL, "the_jester"), new Phrase(Expression.THE, Expression.CROWN)));
        }
    }
}
