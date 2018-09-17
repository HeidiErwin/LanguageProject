using System.Collections.Generic;
using UnityEngine;

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
    private List<EvaluationRule> evaluationRules = new List<EvaluationRule>();
    private HashSet<SubstitutionRule> substitutionRules = new HashSet<SubstitutionRule>();
    private HashSet<InferenceRule> inferenceRules = new HashSet<InferenceRule>();

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

    public void Add(SubstitutionRule r) {
        substitutionRules.Add(r);
    }

    public void Add(InferenceRule r) {
        inferenceRules.Add(r);
    }

    // TODO: important. Need to add a "Path" so that copies of the same sentence
    // Don't get tried. Also, need to think of a solution to the 
    // S |- T(S) problem.
    protected HashSet<Expression> GenerateSubexpressions(Expression expr, EntailmentContext context) {
        // Debug.Log(expr + (context == EntailmentContext.Upward ? "+" : "-"));
        // TODO make this more efficient
        
        HashSet<Expression> expressions = new HashSet<Expression>();
        expressions.Add(expr);

        foreach (EvaluationRule er in this.evaluationRules) {
            Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
            if (er.Matches(expr, bindings)) {

                IPattern result = er.result;

                foreach (MetaVariable x in bindings.Keys) {
                    result = result.Bind(x, bindings[x]);
                }

                HashSet<Expression>[] subExpressions = new HashSet<Expression>[er.Length()];
                for (int i = 0; i < er.Length(); i++) {
                    IPattern pattern = er.Get(i).pattern;

                    foreach (MetaVariable x in bindings.Keys) {
                        pattern = pattern.Bind(x, bindings[x]);
                    }

                    subExpressions[i] = GenerateSubexpressions(pattern.ToExpression(), EvaluationPattern.MergeContext(context, er.Get(i).context));
                }

                HashSet<IPattern> partials = new HashSet<IPattern>();
                HashSet<IPattern> newPartials = new HashSet<IPattern>();
                partials.Add(result);

                for (int i = 0; i < subExpressions.Length; i++) {
                    foreach (IPattern partial in partials) {
                        foreach (Expression e in subExpressions[i]) {
                            newPartials.Add(partial.Bind(new MetaVariable(e.type, -1 * (i + 1)), e));
                        }
                    }
                    partials = newPartials;
                    newPartials = new HashSet<IPattern>();
                }

                foreach (IPattern partial in partials) {
                    // might equal null if something is wrong with the metavariables
                    expressions.Add(partial.ToExpression());
                }

                break; // we want an expression to match only one evaluation rule
            }
        }
        HashSet<Expression> newExpressions = new HashSet<Expression>();
        foreach (Expression e in expressions) {
            newExpressions.Add(e);
            foreach (SubstitutionRule sr in this.substitutionRules) {
                Expression prover = sr.Substitute(e, context);
                if (prover != null) {
                    HashSet<Expression> substitutedExpressions = GenerateSubexpressions(prover, context);
                    foreach (Expression ie in substitutedExpressions) {
                        newExpressions.Add(ie);
                    }
                }
            }
        }

        return newExpressions;
    }

    // return true if this model proves expr.
    public bool Proves(Expression expr) {
        // base case
        if (this.Contains(expr)) {
            return true;
        }

        foreach (InferenceRule ir in this.inferenceRules) {
            if (ir.CanInfer(this, expr, EntailmentContext.Downward)) {
                return true;
            }
        }

        HashSet<Expression> provers = new HashSet<Expression>();

        foreach (SubstitutionRule sr in this.substitutionRules) {
            Expression prover = sr.Substitute(expr, EntailmentContext.Downward);
            if (prover != null) {
                if (this.Contains(prover)) {
                    return true;
                }
                provers.Add(prover);
            }
        }

        foreach (Expression next in provers) {
            if (this.Proves(next)) {
                return true;
            }
        }

        foreach (Expression e in this.GenerateSubexpressions(expr, EntailmentContext.Downward)) {
            if (this.Contains(e)) {
                return true;
            }
        }

        return false;
    }

    public abstract List<Dictionary<MetaVariable, Expression>> Find(IPattern pattern);
}
