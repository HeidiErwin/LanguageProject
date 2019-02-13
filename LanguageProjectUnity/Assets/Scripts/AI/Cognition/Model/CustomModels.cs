public class CustomModels {
    public static void AddDoorModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD,
                new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)),
            new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN)),
            new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.COW))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.COW))));

        // SUBSTITUTION RULES
        // open(x) |- not(closed(x))
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(
                    Expression.OPEN,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0))},
            new IPattern[]{new ExpressionPattern(
                Expression.NOT,
                new ExpressionPattern(Expression.CLOSED, new MetaVariable(SemanticType.INDIVIDUAL, 0)))}));
    }

    public static void AddWoodcutterModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD,
                new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.KING))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.KING))));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(Expression.CREDIBLE, new Phrase(Expression.THE, Expression.KING)));
    }
}