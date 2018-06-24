using System.Collections.Generic;

// this is a type system for logical forms.
// it's a way of restricting what's expressible
// as a logical form: think about it as a
// grammatical marker like singular/plural.
// 
// just like you can't grammatically say
// "he walk",
// you can't grammatically say "F_{e->t}(x_t)",
// but you can grammatically say "F_{e->t}(x_e)"
// 
// this allows logical forms to mark variables and constants
// with semantic types.
// 
// the following are possible semantic types, defined recursively:
// 
// semantictype := e
// semantictype := t
// semantictype := (semantictype, ..., semantictype -> e|t)
// 
// the third is implemented in "Arrow.cs".
// 
// "e" is used to restrict to expressions which refer to an individual
// "t" is used to restrict to expressions which refer to a truth value
// "X1, X2, ... Xn -> Y" is used to restrict to expressions which refer to
// n-argument functions from (X1, X2, ..., Xn)s to Ys.
// 
public abstract class SemanticType {
    // returns true if the semantic type is not a
    // functional type, as in e or t or whatever else.
    public abstract bool IsAtomic();
    
    // returns the number of arguments the function accepts,
    // if it's a functional type. If not, it returns 0.
    public virtual int GetNumArgs() {
        return 0;
    }

    // these apply only to the functional types,
    // returning the input or output type of this type,
    // and returning null if they're not a functional type
    public virtual SemanticType GetInputType(int n) {
        return null;
    }

    public virtual List<SemanticType> GetInputType() {
        return null;
    }

    public virtual SemanticType GetOutputType() {
        return null;
    }
}
