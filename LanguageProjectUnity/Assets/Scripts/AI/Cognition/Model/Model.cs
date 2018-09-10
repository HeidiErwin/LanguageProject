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

    // removes e from this model
    // returns true if it was already in the model,
    // false if it wasn't in there anyway
    public abstract bool Remove(Expression e);

    // Adding rules into the model
    public void Add(EvaluationRule r) {
        evaluationRules.Add(r);
    }

    public void Add(SubsententialRule r) {
        subsententialRules.Add(r);
    }

    public bool Proves(Expression expr) {
        return Proves(expr, EntailmentContext.Downward);
    }

    // return true if this model proves expr.
    private bool Proves(Expression expr, EntailmentContext context) {
        // base case
        if (this.Contains(expr)) {
            return true;
        }

        HashSet<Expression> provers = new HashSet<Expression>();

        foreach (SubsententialRule sr in this.subsententialRules) {
            Expression prover = sr.Infer(expr, context);
            if (prover != null) {
                if (this.Contains(prover)) {
                    return true;
                }
                provers.Add(prover);
            }
        }

        foreach (Expression next in provers) {
            if (this.Proves(next, context)) {
                return true;
            }
        }

        // TODO: the rest of it lol

        return false;
    }
}
