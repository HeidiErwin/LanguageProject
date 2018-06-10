using System;

// a linguistic entity, either as a primitive word or a
// complex phrase which (currently but perhaps not permanently)
// consists of a combination of two subexpressions. That is:
// 
// expression := <string> : X
// expression := (expression : X -> Y, expression : X)
// 
// an expression consists of either any string with any semantic type,
// or an ordered pair of two expressions, where the only constraint is
// that the second has the semantic type of the
// first expression's input semantic type.
// 
public abstract class Expression {
    protected String name;
    protected SemanticType type;

    public Expression(SemanticType type) {
        this.type = type;
    }

    // returns the name of this expression.
    // Right now, this is identical to ToString(),
    // but this will change when we have graphical
    // representations of this expression.
    public String GetName() {
        return name;
    }

    // returns this expression's semantic type.
    public SemanticType GetSemanticType() {
        return type;
    }

    // returns the input type of this expression's semantic type,
    // if it is a functional type. If not, it returns null.
    public SemanticType GetInputType() {
        return type.GetInputType();
    }

    // returns the output type of this expression's semantic type,
    // if it is a functional type. If not, it returns null.
    public SemanticType GetOutputType() {
        return type.GetOutputType();
    }

    public override String ToString() {
        return name;
    }
}
