using System.Collections.Generic;

public class MetaVariable : IPattern {
    protected SemanticType type;
    protected int localID;

    public MetaVariable(SemanticType type, int localID) {
        this.type = type;
        this.localID = localID;
    }

    public int GetLocalID() {
        return localID;
    }

    public bool Matches(Expression expr) {
        return this.type.Equals(expr.type);
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        HashSet<MetaVariable> xs = new HashSet<MetaVariable>();
        xs.Add(this);
        return xs;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (x.GetLocalID() == this.localID && x.type == this.type) {
            return expr;
        } else {
            return this;
        }
    }
}
