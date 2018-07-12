using System;
using System.Collections.Generic;

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
    public SemanticType type { get; protected set; }
    public String head { get; protected set; }
    protected Expression[] args;

    public Expression(SemanticType type) {
        this.type = type;
    }

    // returns the filled-in arguments for this expression.
    public Expression GetArg(int i) {
        return args[i];
    }

    // returns the string representation of the head of this expression.
    public String GetHead() {
        return head;
    }

    // returns the total number of arguments of this expression,
    // if any. If not, it returns 0.
    public int GetNumArgs() {
        return args.Length;
    }

    // returns the number of unbounded arguments of this expression,
    // if any. If not, it returns 0.
    public int GetNumFreeArgs() {
        return type.GetNumArgs();
    }

    // returns the input type of this expression's semantic type,
    // if it is a functional type. If not, it returns null.
    public SemanticType GetInputType(int index) {
        return type.GetInputType(index);
    }

    // returns the list of types corresponding to the
    // input type of this expression.
    public List<SemanticType> GetInputType() {
        return type.GetInputType();
    }

    // returns the output type of this expression's semantic type,
    // if it is a functional type. If not, it returns null.
    public SemanticType GetOutputType() {
        return type.GetOutputType();
    }

    // this is syntactic equality; two expressions are equal
    // just when the expressions are identical. Two expressions
    // whose referents are identical are still considered distinct
    // under this form of equality.
    // 
    // i.e. (2 + 3) != (3 + 2)
    public override bool Equals(Object o) {
        Expression that = o as Expression;

        if (that == null) {
            return false;
        }

        if (!this.head.Equals(that.head)) {
            return false;
        }

        if (this.GetNumArgs() != that.GetNumArgs()) {
            
            return false;
        }

        for (int i = 0; i < this.GetNumArgs(); i++) {
            if (!this.GetArg(i).Equals(that.GetArg(i))) {
                return false;
            }
        }

        return true;
    }
}
