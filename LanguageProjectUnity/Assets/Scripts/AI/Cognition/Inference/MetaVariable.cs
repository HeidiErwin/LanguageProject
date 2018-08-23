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

    public IPattern Bind(int id, Expression expr) {
        if (id == localID && expr.type.Equals(this.type)) {
            return expr;
        } else {
            return this;
        }
    }
}
