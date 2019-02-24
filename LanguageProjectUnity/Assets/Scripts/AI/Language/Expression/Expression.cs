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

    public Expression GetHead() {
        return new Word(headType, headString);
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

    // returns the output type of this expression's semantic type,
    // if it is a functional type. If not, it returns null.
    public SemanticType GetOutputType() {
        return type.GetOutputType();
    }

    public Expression Copy() {
        Expression[] newArgs = new Expression[GetNumArgs()];
        for (int i = 0; i < GetNumArgs(); i++) {
            if (args[i] != null) {
                newArgs[i] = args[i].Copy();
            }
        }

        return new Phrase(GetHead(), newArgs);
    }

    // returns an expression with the current argument index removed.
    // if there is no such argument specified at index, then it simply
    // returns this expression.
    public Expression Remove(int index) {
        if (index > args.Length || index < 0) {
            throw new ArgumentException("Remove: index out of bounds.");
        }

        if (args[index] == null) {
            return this;
        }

        Expression[] newArgs = new Expression[args.Length];

        for (int i = 0; i < args.Length; i++) {
            newArgs[i] = args[i];
        }

        newArgs[index] = null;

        return new Phrase(new Word(headType, headString), newArgs);
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

    public SemanticType GetSemanticType() {
        return type;
    }

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr, List<Dictionary<MetaVariable, Expression>> bindings) {
        if (this.Equals(expr)) {
            return bindings;
        } else {
            return null;
        }
    }

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr) {
        return GetBindings(expr, new List<Dictionary<MetaVariable, Expression>>());
    }

    // TODO implement GetHashCode() so SimpleModel can be used
    public bool Matches(Expression expr) {
        return this.Equals(expr);
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        return this.Matches(expr);
    }

    public bool Matches(Expression expr, List<Dictionary<MetaVariable, Expression>> bindings) {
        return this.Matches(expr);
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return new HashSet<MetaVariable>();
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        return this;
    }

    public IPattern Bind(Dictionary<MetaVariable, Expression> binding) {
        return this;
    }

    public List<IPattern> Bind(List<Dictionary<MetaVariable, Expression>> bindings) {
        List<IPattern> output = new List<IPattern>();
        output.Add(this);
        return output;
    }

    public Expression ToExpression() {
        return this;
    }

    public void AddToDomain(Model m) {
        m.AddToDomain(this);
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

    public static readonly Expression VERUM = new Word(SemanticType.TRUTH_VALUE, "verum");
    public static readonly Expression FALSUM = new Phrase(Expression.NOT, Expression.VERUM);

    public static readonly Expression AFFIRM = new Word(SemanticType.TRUTH_VALUE, "affirm");
    public static readonly Expression DENY = new Phrase(Expression.NOT, Expression.AFFIRM);

    public static readonly Expression ACCEPT = new Word(SemanticType.CONFORMITY_VALUE, "accept");
    public static readonly Expression REFUSE = new Word(SemanticType.CONFORMITY_VALUE, "refuse");
    
    public static readonly Expression ASSERT = new Word(SemanticType.TRUTH_ASSERTION_FUNCTION, "assert");

    public static readonly Expression WOULD = new Word(SemanticType.TRUTH_CONFORMITY_FUNCTION, "would");

    public static readonly Expression AND = new Word(SemanticType.TRUTH_FUNCTION_2, "and");
    public static readonly Expression OR = new Word(SemanticType.TRUTH_FUNCTION_2, "or");
    public static readonly Expression IF = new Word(SemanticType.TRUTH_FUNCTION_2, "if");
    public static readonly Expression EQUIVALENT = new Word(SemanticType.TRUTH_FUNCTION_2, "equivalent");

    public static readonly Expression NOMINALIZE = new Word(SemanticType.DETERMINER, "nominalize");

    public static readonly Expression NO = new Word(SemanticType.QUANTIFIER, "no");
    public static readonly Expression SOME = new Word(SemanticType.QUANTIFIER, "some");
    public static readonly Expression TWO = new Word(SemanticType.QUANTIFIER, "two");
    public static readonly Expression THREE = new Word(SemanticType.QUANTIFIER, "three");
    public static readonly Expression ALL = new Word(SemanticType.QUANTIFIER, "all");
    public static readonly Expression GEACH_QP = new Word(SemanticType.GEACH_QUANTIFIER_PHRASE, "geach");

    public static readonly Expression A = new Word(SemanticType.DETERMINER, "a");
    public static readonly Expression THE = new Word(SemanticType.DETERMINER, "the");

    public static readonly Expression MAKE = new Word(SemanticType.INDIVIDUAL_TRUTH_RELATION, "make");
    public static readonly Expression PERCEIVE = new Word(SemanticType.INDIVIDUAL_TRUTH_RELATION, "perceive");
    public static readonly Expression BELIEVE = new Word(SemanticType.INDIVIDUAL_TRUTH_RELATION, "believe");
    public static readonly Expression DESIRE = new Word(SemanticType.INDIVIDUAL_TRUTH_RELATION, "desire");
    public static readonly Expression INTEND = new Word(SemanticType.INDIVIDUAL_TRUTH_RELATION, "intend");

    public static readonly Expression BOB = new Word(SemanticType.INDIVIDUAL, "bob");
    public static readonly Expression BOB_2 = new Word(SemanticType.INDIVIDUAL, "bob2");
    public static readonly Expression EVAN = new Word(SemanticType.INDIVIDUAL, "evan");
    public static readonly Expression PLAYER = new Word(SemanticType.INDIVIDUAL, "player");
    public static readonly Expression WAYSIDE_PARK = new Word(SemanticType.INDIVIDUAL, "wayside_park");
    public static readonly Expression THE_GREAT_KEY = new Word(SemanticType.INDIVIDUAL, "the_great_key");
    public static readonly Expression SELF = new Word(SemanticType.INDIVIDUAL, "self");
    public static readonly Expression GOAL = new Word(SemanticType.INDIVIDUAL, "goal");
    // public static readonly Expression NOW = new Word(SemanticType.EVENT, "now");

    public static readonly Expression INDIVIDUAL_VARIABLE = new Word(SemanticType.INDIVIDUAL, "x");
    public static readonly Expression NEXT_VARIABLE = new Word(SemanticType.INDIVIDUAL_FUNCTION_1, "prime");

    public static readonly Expression CREDIBLE = new Word(SemanticType.PREDICATE, "credible");

    public static readonly Expression EXISTS = new Word(SemanticType.PREDICATE, "exists");
    public static readonly Expression FOUNTAIN = new Word(SemanticType.PREDICATE, "fountain");
    public static readonly Expression LAMP = new Word(SemanticType.PREDICATE, "lamp");
    public static readonly Expression ACTIVE = new Word(SemanticType.PREDICATE, "active");
    public static readonly Expression INACTIVE = new Word(SemanticType.PREDICATE, "inactive");
    public static readonly Expression KING = new Word(SemanticType.PREDICATE, "king");
    public static readonly Expression CROWN = new Word(SemanticType.PREDICATE, "crown");
    public static readonly Expression COW = new Word(SemanticType.PREDICATE, "cow");
    public static readonly Expression PERSON = new Word(SemanticType.PREDICATE, "person");
    public static readonly Expression ANIMAL = new Word(SemanticType.PREDICATE, "animal");
    public static readonly Expression COLOR = new Word(SemanticType.PREDICATE, "color");

    public static readonly Expression TREE = new Word(SemanticType.PREDICATE, "tree");
    public static readonly Expression LOG = new Word(SemanticType.PREDICATE, "log");

    public static readonly Expression DOOR = new Word(SemanticType.PREDICATE, "door");
    public static readonly Expression CLOSED = new Word(SemanticType.PREDICATE, "closed");
    public static readonly Expression OPEN = new Word(SemanticType.PREDICATE, "open");

    public static readonly Expression IN_THE_ROOM = new Word(SemanticType.PREDICATE, "in_the_room");
    
    public static readonly Expression BLACK = new Word(SemanticType.PREDICATE, "black");
    public static readonly Expression RED = new Word(SemanticType.PREDICATE, "red");
    public static readonly Expression GREEN = new Word(SemanticType.PREDICATE, "green");
    public static readonly Expression BLUE = new Word(SemanticType.PREDICATE, "blue");
    public static readonly Expression YELLOW = new Word(SemanticType.PREDICATE, "yellow");
    public static readonly Expression CYAN = new Word(SemanticType.PREDICATE, "cyan");
    public static readonly Expression MAGENTA = new Word(SemanticType.PREDICATE, "magenta");
    public static readonly Expression WHITE = new Word(SemanticType.PREDICATE, "white");

    public static readonly Expression REAL = new Word(SemanticType.PREDICATE_MODIFIER, "real");
    public static readonly Expression FAKE = new Word(SemanticType.PREDICATE_MODIFIER, "fake");

    public static readonly Expression MOD = new Word(SemanticType.PREDICATE_MODIFIER_2, "mod");
    public static readonly Expression AND_PRED = new Word(SemanticType.PREDICATE_MODIFIER_2, "and");

    public static readonly Expression IDENTITY = new Word(SemanticType.RELATION_2, "=");
    public static readonly Expression CONTAINED_WITHIN = new Word(SemanticType.RELATION_2, "contained_within");
    public static readonly Expression OVERLAPS_WITH = new Word(SemanticType.RELATION_2, "overlaps_with");
    public static readonly Expression HELP = new Word(SemanticType.RELATION_2, "help");
    public static readonly Expression AT = new Word(SemanticType.RELATION_2, "at");
    public static readonly Expression POSSESS = new Word(SemanticType.RELATION_2, "possess");
    public static readonly Expression WEAR = new Word(SemanticType.RELATION_2, "wear");

    public static readonly Expression GIVE = new Word(SemanticType.RELATION_3, "give");

    public static readonly Expression ITSELF = new Word(SemanticType.RELATION_2_REDUCER, "itself");
    public static readonly Expression INVERSE = new Word(SemanticType.RELATION_FUNCTION_2, "inverse");

    public static readonly Expression GEACH_TF1 = new Word(SemanticType.GEACH_TRUTH_FUNCTION_1, "geach");
    public static readonly Expression GEACH_TF2 = new Word(SemanticType.GEACH_TRUTH_FUNCTION_2, "geach");
}
