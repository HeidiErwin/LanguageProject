using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    void Start() {
        Model bm = CustomModels.BobModel();
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
    }

    private void PrintProves(Model m, bool proves, Expression e) {
        if (proves) {
            if (m.Proves(e)) {
                Debug.Log("SUCCESS: model proves '" + e + "'");
            } else {
                Debug.Log("FAILURE: model doesn't prove '" + e + "'");
            }
        } else {
            if (!m.Proves(e)) {
                Debug.Log("SUCCESS: model doesn't prove '" + e + "'");
            } else {
                Debug.Log("FAILURE: model proves '" + e + "'");
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
