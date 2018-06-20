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
// semantictype := (semantictype -> semantictype)
// 
// the third is implemented in "Arrow.cs".
// 
// "e" is used to restrict to expressions which refer to an individual
// "t" is used to restrict to expressions which refer to a truth value
// "X -> Y" is used to restrict to expressions which refer to
// functions from Xs to Ys.
// 

public abstract class SemanticType {
    // these apply only to the functional types,
    // returning the input or output type, or number of arguments
    // of this type, and returning null if they're not a functional type
    public virtual int GetNumArgs() {
        return -1;
    }

    public virtual SemanticType GetInputType(int n) {
        return null;
    }

    public virtual SemanticType GetOutputType() {
        return null;
    }
}
