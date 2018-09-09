using System.Collections.Generic;

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
public abstract class Model {
    private HashSet<EvaluationRule> evaluationRules = new HashSet<EvaluationRule>();
    private HashSet<SubsententialRule> subsententialRules = new HashSet<SubsententialRule>();

    // returns true if e is in this model
    public abstract bool Contains(Expression e);

    // adds e to this model
    // returns true if it wasn't already in the model,
    // false if it already was
    public abstract bool Add(Expression e);
    
    public void Add(EvaluationRule r) {
        evaluationRules.Add(r);
    }

    public void Add(SubsententialRule r) {
        subsententialRules.Add(r);
    }

    // removes e from this model
    // returns true if it was already in the model,
    // false if it wasn't in there anyway
    public abstract bool Remove(Expression e);

    // return true if this model proves expr.
    public bool Proves(Expression expr) {

        // the easy one!!! if the sentence is already just in this model,
        // that means we're good to go.
        if (this.Contains(expr)) {
            return true;
        }

        // Otherwise, we have to go on a fishing expedition.
        foreach (EvaluationRule er in this.evaluationRules) {
            // we evaluate in a downward context because we want to backtrack:
            // we want to see if anything we believe entails this sentence.
            EvaluationPattern evaluation = er.Evaluate(expr, EntailmentContext.Downward);

            if (evaluation != null) {
                // TODO
            }
        }

        return false;
    }
}
