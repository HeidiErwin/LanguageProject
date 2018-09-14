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
public abstract class Expression : IPattern {
    public SemanticType type { get; protected set; }
    public String headString { get; protected set; }
    public SemanticType headType { get; protected set; }
    protected Expression[] args;

    public Expression(SemanticType type) {
        this.type = type;
    }

    // returns the filled-in arguments for this expression.
    public Expression GetArg(int i) {
        return args[i];
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

        if (!this.type.Equals(that.type)) {
            return false;
        }

        if (!this.headString.Equals(that.headString)) {
            return false;
        }

        if (this.GetNumArgs() != that.GetNumArgs()) {

            return false;
        }



        for (int i = 0; i < this.GetNumArgs(); i++) {
            if (this.GetArg(i) == null) {
                if (that.GetArg(i) != null) {
                    return false;
                } else {
                    continue;
                }
            }

            if (that.GetArg(i) == null) {
                if (this.GetArg(i) != null) {
                    return false;
                } else {
                    continue;
                }
            }

            if (!this.GetArg(i).Equals(that.GetArg(i))) {
                return false;
            }
        }

        return true;
    }

    // TODO implement GetHashCode() so SimpleModel can be used
    public bool Matches(Expression expr) {
        return this.Equals(expr);
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        return this.Matches(expr);
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return new HashSet<MetaVariable>();
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        return this;
    }

    public Expression ToExpression() {
        return this;
    }

    public override int GetHashCode() {
        int hash = 5381 * headString.GetHashCode();
        for (int i = 0; i < args.Length; i++) {
            hash = 33 * hash + (args[i] == null ? i : args[i].GetHashCode());
        }
        return hash;
    }

    // the words of the language
    public static readonly Expression TRUE = new Word(SemanticType.TRUTH_FUNCTION_1, "true");
    public static readonly Expression NOT = new Word(SemanticType.TRUTH_FUNCTION_1, "not");

    public static readonly Expression AND = new Word(SemanticType.TRUTH_FUNCTION_2, "and");
    public static readonly Expression OR = new Word(SemanticType.TRUTH_FUNCTION_2, "or");

    public static readonly Expression NO = new Word(SemanticType.DETERMINER, "no");
    public static readonly Expression A = new Word(SemanticType.DETERMINER, "a");
    public static readonly Expression TWO = new Word(SemanticType.DETERMINER, "two");
    public static readonly Expression THREE = new Word(SemanticType.DETERMINER, "three");
    public static readonly Expression EVERY = new Word(SemanticType.DETERMINER, "every");

    public static readonly Expression BOB = new Word(SemanticType.INDIVIDUAL, "bob");
    public static readonly Expression EVAN = new Word(SemanticType.INDIVIDUAL, "evan");

    public static readonly Expression FOUNTAIN = new Word(SemanticType.PREDICATE, "fountain");
    public static readonly Expression LAMP = new Word(SemanticType.PREDICATE, "lamp");
    public static readonly Expression ACTIVE = new Word(SemanticType.PREDICATE, "active");
    public static readonly Expression INACTIVE = new Word(SemanticType.PREDICATE, "inactive");
    public static readonly Expression KING = new Word(SemanticType.PREDICATE, "king");
    public static readonly Expression YELLOW = new Word(SemanticType.PREDICATE, "yellow");
    public static readonly Expression GREEN = new Word(SemanticType.PREDICATE, "green");
    public static readonly Expression BLUE = new Word(SemanticType.PREDICATE, "blue");
    public static readonly Expression RED = new Word(SemanticType.PREDICATE, "red");
    public static readonly Expression IN_YOUR_AREA = new Word(SemanticType.PREDICATE, "in-your-area");
    public static readonly Expression IN_YELLOW_AREA = new Word(SemanticType.PREDICATE, "in-yellow-area");
    public static readonly Expression IN_GREEN_AREA = new Word(SemanticType.PREDICATE, "in-green-area");
    public static readonly Expression IN_BLUE_AREA = new Word(SemanticType.PREDICATE, "in-blue-area");
    public static readonly Expression IN_RED_AREA = new Word(SemanticType.PREDICATE, "in-red-area");

    public static readonly Expression ITSELF = new Word(SemanticType.RELATION_2_REDUCER, "itself");
}
