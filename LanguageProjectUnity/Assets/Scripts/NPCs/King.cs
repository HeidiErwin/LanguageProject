using System;
using UnityEngine;

public class King : NPC {
    void Start() {
        base.Start();
        CustomModels.AddWoodcutterModel(this.model);
        // SUBSTITUTION RULES

        // ACTION RULES

        // PARTICULAR BELIEFS
        model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, new Phrase(Expression.THE, Expression.KING)));

        // SELF-KNOWLEDGE
    }
}
