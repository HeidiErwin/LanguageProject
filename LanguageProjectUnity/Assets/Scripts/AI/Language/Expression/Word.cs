using System;

// the primitive linguistic expression, whose string representation of its name, semantic type,
// and meaning are directly given.
public class Word : Expression {
    public Word(SemanticType type, String nameString) : base(type) {
        this.headString = nameString;
        this.args = new Expression[type.GetNumArgs()];
    }

    public override String ToString() {
        return headString;
    }
}