using System;
using UnityEngine;

public class Jester : NPC {
    void Start() {
        base.Start();
        CustomModels.AddWoodcutterModel(this.model);
        // Substitution Rules

        // Action Rules
        model.Add(new ActionRule(
            new Phrase(Expression.POSSESS, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))),
            new Phrase(Expression.WOULD, new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))),
            new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))));

        model.Add(new ActionRule(
            new Phrase(Expression.POSSESS, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))),
            new Phrase(Expression.WOULD, new Phrase(Expression.NOT, new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))))),
            new Phrase(Expression.NOT, new Phrase(Expression.WEAR, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN))))));

        model.Add(new ActionRule(
            new ExpressionPattern(Expression.AND,
                new ExpressionPattern(Expression.NEAR, Expression.SELF, new MetaVariable(SemanticType.INDIVIDUAL, 0)),
                new Phrase(Expression.POSSESS, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.POSSESS,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))),
            new ExpressionPattern(Expression.POSSESS,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))));

        // particular beliefs
        model.Add(new Phrase(Expression.PERCEIVE, Expression.SELF, new Phrase(Expression.POSSESS, Expression.SELF, new Phrase(Expression.THE, new Phrase(Expression.FAKE, Expression.CROWN)))));
        // model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, new Phrase(Expression.THE, Expression.KING)));
        // self-knowledge

    }
}
