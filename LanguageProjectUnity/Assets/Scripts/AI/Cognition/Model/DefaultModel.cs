using System.Collections.Generic;
using static Expression;

public class DefaultModel {
    public static List<IPattern> BuildList(params IPattern[] args) {
        List<IPattern> list = new List<IPattern>();
        for (int i = 0; i < args.Length; i++) {
            list.Add(args[i]);
        }
        return list;
    }

    public static Model Make() {
        return Make(true);
    }

    public static Model Make(bool isSimple) {
        Model m = isSimple ? (Model) new SimpleModel() : (Model) new PrefixModel();

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

        Expression not = NOT;

        // COMMON-KNOWLEDGE
        // m.Add(new Phrase(AT, SELF, SELF));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, BLACK)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, RED)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, GREEN)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, BLUE)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, CYAN)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, MAGENTA)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, YELLOW)));
        // m.Add(new Phrase(COLOR, new Phrase(NOMINALIZE, WHITE)));

        // SELF-KNOWLEDGE
        m.Add(new Phrase(PERSON, SELF));
        m.Add(new Phrase(ACTIVE, SELF));

        // SUBSTITUTION RULES

        // |- verum
        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{BuildList(VERUM)}));

        // falsum |-
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(FALSUM)},
            new List<IPattern>[]{}));

        // // S |- believes(self, S)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(xt0)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(BELIEVE, SELF, xt0))},
        //     false));

        // // believes(x, ~S) |- ~believes(x, S)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(BELIEVE, xi0, new ExpressionPattern(not, xt0)))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, new ExpressionPattern(BELIEVE, xi0, xt0)))},
        //     false));
        
        // better(A, B) |- prefers(self, A, B)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(BETTER, xt0, xt1))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(PREFER, SELF, xt0, xt1))},
            false));

        // ~better(A, B) |- ~prefers(self, A, B)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(BETTER, xt0, xt1)))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(PREFER, SELF, xt0, xt1)))},
            false));

        // S |- T(S)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(xt0)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(TRUE, xt0))},
            false));

        // ~S |- ~T(S)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, xt0))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(TRUE, xt0)))},
            false));

        // S |- ~~S
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(xt0)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(not, new ExpressionPattern(not, xt0)))},
            false));

        // GEACH RULES
        // t -> t
        // !F(x) |- G(!, F, x)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0)))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(GEACH_TF1, xtf10, xp0, xi0))},
            false));
        
        // // t, t -> t
        // // C(F(x), H(x)) |- G(C, F, H, x)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0)))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(GEACH_TF1, xtf10, xp0, xi0))},
        //     false));

        // // |- F(the(F))
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, new ExpressionPattern(THE, xp0)))}));

        // // reflexivity for at
        // // |- at(x, x)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(AT, xi0, xi0))}));

        // reflexivity for identity
        // |- x = x
        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0, xi0))}));

        // @NOTE this is redundant once substitution of identicals is figured out.
        // symmetry for identity
        // x = y |- y = x
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0, xi1))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi1, xi0))}));

        // // CAUSING LOOPS
        // // substitution of identiticals
        // // x = y, F(x) |- F(y)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, xi1),
        //         new ExpressionPattern(xp0, xi0))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi1))}));

        // F(x), G(x) |- some(F, G)
        // not transposable to save time
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(SOME, xp0, xp1))},
            false));

        // // uniqueness of king
        // // king(i), king(j) |- i = j
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(
        //             new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY, xi0, xi1)))},
        //     new List<IPattern>[]{
        //         BuildList(
        //         new ExpressionPattern(NOT, new ExpressionPattern(KING, xi0)),
        //         new ExpressionPattern(NOT, new ExpressionPattern(KING, xi1)))},
        //     false));
        
        MetaVariable xt2 = new MetaVariable(SemanticType.TRUTH_VALUE, 2);
        
        // A, B |- A & B
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(xt0, xt1)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(AND, xt0, xt1))}));

        // // A & B |- A
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(AND, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // A & B |- B
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(AND, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt1)}));

        // // A v B |- A, B
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(OR, xt0, xt1))},
        //     new List<IPattern>[]{BuildList(xt0, xt1)}));

        // A |- A v B
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(xt0)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(OR, xt0, xt1))}));

        // B |- A v B
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(xt1)},
            new List<IPattern>[]{BuildList(new ExpressionPattern(OR, xt0, xt1))}));

        // reflexivity of as_good_as
        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{BuildList(new ExpressionPattern(AS_GOOD_AS, xt0, xt0))}));

        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{BuildList(new ExpressionPattern(INDIFFERENT, xi0, xt0, xt0))}));

        // symmetry of as_good_as
        // as_good_as(A, B) |- as_good_as(B, A)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(AS_GOOD_AS, xt0, xt1))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(AS_GOOD_AS, xt1, xt0))}));

        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(INDIFFERENT, xi0, xt0, xt1))},
            new List<IPattern>[]{BuildList(new ExpressionPattern(INDIFFERENT, xi0, xt1, xt0))}));

        // better(A, B) |- ~better(B, A)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(BETTER, xt0, xt1))},
            new List<IPattern>[]{BuildList(
                new ExpressionPattern(NOT,
                    new ExpressionPattern(BETTER, xt1, xt0)))}));

        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(PREFER, xi0, xt0, xt1))},
            new List<IPattern>[]{BuildList(
                new ExpressionPattern(NOT,
                    new ExpressionPattern(PREFER, xi0, xt1, xt0)))}));

        // as_good_as(A, B) |- ~better(A, B)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(AS_GOOD_AS, xt0, xt1))},
            new List<IPattern>[]{BuildList(
                new ExpressionPattern(NOT,
                    new ExpressionPattern(BETTER, xt0, xt1)))}));

        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(INDIFFERENT, xi0, xt0, xt1))},
            new List<IPattern>[]{BuildList(
                new ExpressionPattern(NOT,
                    new ExpressionPattern(PREFER, xi0, xt0, xt1)))}));

        // // A, B |- equivalent(A, B)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(xt0, xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(EQUIVALENT, xt0, xt1))},
        //     false));

        // // ~A, ~B |- equivalent(A, B)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, xt0), new ExpressionPattern(not, xt1))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(EQUIVALENT, xt0, xt1))},
        //     false));

        // // A, ~B |- ~equivalent(A, B)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(xt0, new ExpressionPattern(not, xt1))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(EQUIVALENT, xt0, xt1)))},
        //     false));

        // // ~A, B |- ~equivalent(A, B)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(not, xt0), xt1)},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(EQUIVALENT, xt0, xt1)))},
        //     false));
        
        // // NATURAL ARITHEMTIC
        // // numeric conversion
        // MetaVariable xii0 = new MetaVariable(SemanticType.INDIVIDUAL_FUNCTION_1, 0);
        
        // // x = succ(0) |- x = 1
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0,
        //         new ExpressionPattern(SUCC, ZERO)))},
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, ONE))}, false));

        // // x = succ(1) |- x = 2
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0,
        //         new ExpressionPattern(SUCC, ONE)))},
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, TWO))}, false));

        // // x = succ(2) |- x = 3
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0,
        //         new ExpressionPattern(SUCC, TWO)))},
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, THREE))}, false));

        // // x = succ(3) |- x = 4
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0,
        //         new ExpressionPattern(SUCC, THREE)))},
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, FOUR))}, false));

        // // x = succ(4) |- x = 5
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0,
        //         new ExpressionPattern(SUCC, FOUR)))},
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(IDENTITY, xi0, FIVE))}, false));

        // |- succ(x) != 0
        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT,
                    new ExpressionPattern(IDENTITY,
                    new ExpressionPattern(SUCC, xi0), ZERO)))}, false));

        // x = y |- succ(x) = succ(y)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0, xi1))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(IDENTITY, new ExpressionPattern(SUCC, xi0),
                        new ExpressionPattern(SUCC, xi1)))}, false));

        // x != y |- succ(x) != succ(y)
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY, xi0, xi1)))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT,
                    new ExpressionPattern(IDENTITY, new ExpressionPattern(SUCC, xi0),
                        new ExpressionPattern(SUCC, xi1))))}, false));

        // x = y |- x = y + 0
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(IDENTITY, xi0, xi1))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(IDENTITY, xi0,
                        new ExpressionPattern(PLUS, xi1, ZERO)))}, false));

        // x != y |- x != y + 0
        m.Add(new InferenceRule(
            new List<IPattern>[]{BuildList(new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY, xi0, xi1)))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY, xi0,
                        new ExpressionPattern(PLUS, xi1, ZERO))))}, false));

        // x = y + z |- succ(x) = y + succ(z)
        m.Add(new InferenceRule(
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(IDENTITY, xi0,
                    new ExpressionPattern(PLUS, xi1, xi2)))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(IDENTITY,
                    new ExpressionPattern(SUCC, xi0),
                    new ExpressionPattern(PLUS, xi1, new ExpressionPattern(SUCC, xi2))))}, false));

        // x != y + z |- succ(x) != y + succ(z)
        m.Add(new InferenceRule(
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY, xi0,
                    new ExpressionPattern(PLUS, xi1, xi2))))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT, new ExpressionPattern(IDENTITY,
                    new ExpressionPattern(SUCC, xi0),
                    new ExpressionPattern(PLUS, xi1, new ExpressionPattern(SUCC, xi2)))))}, false)) ;

        // // make(x, S) |- S
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(MAKE, SELF, xt0))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // you're able to give your stuff to other people
        m.Add(new InferenceRule(
            new List<IPattern>[]{DefaultModel.BuildList(
                new ExpressionPattern(PERSON, xi1),
                new ExpressionPattern(POSSESS, xi0, xi2))},
            new List<IPattern>[]{
                DefaultModel.BuildList(
                new ExpressionPattern(ABLE, xi0,
                    new ExpressionPattern(POSSESS, xi1, xi2)))}));

        // you're able to get to any location
        // |- able(self, at(self, location(x, y)))
        m.Add(new InferenceRule(
            new List<IPattern>[]{},
            new List<IPattern>[]{DefaultModel.BuildList(
                new ExpressionPattern(ABLE, SELF, new ExpressionPattern(AT, SELF, new ExpressionPattern(LOCATION, xi0, xi1))))}
            ));

        // TRUST
        // renege
        // bound(x, A), ~A |- ~trustworthy(x)
        m.Add(new InferenceRule(
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(BOUND, xi0, xt0), new ExpressionPattern(NOT, xt0))},
            new List<IPattern>[]{
                BuildList(new ExpressionPattern(NOT, new ExpressionPattern(TRUSTWORTHY, xi0)))},
            false));

        // // perceptual inference
        // // perceive(self, S) | veridical(self, S) |- S
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(PERCEIVE, SELF, xt0)),
        //         BuildList(new ExpressionPattern(VERIDICAL, SELF, xt0))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // able(x, S), make(x, S) |- S
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(
        //             new ExpressionPattern(ABLE, xi0, xt0),
        //             new ExpressionPattern(MAKE, xi0, xt0))},
        //     new List<IPattern>[]{BuildList(xt0)}));

        // // testimonial inference
        // // express(x, S) | sincere(x, S) |- believe(x, S)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(EXPRESS, xi0, xt0)),
        //         BuildList(new ExpressionPattern(SINCERE, xi0, xt0))
        //     },
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(BELIEVE, xi0, xt0))
        //     }));

        // // believe(x, S) | correct(x, S) |- S
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{
        //         BuildList(new ExpressionPattern(BELIEVE, xi0, xt0)),
        //         BuildList(new ExpressionPattern(CORRECT, xi0, xt0))
        //     },
        //     new List<IPattern>[]{
        //         BuildList(xt0)
        //     }));

        // // F(x) |- exists(x)
        // m.Add(new InferenceRule(
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(xp0, xi0))},
        //     new List<IPattern>[]{BuildList(new ExpressionPattern(EXISTS, xi0))}, false));

        return m;
    }
}
