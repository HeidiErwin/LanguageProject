public class DefaultModel {
    public static Model Make() {
        Model m = new SimpleModel();

        // // SPECIAL EVALUATION RULES
        // m.Add(EvaluationRule.NOT);
        // m.Add(EvaluationRule.EVERY);

        // // DEFAULT EVALUATION RULES
        // m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_1);
        // m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_2);
        // m.Add(EvaluationRule.DEFAULT_DETERMINER);
        // m.Add(EvaluationRule.DEFAULT_PREDICATE);
        // m.Add(EvaluationRule.DEFAULT_RELATION_2);
        // m.Add(EvaluationRule.DEFAULT_RELATION_3);

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xi1 = new MetaVariable(SemanticType.INDIVIDUAL, 1);
        MetaVariable xi2 = new MetaVariable(SemanticType.INDIVIDUAL, 2);
        MetaVariable xp0 = new MetaVariable(SemanticType.PREDICATE, 0);
        MetaVariable xp1 = new MetaVariable(SemanticType.PREDICATE, 1);

        Expression not = Expression.NOT;

        // SUBSTITUTION RULES
        // S |- T(S)
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.TRUE, xt0)},
            EntailmentContext.Downward));

        // T(S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.TRUE, xt0)},
            new IPattern[]{xt0},
            EntailmentContext.Upward));

        // S |- ~~S
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(not, xt0))},
            EntailmentContext.Downward));

        // ~~S |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(not, xt0))},
            new IPattern[]{xt0},
            EntailmentContext.Upward));

        // A, B |- A & B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            EntailmentContext.Downward));

        // // A & B |- A
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
        //     new IPattern[]{xt0},
        //     EntailmentContext.Upward));

        // // A & B |- B
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
        //     new IPattern[]{xt1},
        //     EntailmentContext.Upward));

        // // A v B |- A, B
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
        //     new IPattern[]{xt0, xt1},
        //     EntailmentContext.Upward));

        // A |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));

        // B |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));

        // reflexivity for identity
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi0)}));

        // // symmetry for identity
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi1, xi0)}));

        // // transitivity for identity
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.IDENTITY, xi0, xi1),
        //         new ExpressionPattern(Expression.IDENTITY, xi1, xi2)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.IDENTITY, xi0, xi2)}));

        // substitution of identiticals
        // [i = j] i |- j
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
        //     new IPattern[]{xi0},
        //     new IPattern[]{xi1}));

        // F(x), G(x) |- some(F, G)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.SOME, xp0, xp1)}));

        // // [i != j, F(i), F(j)] G(i), G(j) |- Two(F, G)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.IDENTITY, xi0, xi1)),
        //         new ExpressionPattern(xp0, xi0),
        //         new ExpressionPattern(xp0, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(xp1, xi0),
        //         new ExpressionPattern(xp1, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.TWO, xp0, xp1)}));

        // // every(F, G), F(x) |- G(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.EVERY, xp0, xp1),
        //         new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(xp1, xi0)}));

        // // |- every(F, F)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{new ExpressionPattern(Expression.EVERY, xp0, xp0)}));

        // // antisymmetry for contained_within
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1)},
        //     new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi0))},
        //     EntailmentContext.Downward));

        // // transitivity for contained_within
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1),
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi2)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi2)}));

        // uniqueness of king
        // king(i), king(j) |- i = j
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.KING, xi0),
        //         new ExpressionPattern(Expression.KING, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.IDENTITY, xi0, xi1)}));

        // // reflexivity for overlaps_with
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi0)}));

        // // symmetry for overlaps_with
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi1, xi0)}));

        // cow(x) |- animal(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.COW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.ANIMAL, xi0)}));

        // person(x) |- animal(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.ANIMAL, xi0)}));

        // // F |- exists
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{xp0},
        //     new IPattern[]{Expression.EXISTS}));

        // // F(x) |- exists(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(Expression.EXISTS, xi0)}));

        return m;
    }
}
