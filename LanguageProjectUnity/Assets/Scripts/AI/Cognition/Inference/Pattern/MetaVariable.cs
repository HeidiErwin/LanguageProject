using System;
using System.Collections.Generic;

public class MetaVariable : IPattern {
    protected SemanticType type;
    protected int localID;

    public MetaVariable(SemanticType type, int localID) {
        this.type = type;
        this.localID = localID;
    }

    public bool Matches(Expression expr) {
        return this.type.Equals(expr.type);
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        foreach (MetaVariable x in bindings.Keys) {
            System.Console.WriteLine(x + "=" + this);
        }

        if (bindings.ContainsKey(this)) {
            return bindings[this].Equals(expr);
        } else {
            return this.Matches(expr);
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
