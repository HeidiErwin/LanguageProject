using System.Collections.Generic;

public class CustomModels {
    public static void AddDoorModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD,
                new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.AT, Expression.SELF, Expression.BOB)),
            new Phrase(Expression.AT, Expression.SELF, Expression.BOB)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.AT, Expression.SELF, Expression.EVAN)),
            new Phrase(Expression.AT, Expression.SELF, Expression.EVAN)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.COW))),
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.COW))));

        // SUBSTITUTION RULES
        // open(x) |- not(closed(x))
        m.Add(new SubstitutionRule(
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(
                    Expression.OPEN,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0)))},
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(
                Expression.NOT,
                new ExpressionPattern(Expression.CLOSED, new MetaVariable(SemanticType.INDIVIDUAL, 0))))}));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.BOB, Expression.EVAN)));
    }

    public static void AddWoodcutterModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD,
                new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.KING))),
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.KING))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD,
                new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.TREE))),
            new Phrase(Expression.AT, Expression.SELF, new Phrase(Expression.THE, Expression.TREE))));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(Expression.CREDIBLE, new Phrase(Expression.THE, Expression.KING)));
    }

    public static void AddFPExperimentModel(Model m) {
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);

        // ACTION RULES
        m.Add(new ActionRule(
            new ExpressionPattern(Expression.EQUIVALENT,
                new Phrase(Expression.IN_THE_ROOM, Expression.SELF),
                new ExpressionPattern(Expression.IN_THE_ROOM, xi0)),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.AT, Expression.SELF, xi0)),
            new ExpressionPattern(Expression.AT, Expression.SELF, xi0)));

        m.Add(new ActionRule(
            new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.AT, Expression.SELF, xi0)),
            new ExpressionPattern(Expression.AT, Expression.SELF, xi0)));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(Expression.PERCEIVE, Expression.SELF, new Phrase(Expression.IN_THE_ROOM, Expression.BOB)));
        m.Add(new Phrase(Expression.PERCEIVE, Expression.SELF, new Phrase(Expression.IN_THE_ROOM, Expression.EVAN)));
        m.Add(new Phrase(Expression.PERCEIVE, Expression.SELF, new Phrase(Expression.IN_THE_ROOM, new Phrase(Expression.THE, Expression.DOOR))));
        m.Add(new Phrase(Expression.PERCEIVE, Expression.SELF,
            new Phrase(Expression.NOT, new Phrase(Expression.IN_THE_ROOM, Expression.GOAL))));
        m.Add(new Phrase(Expression.CREDIBLE, Expression.PLAYER));
    }
}
