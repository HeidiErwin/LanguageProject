using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

using static Expression;

public class Testing : MonoBehaviour {
    void Start() {
        Model m = DefaultModel.Make(true);
        // m.Add(new Word(SemanticType.TRUTH_VALUE, "A"));
        // Debug.Log(m.Contains(new Word(SemanticType.TRUTH_VALUE, "A")));
        // Debug.Log(!m.Contains(new Word(SemanticType.TRUTH_VALUE, "B")));
        // m.Add(new Word(SemanticType.TRUTH_VALUE, "B"));
        // Debug.Log(m.Contains(new Word(SemanticType.TRUTH_VALUE, "B")));
        // m.Add(new Phrase(ANIMAL, BOB));
        // Debug.Log(m.Contains(new Phrase(ANIMAL, BOB)));
        // m.Add(new Phrase(ANIMAL, EVAN));
        // Debug.Log(m.Contains(new Phrase(ANIMAL, EVAN)));
        m.Add(new Phrase(AT, BOB, EVAN));
        m.Add(new Phrase(AT, EVAN, GOAL));
        Debug.Log(!m.Contains(new Phrase(AT, BOB, GOAL)));

        // in the model: 
        // 
        // greater(one, _) is no good
        // 
        // every dog howls.
        // 
        // dog is predicate and so is howls
        // 
        // every(predicate, predicate) -> true
        // every(dog(_), howls(_))
        // 

        // Expression example = new Word(SemanticType.TRUTH_VALUE, "example");
        // Expression iPerceiveExample = new Phrase(PERCEIVE, SELF, example);
        // Expression iPerceiveNotExample = new Phrase(PERCEIVE, SELF, new Phrase(NOT, example));
        // m.Add(iPerceiveExample);
        // PrintProves(m, true, iPerceiveExample);
        // PrintProves(m, true, example);
        // Debug.Log(m.UpdateBelief(new Phrase(NOT, example)));
        // // Debug.Log(m.UpdateBelief(iPerceiveNotExample));
        // PrintProves(m, true, iPerceiveExample);
        // PrintProves(m, true, new Phrase(NOT, example));
        // PrintProves(m, false, example);
        // PrintProves(m, true, new Phrase(NOT, new Phrase(VERIDICAL, SELF, example)));

        // Debug.Log(m.UpdateBelief(new Phrase(EXPRESS, PLAYER, new Phrase(TREE, BOB))));
        // PrintProves(m, true, new Phrase(EXPRESS, PLAYER, new Phrase(TREE, BOB)));
        // PrintProves(m, true, new Phrase(BELIEVE, PLAYER, new Phrase(TREE, BOB)));
        // PrintProves(m, true, new Phrase(TREE, BOB));

        // Debug.Log(bm.DomainString());
        // PrintProves(bm, true, new Phrase(ANIMAL, BOB));
        // PrintProves(bm, true, new Phrase(SOME, ANIMAL, ACTIVE));
        // PrintProves(bm, true, new Phrase(SOME, new Phrase(IDENTITY, BOB), ACTIVE));
        // PrintProves(bm, true, new Phrase(EXISTS, BOB));
        // PrintProves(bm, true, new Phrase(CONTAINED_WITHIN, BOB, WAYSIDE_PARK));
        // PrintProves(bm, true, new Phrase(NOT, new Phrase(IDENTITY, BOB, EVAN)));
        // PrintProves(bm, true,
        //     new Phrase(NOT,
        //         new Phrase(AND, 
        //             new Phrase(IDENTITY, BOB, EVAN),
        //             new Phrase(ACTIVE, BOB))));
        // PrintProves(bm, true, new Phrase(NOT, new Phrase(KING, EVAN)));
        // PrintProves(bm, true, new Phrase(ANIMAL, BOB_2));
        // PrintProves(bm, true, new Phrase(NOT, new Phrase(IDENTITY, EVAN, BOB_2)));
        // Debug.Log(new Phrase(IDENTITY, EVAN, BOB));
        // Debug.Log(new Phrase(SOME, KING, ACTIVE));
        // Debug.Log(new Phrase(GIVE, THE_GREAT_DOOR, BOB));
        // PrintProves(bm, true, new Phrase(GEACH_TF1, NOT, KING, EVAN));
        // PrintProves(m, true, b);
        // PrintProves(m, true, new Phrase(NOT, new Phrase(NOT, new Phrase(PERSON, SELF))));
        
        // Expression a = new Word(SemanticType.TRUTH_VALUE, "A");
        // Expression b = new Word(SemanticType.TRUTH_VALUE, "B");

        // m.Add(a);
        // m.Add(new Phrase(NOT, b));

        // PrintProves(m, true, new Phrase(TRUE, a));
        // PrintProves(m, true, new Phrase(TRUE, new Phrase(TRUE, a)));
        // PrintProves(m, true, new Phrase(TRUE, new Phrase(NOT, b)));
        // PrintProves(m, true, new Phrase(NOT, new Phrase(TRUE, b)));

        // m.Add(new SubstitutionRule(
        //     new List<IPattern>[]{DefaultModel.BuildList(a)},
        //     new List<IPattern>[]{DefaultModel.BuildList(b)},
        //     false));
        // Expression ifAB = new Phrase(IF, a, b);
        // PrintProves(m, false, b);
        // PrintProves(m, true, ifAB);
        // PrintProves(m, false, a);
        
        // // now, testing a conditional is true, even if the antecedant is thought false
        // m.Add(new Phrase(NOT, a));
        // PrintProves(m, true, new Phrase(NOT, a));
        // PrintProves(m, true, ifAB);

        // MODUS PONENS
        Expression c = new Word(SemanticType.TRUTH_VALUE, "C");
        Expression d = new Word(SemanticType.TRUTH_VALUE, "D");
        m.Add(new Phrase(IF, c, d));
        m.Add(c);
        PrintProves(m, true, d);
        Expression e = new Word(SemanticType.TRUTH_VALUE, "E");
        m.Add(new Phrase(IF, d, e));
        PrintProves(m, true, e);

        // // Expression f = new Word(SemanticType.TRUTH_VALUE, "F");
        // // m.Add(new Phrase(IF, e, f));
        // // PrintProves(m, true, f);

        // Expression bobIsRed = new Phrase(RED, BOB);
        // Expression allRedsAreGreen = new Phrase(ALL, RED, GREEN);
        // m.Add(bobIsRed);
        // m.Add(allRedsAreGreen);
        // // PrintProves(m, true, new Phrase(GREEN, BOB));

        // Expression allGreensAreBlue = new Phrase(ALL, GREEN, BLUE);
        // m.Add(allGreensAreBlue);
        // PrintProves(m, true, new Phrase(BLUE, BOB));

        // // Expression dBetterThanC = new Phrase(BETTER, d, c);
        // // Expression eBetterThanD = new Phrase(BETTER, e, d);
        // // m.Add(dBetterThanC);
        // // m.Add(eBetterThanD);
        // // PrintProves(m, true, new Phrase(BETTER, e, c));
        // // Expression fBetterThanE = new Phrase(BETTER, f, e);
        // // m.Add(fBetterThanE);
        // // PrintProves(m, true, new Phrase(BETTER, f, c));

        // Expression g = new Word(SemanticType.TRUTH_VALUE, "G");
        // // PrintProves(m, false, new Phrase(BETTER, g, c));

        // // PrintPlan(m, c);

        // // PrintPlan(m, new Phrase(MAKE, SELF, g));

        // m.Add(new Phrase(ABLE, SELF, g));
        // // m.Add(new Phrase(MAKE, SELF, g));
        // // PrintProves(m, false, g);
        // PrintPlan(m, g);


        // // able(x, H), make(x, H) |- H
        // Expression h = new Word(SemanticType.TRUTH_VALUE, "H");
        // MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        // // m.Add(new SubstitutionRule(
        // //     new List<IPattern>[]{
        // //         DefaultModel.BuildList(new ExpressionPattern(ABLE, xi0, h)),
        // //         DefaultModel.BuildList(new ExpressionPattern(MAKE, xi0, h))
        // //     },
        // //     new List<IPattern>[]{DefaultModel.BuildList(h)}));

        // // m.Add(new Phrase(WOULD, h));
        // m.Add(new Phrase(ABLE, BOB, h));
        // m.Add(new Phrase(MAKE, BOB, h));
        // PrintProves(m, true, h);

        // m.Add(new Phrase(ABLE, SELF, new Phrase(AT, SELF, BOB)));
        // // PrintProves(m, false, new Phrase(AT, SELF, BOB));
        // PrintPlan(m, new Phrase(AT, SELF, BOB));

        // m.Add(new Phrase(IDENTITY, BOB, BOB_2));
        // PrintProves(m, true, new Phrase(IDENTITY, BOB, BOB_2));
        // // m.Add(new Phrase(TREE, BOB));
        // // PrintProves(m, true, new Phrase(TREE, BOB_2));

        // // PrintProves(m, true, new Phrase(IDENTITY,
        // //     new Phrase(SUCC, new Phrase(SUCC, new Phrase(SUCC, ZERO))),
        // //     new Phrase(PLUS, new Phrase(SUCC, ZERO), 
        // //         new Phrase(SUCC, new Phrase(SUCC, ZERO)))));

        // // PrintProves(m, true, new Phrase(NOT, new Phrase(IDENTITY,
        // //     new Phrase(SUCC, new Phrase(SUCC, new Phrase(SUCC, ZERO))),
        // //     new Phrase(PLUS, new Phrase(SUCC, new Phrase(SUCC, ZERO)), 
        // //         new Phrase(SUCC, new Phrase(SUCC, ZERO))))));
        
        // m.Add(new Phrase(BELIEVE, EVAN, new Phrase(BELIEVE, BOB, new Phrase(IN_THE_ROOM, RUBY))));
        // "every(A, B)" => write: "B(x) :- A(x)"
        // PrintProves(m, true, new Phrase(IN_THE_ROOM, RUBY));
    }

    private void PrintProves(Model m, bool proves, Expression e) {
        HashSet<Expression> basis = m.GetBasis(e);
        if (proves) {
            if (basis != null) {
                StringBuilder s = new StringBuilder();
                s.Append("SUCCESS: model proves '" + e + "' with basis {");
                foreach (Expression b in basis) {
                    s.Append(b + ", ");
                }
                s.Append("}");
                Debug.Log(s.ToString());
            } else {
                Debug.Log("FAILURE: model doesn't prove '" + e + "'");
            }
        } else {
            if (basis == null) {
                Debug.Log("SUCCESS: model doesn't prove '" + e + "'");
            } else {
                StringBuilder s = new StringBuilder();
                s.Append("FAILURE: model proves '" + e + "' with basis {");
                foreach (Expression b in basis) {
                    s.Append(b + ", ");
                }
                s.Append("}");
                Debug.Log(s.ToString());
            }
        }
    }

    private void PrintPlan(Model m, Expression e) {
        HashSet<Expression> plan = m.GetBasis(true, e);
        if (plan == null) {
            Debug.Log("NO PLAN FOR " + e);
        } else {
            StringBuilder s = new StringBuilder();
            s.Append("PLAN FOR " + e + ":\n");
            foreach (Expression p in plan) {
                if (p.GetHead().Equals(WOULD)) {
                    s.Append(p + "\n");
                }
            }
            s.Append("END PLAN FOR " + e);
            Debug.Log(s.ToString());
        }
    }

    private String ContentString(Dictionary<MetaVariable, Expression> bindings) {
        StringBuilder s = new StringBuilder();
        s.Append("{");
        foreach (MetaVariable x in bindings.Keys) {
            s.Append(x);
            s.Append(" |-> ");
            s.Append(bindings[x]);
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append("}");

        return s.ToString();
    }

    private String ContentString(IEnumerable<Dictionary<MetaVariable, Expression>> bindingSet) {
        StringBuilder s = new StringBuilder();
        s.Append("{\n");
        if (bindingSet == null) {
            return "NULL";
        }

        foreach (Dictionary<MetaVariable, Expression> bindings in bindingSet) {
            s.Append("\t");
            s.Append(ContentString(bindings));
            s.Append(",\n");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append("\n}");

        return s.ToString();
    }
}
