public class DefaultModel {
    public static Model Make() {
        Model m = new SimpleModel();

        MetaVariable xt0   = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1   = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        MetaVariable xi0   = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xi1   = new MetaVariable(SemanticType.INDIVIDUAL, 1);
        MetaVariable xi2   = new MetaVariable(SemanticType.INDIVIDUAL, 2);
        MetaVariable xp0   = new MetaVariable(SemanticType.PREDICATE, 0);
        MetaVariable xp1   = new MetaVariable(SemanticType.PREDICATE, 1);
        MetaVariable xr20  = new MetaVariable(SemanticType.RELATION_2, 0);
        MetaVariable xitr0 = new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0);
        MetaVariable xtf10 = new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0);
        MetaVariable xqp0  = new MetaVariable(SemanticType.QUANTIFIER_PHRASE, 0);

        Expression not = Expression.NOT;

        // verum and falsum
        m.Add(Expression.VERUM);
        m.Add(new Phrase(not, Expression.FALSUM));

        // COMMON-KNOWLEDGE
        m.Add(new Phrase(Expression.NEAR, Expression.SELF, Expression.SELF));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.BLACK)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.RED)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.GREEN)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.BLUE)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.CYAN)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.MAGENTA)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.YELLOW)));
        // m.Add(new Phrase(Expression.COLOR, new Phrase(Expression.NOMINALIZE, Expression.WHITE)));

        // SELF-KNOWLEDGE
        m.Add(new Phrase(Expression.PERSON, Expression.SELF));
        m.Add(new Phrase(Expression.ACTIVE, Expression.SELF));

        // ACTION RULES
        m.Add(new ActionRule(
            Expression.VERUM,
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.NEAR, Expression.SELF, xi0)),
            new ExpressionPattern(Expression.NEAR, Expression.SELF, xi0)));

        m.Add(new ActionRule(
            new ExpressionPattern(Expression.NEAR, Expression.SELF, xi0),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.BELIEVE, xi0, xt0)),
            new ExpressionPattern(Expression.BELIEVE, xi0, xt0)));

        m.Add(new ActionRule(
            new ExpressionPattern(Expression.NEAR, Expression.SELF, xi0),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.INTEND, xi0, xt0)),
            new ExpressionPattern(Expression.INTEND, xi0, xt0)));

        // SUBSTITUTION RULES        

        // S |- believes(self, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, Expression.SELF, xt0)},
            false));

        // believes(x, ~S) |- ~believes(x, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, xi0, new ExpressionPattern(not, xt0))},
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.BELIEVE, xi0, xt0))},
            false));

        // // ~S |- ~believes(self, S)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.NOT, xt0)},
        //     new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BELIEVE, Expression.SELF, xt0))},
        //     false));

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

        // // active(x) |- ~inactive(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.ACTIVE, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.INACTIVE, xi0))}));

        // // person(x) |- animal(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.ANIMAL, xi0))}));

        // // cow(x) |- animal(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.COW, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.ANIMAL, xi0))}));

        // // person(x) |- ~cow(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.COW, xi0))}));

        // // animal(x) |- ~lamp(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.LAMP, xi0))}));

        // // animal(x) |- ~fountain(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
        //     new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(Expression.FOUNTAIN, xi0))}));

        // // color(nominalize(C)), color(nominalize(D)), D(x) |- ~C(x), nominalize(C) = nominalize(D)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.COLOR, new ExpressionPattern(Expression.NOMINALIZE, xp0)),
        //         new ExpressionPattern(Expression.COLOR, new ExpressionPattern(Expression.NOMINALIZE, xp1)),
        //         new ExpressionPattern(xp1, xi0)},
        //     new IPattern[]{
        //         new ExpressionPattern(not, new ExpressionPattern(xp0, xi0)),
        //         new ExpressionPattern(Expression.IDENTITY, new ExpressionPattern(Expression.NOMINALIZE, xp0), new ExpressionPattern(Expression.NOMINALIZE, xp1))}));

        // // R(x, x) |- itself(R, x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xr20, xi0, xi0)},
        //     new IPattern[]{new ExpressionPattern(Expression.ITSELF, xr20, xi0)}));

        // // itself(R, x) |- R(x, x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.ITSELF, xr20, xi0)},
        //     new IPattern[]{new ExpressionPattern(xr20, xi0, xi0)}));

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

        // |- F(the(F))
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{new ExpressionPattern(xp0, new ExpressionPattern(Expression.THE, xp0))}));

        // reflexivity for identity
        // |- x = x
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi0)}));

        // substitution of identiticals
        // [i = j] F(i) |- F(j)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(xp0, xi1)}));

        // F(x), G(x) |- some(F, G)
        // not transposable to save time
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.SOME, xp0, xp1)},
            false));

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
        


        // // antisymmetry for contained_within
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1)},
        //     new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi0))},
        //     false));

        // // transitivity for contained_within
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1),
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi2)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi2)}));

        // uniqueness of king
        // king(i), king(j) |- i = j
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.IDENTITY, xi0, xi1))},
            new IPattern[]{
                new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.KING, xi0)),
                new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.KING, xi1))},
            false));

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

        // perceive(self, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERCEIVE, Expression.SELF, xt0)},
            new IPattern[]{xt0}));

        // make(x, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAKE, Expression.SELF, xt0)},
            new IPattern[]{xt0}));

        // believes(x, S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, xi0, xt0)},
            new IPattern[]{xt0}));

        // F(x) |- exists(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.EXISTS, xi0)}, false));

        return m;
    }
}
