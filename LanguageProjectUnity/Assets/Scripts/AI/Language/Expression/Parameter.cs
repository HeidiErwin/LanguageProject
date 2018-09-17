using System;

// an unnamed linguistic expression.
// Only used in "thought"; Cannot be uttered.
public class Parameter : Expression {
    public Parameter(SemanticType type, int id) : base(type) {
        this.headString = "[" + id + "]";
        this.headType = type;
        this.args = new Expression[type.GetNumArgs()];
    }

    public override String ToString() {
        return this.headString;
    }
}
