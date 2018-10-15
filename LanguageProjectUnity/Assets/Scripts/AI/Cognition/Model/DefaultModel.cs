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

        // It's not the case that some bird flies
        // ~some(bird, flies)
        // no bird flies

        // reflexivity for identity
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi0)}));

        // // symmetry for identity (commented out so it doesn't loop)
        // m.Add(new SubstituionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi1, xi0)}));

        // transitivity for identity
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi1),
                new ExpressionPattern(Expression.IDENTITY, xi1, xi2)
            },
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi2)
            }));

        // [F(x)] G(x) |- some(F, G) TODO determine correct proof semantics
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(xp1, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.SOME, xp0, xp1)}));

        // [F(x)] every(F, G) |- G(x) TODO determine correct proof semantics
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.EVERY, xp0, xp1)},
            new IPattern[]{new ExpressionPattern(xp1, xi0)}));

        // antisymmetry for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi0))},
            EntailmentContext.Downward));

        // transitivity for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1),
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi2)
            },
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi2)}));

        // // reflexivity for overlaps_with
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi0)
        //     }
        // ));

        // symmetry for overlaps_with
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi1)
        //     },
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.OVERLAPS_WITH, xi1, xi0)
        //     }
        // ));
        // BUG: causes loops

        // // cow |- animal
        // m.Add(new SubstitutionRule(Expression.COW, Expression.ANIMAL));
        // // person |- animal
        // m.Add(new SubstitutionRule(Expression.PERSON, Expression.ANIMAL));
        return m;
    }
}
