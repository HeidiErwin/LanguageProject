using System;
using System.Collections.Generic;
using UnityEngine;

public class Woodcutter : NPC {
    void Start() {
        base.Start();
        this.name = new Phrase(Expression.THE, new Phrase(Expression.POSSESS, new Phrase(Expression.THE, Expression.LOG), 1));

        CustomModels.AddWoodcutterModel(this.model);
        
        // ACTION RULES
        this.model.Add(new ActionRule(
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.TREE)),
            new Phrase(Expression.WOULD, new Phrase(Expression.EXISTS, new Phrase(Expression.THE, Expression.LOG))),
            new Phrase(Expression.EXISTS, new Phrase(Expression.THE, Expression.LOG))));

        // Substitution Rules
        
        // x has the crown |- x is king
        this.model.Add(new InferenceRule(
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(Expression.POSSESS, new MetaVariable(SemanticType.INDIVIDUAL, 0), new Phrase(Expression.THE, Expression.CROWN)))},
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(Expression.KING, new MetaVariable(SemanticType.INDIVIDUAL, 0)))}));

        // particular beliefs

        // self-knowledge
    }
}
