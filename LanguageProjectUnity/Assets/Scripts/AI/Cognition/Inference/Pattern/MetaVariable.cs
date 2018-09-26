using System;
using System.Collections.Generic;

public class MetaVariable : IPattern {
    protected SemanticType type;
    protected int localID;

    public MetaVariable(SemanticType type, int localID) {
        this.type = type;
        this.localID = localID;
    }

    public SemanticType GetSemanticType() {
        return type;
    }

    public bool Matches(Expression expr) {
        return expr != null && this.type.Equals(expr.type);
    }

    public bool Matches(Expression expr, List<Dictionary<MetaVariable, Expression>> possibleBindings) {
        if (expr == null) {
            return false;
        }

        bool oneMatched = false;
        foreach (Dictionary<MetaVariable, Expression> binding in possibleBindings) {
            if (this.Matches(expr, binding)) {
                oneMatched = true;
            } else {
                // here, we want to remove this binding from possible bindings:
                // if it can't match the variable, then it's not possible.
            }
        }

        return oneMatched;
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> binding) {        
        if (binding.ContainsKey(this)) {
            return binding[this].Equals(expr);
        } else {
            bool matches = this.Matches(expr);

            if (matches) {
                binding.Add(this, expr);
            }

            return matches;
        }
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        HashSet<MetaVariable> xs = new HashSet<MetaVariable>();
        xs.Add(this);
        return xs;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (x.localID == this.localID && x.type == this.type) {
            return expr;
        } else {
            return this;
        }
    }

    public Expression ToExpression() {
        return null;
    }

    public override int GetHashCode() {
        return localID;
    }

    public override bool Equals(Object o) {
        MetaVariable that = o as MetaVariable;
        return this.type.Equals(that.type) && this.localID == that.localID;
    }

    public override String ToString() {
        return "{" + localID + ":" + type + "}";
    }
}
