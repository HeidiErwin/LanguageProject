using System;
using System.Text;
using System.Collections.Generic;

// the compound expression, where two expressions form a new expression.
// The first expression is an n-place function which takes the second (indexed) expression
// as input. The semantic type of the compound expression is the result of partially
// applying the function to the argument at the appropriate index.
// 
// for example, if "Bill" is an expression of type e, and "runs" is an expression
// of type e -> t, then "runs[x0](x0:Bill)" is valid expression of type t.
// 
// OR:
// 
// if "Bill" and "Heidi" are expressions of type e, and "helps" is an expression
// of type e, e -> t, then "helps[x0, x1](Bill:x1)" reduces to "helps[x0, Bill]"
// which is of type e -> t.
// 
public class Phrase : Expression {
    // a shortcut for application: simply amounts to
    // applying an argument to the first place
    public Phrase(Expression function, Expression input) : this(function, input, 0) { }

    public Phrase(Expression function, Expression input, int index) : base(null) {
        // the index has to be between 0 and (numargs - 1)
        if (index < 0 || index >= function.GetNumFreeArgs()) {
            throw new IndexOutOfRangeException();
        }

        // ensuring that the types of the expressions are correct:
        // the indexed input type of the first expression should match the type
        // of the second expression.
        if (!function.GetInputType(index).Equals(input.type)) {
            throw new ArgumentException();
        }

        this.headString = function.headString;
        this.headType = function.type;
        this.args = new Expression[function.GetNumArgs()];

        // fills in one the old arguments for this expression.
        int counter = -1;
        for (int i = 0; i < GetNumArgs(); i++) {
            args[i] = function.GetArg(i);

            if (args[i] == null) {
                counter++;
            }

            if (counter == index) {
                args[i] = input;
                counter++;
            }
        }

        // determines the output type of the expression.
        // if it's a one-argument function, then the type is the output type.
        if (function.GetNumFreeArgs() == 1) {
            this.type = function.GetOutputType();
        } else {
            // if it's an n-argument function, then the semantic type is
            // the functional type you get if you remove the type of the indexed input.
            // e.g. if your function had a type (A, B, C -> O) and the input type was B,
            // then the new type would be (A, C -> D)

            List<SemanticType> oldInput = function.GetInputType();
            List<SemanticType> newInput = new List<SemanticType>();

            newInput.AddRange(oldInput);
            newInput.RemoveAt(index);

            this.type = new Arrow(newInput, function.GetOutputType());
        }
    }

    public Phrase(Expression function, Expression[] inputs) : base(function.GetOutputType()) {
        if (function.GetNumArgs() != function.GetNumFreeArgs() || inputs.Length != function.GetNumArgs()) {
            // we don't want partial application for this constructor... yet.
            throw new ArgumentException();
        }

        for (int i = 0; i < inputs.Length; i++) {
            if (!function.GetInputType(i).Equals(inputs[i].type)) {
                // type check
                throw new ArgumentException();
            }
        }

        this.headType = function.headType;
        this.headString = function.headString;
        this.args = new Expression[function.GetNumArgs()];

        for (int i = 0; i < inputs.Length; i++) {
            this.args[i] = inputs[i];
        }
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        s.Append(headString);
        s.Append("(");

        for (int i = 0; i < args.Length; i++) {
            Expression currentArg = args[i];
            if (currentArg == null) {
                s.Append("_");
            }
            else {
                s.Append(currentArg.ToString());
            }
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append(")");

        return s.ToString();
    }
}
