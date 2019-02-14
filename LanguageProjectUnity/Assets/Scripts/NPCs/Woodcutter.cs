using System;
using UnityEngine;

public class Woodcutter : NPC {
    void Start() {
        base.Start();
        CustomModels.AddWoodcutterModel(this.model);
        
        // ACTION RULES
        this.model.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.TREE)),
            new Phrase(Expression.WOULD, new Phrase(Expression.EXISTS, new Phrase(Expression.THE, Expression.LOG))),
            new Phrase(Expression.EXISTS, new Phrase(Expression.THE, Expression.LOG))));

        // Substitution Rules

        

        // particular beliefs

        // self-knowledge
    }
}
