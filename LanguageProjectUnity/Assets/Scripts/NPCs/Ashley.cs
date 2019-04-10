using System;
using System.Collections.Generic;
using UnityEngine;

public class Ashley : NPC {
    void Start() {
        base.Start();
        this.name = new Phrase(Expression.ASHLEY);

        CustomModels.AddVillageModel(this.model);
        
        // ACTION RULES

        // Substitution Rules

        // particular beliefs
        model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, Expression.ASHLEY));

        // utilities
        
    }
}
