// an interface for an agent's "model", an
// internal representation of the propositional attitudes
// that the agent holds: which sentences it holds true,
// which goals the agent has, etc.
// 
// this simple interface assumes that the model is
// something like a set of sentences/expressions, in
// "Mentalese", if you like (although there currently
// is no distinction between a surface language and
// the internal language).
public interface IModel {
    // returns true if e is in this model
    bool Contains(Expression e);

    // adds e to this model
    // returns true if it wasn't already in the model,
    // false if it already was
    bool Add(Expression e);

    // removes e from this model
    // returns true if it was already in the model,
    // false if it wasn't in there anyway
    bool Remove(Expression e);
}
