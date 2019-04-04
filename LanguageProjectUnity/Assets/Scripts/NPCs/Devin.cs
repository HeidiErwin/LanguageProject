using System;
using System.Collections.Generic;
using UnityEngine;

public class Devin : NPC {
    void Start() {
        base.Start();
        this.name = new Phrase(Expression.DEVIN);

        CustomModels.AddVillageModel(this.model);
        
        // ACTION RULES

        // Substitution Rules

        // particular beliefs
        new Phrase(Expression.IDENTITY, Expression.SELF, Expression.DEVIN);
    }
}
