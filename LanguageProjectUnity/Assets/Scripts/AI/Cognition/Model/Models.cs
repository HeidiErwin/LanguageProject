// ALL PREDICATES
// m.Add(new Phrase(Expression.FOUNTAIN, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.LAMP, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.ACTIVE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.INACTIVE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.KING, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.YELLOW, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.GREEN, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.BLUE, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.RED, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_YOUR_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_YELLOW_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_GREEN_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_BLUE_AREA, new Phrase(Expression.XXX, Expression.YYY)));
// m.Add(new Phrase(Expression.IN_RED_AREA, new Phrase(Expression.XXX, Expression.YYY)));

public class Models {
    public static Model BobModel() {
        Model m = new SimpleModel();
        // rules
        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);

        SubstitutionRule tRule = new SubstitutionRule(xt0, new ExpressionPattern(Expression.TRUE, xt0), EntailmentContext.Downward);
        SubstitutionRule tRule2 = new SubstitutionRule(new ExpressionPattern(Expression.TRUE, xt0), xt0, EntailmentContext.Upward);

        SubstitutionRule dni = new SubstitutionRule(xt0,
            new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.NOT, xt0)),
            EntailmentContext.Downward);

        SubstitutionRule activeNotInactive =
            new SubstitutionRule(new ExpressionPattern(Expression.ACTIVE, xi0),
                new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.INACTIVE, xi0)),
                EntailmentContext.Downward);

        SubstitutionRule inactiveNotActive =
            new SubstitutionRule(new ExpressionPattern(Expression.INACTIVE, xi0),
                new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.ACTIVE, xi0)),
                EntailmentContext.Downward);

        SubstitutionRule cowMeansAnimal =
            new SubstitutionRule(Expression.COW, Expression.ANIMAL);

        InferenceRule andIntroduction = new InferenceRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            EntailmentContext.Downward);

        InferenceRule orIntroduction1 = new InferenceRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward);

        InferenceRule orIntroduction2 = new InferenceRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward);

        SubstitutionRule aIntroduction = new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(new MetaVariable(SemanticType.PREDICATE, 0), new MetaVariable(SemanticType.INDIVIDUAL, 0))},
            new MetaVariable(SemanticType.INDIVIDUAL, 0),
            new ExpressionPattern(Expression.A, new MetaVariable(SemanticType.PREDICATE, 0)),
            EntailmentContext.Downward);

        m.Add(tRule);
        m.Add(tRule2);
        m.Add(dni);
        m.Add(cowMeansAnimal);
        m.Add(activeNotInactive);
        m.Add(inactiveNotActive);
        m.Add(andIntroduction);
        m.Add(orIntroduction1);
        m.Add(orIntroduction2);
        m.Add(aIntroduction);

        m.Add(EvaluationRule.NOT);
        m.Add(EvaluationRule.DEFAULT_PREDICATE);
        m.Add(EvaluationRule.DEFAULT_DETERMINER);

        // Bob's beliefs

        // true of Bob
        m.Add(new Phrase(Expression.KING, Expression.BOB));
        m.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        m.Add(new Phrase(Expression.IN_YOUR_AREA, Expression.BOB));
        m.Add(new Phrase(Expression.IN_RED_AREA, Expression.BOB));
        m.Add(new Phrase(Expression.ANIMAL, Expression.BOB));

        // false of Bob
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.YELLOW, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.GREEN, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.BLUE, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.RED, Expression.BOB)));

        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IN_YELLOW_AREA, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IN_GREEN_AREA, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IN_BLUE_AREA, Expression.BOB)));

        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.FOUNTAIN, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.LAMP, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.KING, Expression.BOB)));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.COW, Expression.BOB)));

        // true of Evan
        // m.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        // m.Add(new Phrase(Expression.IN_GREEN_AREA, Expression.EVAN));

        // true of first lamp
        // 
        // // TODO the rest of this again

        return m;
    }

    public static Model EvanModel() {
        Model m = new SimpleModel();

        // TODO: the rest of this again
        return m;
    }
}
