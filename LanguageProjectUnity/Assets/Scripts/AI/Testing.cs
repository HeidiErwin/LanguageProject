using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    void Start() {
        Model m = DefaultModel.Make();
        // Expression example = new Word(SemanticType.TRUTH_VALUE, "example");
        // Expression iPerceiveExample = new Phrase(Expression.PERCEIVE, Expression.SELF, example);
        // Expression iPerceiveNotExample = new Phrase(Expression.PERCEIVE, Expression.SELF, new Phrase(Expression.NOT, example));
        // m.Add(iPerceiveExample);
        // PrintProves(m, true, iPerceiveExample);
        // PrintProves(m, true, example);
        // Debug.Log(m.UpdateBelief(new Phrase(Expression.NOT, example)));
        // // Debug.Log(m.UpdateBelief(iPerceiveNotExample));
        // PrintProves(m, true, iPerceiveExample);
        // PrintProves(m, true, new Phrase(Expression.NOT, example));
        // PrintProves(m, false, example);
        // PrintProves(m, true, new Phrase(Expression.NOT, new Phrase(Expression.VERIDICAL, Expression.SELF, example)));

        // Debug.Log(m.UpdateBelief(new Phrase(Expression.EXPRESS, Expression.PLAYER, new Phrase(Expression.TREE, Expression.BOB))));
        // PrintProves(m, true, new Phrase(Expression.EXPRESS, Expression.PLAYER, new Phrase(Expression.TREE, Expression.BOB)));
        // PrintProves(m, true, new Phrase(Expression.BELIEVE, Expression.PLAYER, new Phrase(Expression.TREE, Expression.BOB)));
        // PrintProves(m, true, new Phrase(Expression.TREE, Expression.BOB));

        // Debug.Log(bm.DomainString());
        // PrintProves(bm, true, new Phrase(Expression.ANIMAL, Expression.BOB));
        // PrintProves(bm, true, new Phrase(Expression.SOME, Expression.ANIMAL, Expression.ACTIVE));
        // PrintProves(bm, true, new Phrase(Expression.SOME, new Phrase(Expression.IDENTITY, Expression.BOB), Expression.ACTIVE));
        // PrintProves(bm, true, new Phrase(Expression.EXISTS, Expression.BOB));
        // PrintProves(bm, true, new Phrase(Expression.CONTAINED_WITHIN, Expression.BOB, Expression.WAYSIDE_PARK));
        // PrintProves(bm, true, new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.BOB, Expression.EVAN)));
        // PrintProves(bm, true,
        //     new Phrase(Expression.NOT,
        //         new Phrase(Expression.AND, 
        //             new Phrase(Expression.IDENTITY, Expression.BOB, Expression.EVAN),
        //             new Phrase(Expression.ACTIVE, Expression.BOB))));
        // PrintProves(bm, true, new Phrase(Expression.NOT, new Phrase(Expression.KING, Expression.EVAN)));
        // PrintProves(bm, true, new Phrase(Expression.ANIMAL, Expression.BOB_2));
        // PrintProves(bm, true, new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.EVAN, Expression.BOB_2)));
        // Debug.Log(new Phrase(Expression.IDENTITY, Expression.EVAN, Expression.BOB));
        // Debug.Log(new Phrase(Expression.SOME, Expression.KING, Expression.ACTIVE));
        // Debug.Log(new Phrase(Expression.GIVE, Expression.THE_GREAT_DOOR, Expression.BOB));
        // PrintProves(bm, true, new Phrase(Expression.GEACH_TF1, Expression.NOT, Expression.KING, Expression.EVAN));
        Expression a = new Word(SemanticType.TRUTH_VALUE, "A");
        Expression b = new Word(SemanticType.TRUTH_VALUE, "B");
        m.Add(new SubstitutionRule(
            new List<IPattern>[]{DefaultModel.BuildList(a)},
            new List<IPattern>[]{DefaultModel.BuildList(b)},
            false));
        Expression ifAB = new Phrase(Expression.IF, a, b);
        PrintProves(m, false, b);
        PrintProves(m, true, ifAB);
        PrintProves(m, false, a);
        
        
        // PrintProves(m, true, b);
        // PrintProves(m, true, new Phrase(Expression.NOT, new Phrase(Expression.NOT, new Phrase(Expression.PERSON, Expression.SELF))));
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
