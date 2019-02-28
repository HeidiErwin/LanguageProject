using System.Collections.Generic;

public class DefaultModel {
    public static List<IPattern> BuildList(params IPattern[] args) {
        List<IPattern> list = new List<IPattern>();
        for (int i = 0; i < args.Length; i++) {
            list.Add(args[i]);
        }
        return list;
    }

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

        // COMMON-KNOWLEDGE
        // m.Add(new Phrase(Expression.AT, Expression.SELF, Expression.SELF));
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

        // // ACTION RULES
        // m.Add(new ActionRule(
        //     Expression.VERUM,
        //     new ExpressionPattern(Expression.WOULD,
        //         new ExpressionPattern(Expression.AT, Expression.SELF, xi0)),
        //     new ExpressionPattern(Expression.AT, Expression.SELF, xi0)));

        m.Add(new ActionRule(
            new ExpressionPattern(Expression.AT, Expression.SELF, xi0),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.BELIEVE, xi0, xt0)),
            new ExpressionPattern(Expression.BELIEVE, xi0, xt0)));

        m.Add(new ActionRule(
            new ExpressionPattern(Expression.AT, Expression.SELF, xi0),
            new ExpressionPattern(Expression.WOULD,
                new ExpressionPattern(Expression.INTEND, xi0, xt0)),
            new ExpressionPattern(Expression.INTEND, xi0, xt0)));

        // SUBSTITUTION RULES

        // // |- verum
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(Expression.VERUM)}));

        // // falsum |-
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(Expression.FALSUM)},
        //     new List<IPattern>[]{}));

        // // S |- believes(self, S)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.BELIEVE, Expression.SELF, xt0))},
        //     false));

        // // believes(x, ~S) |- ~believes(x, S)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.BELIEVE, xi0, new ExpressionPattern(not, xt0)))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, new ExpressionPattern(Expression.BELIEVE, xi0, xt0)))},
        //     false));

        // // S |- T(S)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.TRUE, xt0))},
        //     false));

        // S |- ~~S
        m.Add(new SubstitutionRule(
            new List<IPattern>[]{BuildList(xt0)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(not, new ExpressionPattern(not, xt0)))},
            false));

        // // GEACH RULES
        // // t -> t
        // // !F(x) |- G(!, F, x)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0)))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.GEACH_TF1, xtf10, xp0, xi0))},
        //     false));
        
        // // t, t -> t
        // // C(F(x), H(x)) |- G(C, F, H, x)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0)))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.GEACH_TF1, xtf10, xp0, xi0))},
        //     false));

        // // |- F(the(F))
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, new ExpressionPattern(Expression.THE, xp0)))}));

        // // reflexivity for at
        // // |- at(x, x)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.AT, xi0, xi0))}));

        // // reflexivity for identity
        // // |- x = x
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.IDENTITY, xi0, xi0))}));

        // // substitution of identiticals
        // // [i = j] F(i) |- F(j)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi0))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi1))}));

        // // F(x), G(x) |- some(F, G)
        // // not transposable to save time
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.SOME, xp0, xp1))},
        //     false));

        // // uniqueness of king
        // // king(i), king(j) |- i = j
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{
        //         BuildList(
        //             new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.IDENTITY, xi0, xi1)))},
        //     new List<IPattern>[]{
        //         BuildList(
        //         new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.KING, xi0)),
        //         new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.KING, xi1)))},
        //     false));
        
        // // A, B |- A & B
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0, xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.AND, xt0, xt1))}));

        // // A & B |- A
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.AND, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // A & B |- B
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.AND, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt1)}));

        // // A v B |- A, B
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.OR, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt0, xt1)}));

        // // A |- A v B
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.OR, xt0, xt1))}));

        // // B |- A v B
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.OR, xt0, xt1))}));

        // // A, B |- equivalent(A, B)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0, xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.EQUIVALENT, xt0, xt1))},
        //     false));

        // // ~A, ~B |- equivalent(A, B)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, xt0), new ExpressionPattern(not, xt1))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.EQUIVALENT, xt0, xt1))},
        //     false));

        // // A, ~B |- ~equivalent(A, B)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(xt0, new ExpressionPattern(not, xt1))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.EQUIVALENT, xt0, xt1)))},
        //     false));

        // // ~A, B |- ~equivalent(A, B)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, xt0), xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.EQUIVALENT, xt0, xt1)))},
        //     false));

        // perceive(self, S) | normal(S) |- S
        m.Add(new SubstitutionRule(
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(Expression.PERCEIVE, Expression.SELF, xt0)),
                BuildList(new ExpressionPattern(Expression.NORMAL, xt0))},
            new List<IPattern>[]{BuildList(xt0)}));

        // // make(x, S) |- S
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.MAKE, Expression.SELF, xt0))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // believe(x, S) |- S
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.BELIEVE, xi0, xt0))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // F(x) |- exists(x)
        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi0))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(Expression.EXISTS, xi0))}, false));

        return m;
    }
}
