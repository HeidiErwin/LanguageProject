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
    public Phrase(Expression function, Expression input) : base(null) {
        // making the expression with subexpressions A and B
        // have the string form (A B)
        this.name = "(" + function.GetName() + " " + input.GetName() + ")";

        // ensuring that the types of the expressions are correct:
        // the input type of the first expression should match the type
        // of the second expression.
        if (!function.GetInputType().Equals(input.GetSemanticType())) {
            throw new ArgumentException();
        }

        // the type of the compound expression is the
        // output type of the first expression.
        this.type = function.GetOutputType();
    }
}
