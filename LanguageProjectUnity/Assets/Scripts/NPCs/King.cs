using System;
using UnityEngine;

public class King : NPC {
    void Start() {
        base.Start();
        this.name = new Word(SemanticType.INDIVIDUAL, "kevin");
        CustomModels.AddWoodcutterModel(this.model);
        // SUBSTITUTION RULES

        // ACTION RULES

        // PARTICULAR BELIEFS
        model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, new Phrase(Expression.THE, Expression.KING)));
        model.Add(new Phrase(Expression.POSSESS, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.REAL, Expression.CROWN))));
        // SELF-KNOWLEDGE
    }
}
