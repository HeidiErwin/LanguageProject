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
        MetaVariable xr20 = new MetaVariable(SemanticType.RELATION_2, 0);
        MetaVariable xitr0 = new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0);
        MetaVariable xtf10 = new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0);
        MetaVariable xqp0 = new MetaVariable(SemanticType.QUANTIFIER_PHRASE, 0);

        Expression not = Expression.NOT;

        // verum and falsum
        m.Add(Expression.VERUM);
        m.Add(new Phrase(not, Expression.FALSUM));

        // COMMON-KNOWLEDGE
        m.Add(new Phrase(Expression.NEAR, Expression.SELF, Expression.SELF));

        // SELF-KNOWLEDGE
        m.Add(new Phrase(Expression.PERSON, Expression.SELF));
        m.Add(new Phrase(Expression.ACTIVE, Expression.SELF));

        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        // SUBSTITUTION RULES
        
        // perceive(self, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERCEIVE, Expression.SELF, xt0)},
            new IPattern[]{xt0}, false));

        // believes(x, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, xi0, xt0)},
            new IPattern[]{xt0}, false));

        // make(x, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAKE, Expression.SELF, xt0)},
            new IPattern[]{xt0}, false));
        
        // S |- believes(self, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, Expression.SELF, xt0)},
            false));

        // S |- T(S)
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.TRUE, xt0)},
            false));

        // S |- ~~S
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(not, xt0))},
            false));

        // R(x, x) |- itself(R, x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xr20, xi0, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.ITSELF, xr20, xi0)}));

        // itself(R, x) |- R(x, x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.ITSELF, xr20, xi0)},
            new IPattern[]{new ExpressionPattern(xr20, xi0, xi0)}));

        // GEACH RULES
        // t -> t
        // !F(x) |- G(!, F, x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0))},
            new IPattern[]{new ExpressionPattern(Expression.GEACH_TF1, xtf10, xp0, xi0)},
            false));
        
        // t, t -> t
        // C(F(x), H(x)) |- G(C, F, H, x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0))},
            new IPattern[]{new ExpressionPattern(Expression.GEACH_TF1, xtf10, xp0, xi0)},
            false));

        // // Q(R(x, _)) |- G(Q, R, x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xqp0, new ExpressionPattern(xr20, xi0))},
        //     new IPattern[]{new ExpressionPattern(xqp0, xr20, xi0)}));

        // A, B |- A & B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)}));

        // A & B |- A
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            new IPattern[]{xt0}));

        // A & B |- B
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            new IPattern[]{xt1}));

        // A v B |- A, B
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            new IPattern[]{xt0, xt1}));

        // A |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)}));

        // B |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)}));

        // // reflexivity for identity
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
        // the rule if subsentential substitution was a thing
        // the cheat for now
        // [i = j] F(i) |- F(j)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(xp0, xi1)}));

        // [i = j] R(i, k) |- R(j, k)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xr20, xi0, xi2)},
            new IPattern[]{new ExpressionPattern(xr20, xi1, xi2)}));

        // [i = j] R(k, i) |- R(k, j)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xr20, xi2, xi0)},
            new IPattern[]{new ExpressionPattern(xr20, xi2, xi1)}));

        // [i = j] A(i, S) |- A(j, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xitr0, xi0, xt0)},
            new IPattern[]{new ExpressionPattern(xitr0, xi1, xt1)}));

        // // F(x), G(x) |- some(F, G)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0)},
        //     new IPattern[]{new ExpressionPattern(Expression.SOME, xp0, xp1)}));

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

        // // all(F, G), F(x) |- G(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.ALL, xp0, xp1),
        //         new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(xp1, xi0)}));

        // // |- all(F, F)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{new ExpressionPattern(Expression.ALL, xp0, xp0)}));
        
        // F(x) |- exists(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(Expression.EXISTS, xi0)}, false));

        // antisymmetry for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi0))},
            false));

        // transitivity for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1),
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi2)},
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi2)}));

        // uniqueness of king
        // king(i), king(j) |- i = j
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.KING, xi0),
                new ExpressionPattern(Expression.KING, xi1)},
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi1)}));

        // reflexivity for overlaps_with
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi0)}));

        // symmetry for overlaps_with
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi1)},
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi1, xi0)}));

        return m;
    }
}
