using System;
using UnityEngine;

public class Jester : NPC {
    void Start() {
        base.Start();
        CustomModels.AddWoodcutterModel(this.model);
        // Substitution Rules

        // Action Rules

        // particular beliefs
        // model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, new Phrase(Expression.THE, Expression.KING)));
        // self-knowledge
    }
}
