using System;

// the compound expression, where two expressions form a new expression.
// The first expression is a function which takes the second expression
// as input. The semantic type of the compound expression is the output
// type of the first expression.
// 
// for example, if "Bill" is an expression of type e, and "runs" is an expression
// of type e -> t, then "(runs Bill)" is valid expression of type t.
// 
public class Phrase : Expression {
    public Phrase(Expression function, Expression input) : this(function, input, 0) {}

    public Phrase(Expression function, Expression input, int index) : base(null) {
        // making the expression with subexpressions A and B
        // have the string form (A B n)
        this.name = "(" + function.GetName() + " " + input.GetName() + " " + index + ")";

        // ensuring that the types of the expressions are correct:
        // the input type of the first expression should match the type
        // of the second expression.
        if (!function.GetInputType(index).Equals(input.GetSemanticType())) {
            throw new ArgumentException();
        }

        if (function.GetNumArgs() < 2) {
            this.type = function.GetOutputType();
        }

        else {
            SemanticType[] newInputType = new SemanticType[function.GetNumArgs() - 1];

            for (int i = 0; i < index; i++) {
                newInputType[i] = function.GetInputType(i);
            }

            for (int i = index; i < newInputType.Length; i++) {
                newInputType[i] = function.GetInputType(i + 1);
            }

            this.type = new Arrow(newInputType, function.GetOutputType());
        }
    }
}
