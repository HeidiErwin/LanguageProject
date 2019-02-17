using UnityEngine;

public class Crown : Perceivable {
    [SerializeField] bool isActive;
    [SerializeField] int area;

    public override void SendPercept(NPC npc) {
        // base.SendPercept(npc);

        NPC wearer = this.gameObject.transform.parent.gameObject.GetComponent<NPC>();

        npc.ReceivePercept(new Phrase(Expression.POSSESS, wearer.name, new Phrase(Expression.THE, Expression.CROWN)));
    }
}
