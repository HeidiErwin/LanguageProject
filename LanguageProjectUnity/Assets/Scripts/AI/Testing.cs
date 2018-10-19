using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    void Start() {
        Model bm = CustomModels.BobModel();
        Debug.Log(bm.DomainString());
        PrintProves(bm, new Phrase(Expression.ANIMAL, Expression.BOB), true);
        PrintProves(bm, new Phrase(Expression.SOME, Expression.ANIMAL, Expression.ACTIVE), true);
    }

    private void PrintProves(Model m, Expression e, bool proves) {
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
