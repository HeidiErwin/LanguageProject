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

    protected HashSet<Expression> GenerateSubexpressions(Expression expr, EntailmentContext context) {
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
                partials.Add(result);

                for (int i = 0; i < subExpressions.Length; i++) {
                    foreach (IPattern partial in partials) {
                        foreach (Expression e in subExpressions[i]) {
                            partials.Add(partial.Bind(new MetaVariable(e.type, -1 * (i + 1)), e));
                        }
                        partials.Remove(partial);
                    }
                }

                foreach (IPattern partial in partials) {
                    // might equal null if something is wrong with the metavariables
                    expressions.Add(partial.ToExpression());
                }
            }
        }

        foreach (Expression e in expressions) {
            foreach (SubsententialRule sr in this.subsententialRules) {
                Expression prover = sr.Infer(expr, context);
                if (prover != null) {
                    HashSet<Expression> inferredExpressions = GenerateSubexpressions(prover, context);
                    foreach (Expression ie in inferredExpressions) {
                        expressions.Add(ie);
                    }
                }
            }
        }

        return expressions; // TODO
    }

    // return true if this model proves expr.
    public bool Proves(Expression expr) {
        // base case
        if (this.Contains(expr)) {
            return true;
        }

        HashSet<Expression> provers = new HashSet<Expression>();

        foreach (SubsententialRule sr in this.subsententialRules) {
            Expression prover = sr.InferDownward(expr); // will need to be updated eventually
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
}
