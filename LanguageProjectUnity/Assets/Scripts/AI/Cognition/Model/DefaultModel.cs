public class DefaultModel {
    public static Model Make() {
        Model m = new SimpleModel();

        // SPECIAL EVALUATION RULES
        m.Add(EvaluationRule.NOT);
        m.Add(EvaluationRule.EVERY);

        // DEFAULT EVALUATION RULES
        m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_1);
        m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_2);
        m.Add(EvaluationRule.DEFAULT_DETERMINER);
        m.Add(EvaluationRule.DEFAULT_PREDICATE);
        m.Add(EvaluationRule.DEFAULT_RELATION_2);
        m.Add(EvaluationRule.DEFAULT_RELATION_3);

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xp0 = new MetaVariable(SemanticType.PREDICATE, 0);

        Expression not = Expression.NOT;

        // SUBSTITUTION RULES
        // S |- T(S)
        m.Add(new SubstitutionRule(xt0, new ExpressionPattern(Expression.TRUE, xt0), EntailmentContext.Downward));
        // T(S) |- S
        m.Add(new SubstitutionRule(new ExpressionPattern(Expression.TRUE, xt0), xt0, EntailmentContext.Upward));
        // S |- ~~S
        m.Add(new SubstitutionRule(xt0, new ExpressionPattern(not, new ExpressionPattern(not, xt0)), EntailmentContext.Downward));
        // ~~S |- S
        m.Add(new SubstitutionRule(new ExpressionPattern(not, new ExpressionPattern(not, xt0)), xt0, EntailmentContext.Upward));
        // i.[F(i)] |- a(F)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            xi0,
            new ExpressionPattern(Expression.A, xp0),
            EntailmentContext.Downward));
        // every(F) |- i.[F(i)]
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new ExpressionPattern(Expression.EVERY, xp0),
            xi0,
            EntailmentContext.Downward));
        // cow |- animal
        m.Add(new SubstitutionRule(Expression.COW, Expression.ANIMAL));
        // person |- animal
        m.Add(new SubstitutionRule(Expression.PERSON, Expression.ANIMAL));

        // INFERENCE RULES
        // A, B |- A & B
        m.Add(new InferenceRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            EntailmentContext.Downward));
        // A |- A v B
        m.Add(new InferenceRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));
        // B |- A v B
        m.Add(new InferenceRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));
        // |- F(every(F))
        m.Add(new InferenceRule(
            new IPattern[]{},
            new IPattern[]{new ExpressionPattern(xp0, new ExpressionPattern(Expression.EVERY, xp0))}));
        return m;
    }
}
