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

    // TODO: important. Need to add a "Path" so that
    // copies of the same sentence don't get retried.
    protected HashSet<Expression> GenerateSubexpressions(Expression expr, EntailmentContext context) {
        return null;
        
        // HashSet<Expression> expressions = new HashSet<Expression>();
        // expressions.Add(expr);

        // foreach (EvaluationRule er in this.evaluationRules) {
        //     Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        //     if (er.Matches(expr, bindings)) {
        //         IPattern result = er.result;

        //         foreach (MetaVariable x in bindings.Keys) {
        //             result = result.Bind(x, bindings[x]);
        //         }

        //         HashSet<Expression>[] subExpressions = new HashSet<Expression>[er.Length()];
        //         for (int i = 0; i < er.Length(); i++) {
        //             IPattern pattern = er.Get(i).pattern;

        //             foreach (MetaVariable x in bindings.Keys) {
        //                 pattern = pattern.Bind(x, bindings[x]);
        //             }

        //             subExpressions[i] = GenerateSubexpressions(pattern.ToExpression(), EvaluationPattern.MergeContext(context, er.Get(i).context));
        //         }

        //         HashSet<IPattern> partials = new HashSet<IPattern>();
        //         HashSet<IPattern> newPartials = new HashSet<IPattern>();

        //         if (result == null) {
        //             UnityEngine.Debug.Log("the rule that caused this: " + er);
        //             continue;
        //         }
                
        //         partials.Add(result);
        //         for (int i = 0; i < subExpressions.Length; i++) {
        //             foreach (IPattern partial in partials) {
        //                 foreach (Expression e in subExpressions[i]) {
        //                     if (e != null) {
        //                         newPartials.Add(partial.Bind(new MetaVariable(e.type, -1 * (i + 1)), e));
        //                     }
        //                 }
        //             }
        //             partials = newPartials;
        //             newPartials = new HashSet<IPattern>();
        //         }

        //         foreach (IPattern partial in partials) {
        //             // might equal null if something is wrong with the metavariables
        //             expressions.Add(partial.ToExpression());
        //         }

        //         break; // we want an expression to match only one evaluation rule
        //     }
        // }

        // HashSet<Expression> newExpressions = new HashSet<Expression>();
        // foreach (Expression e in expressions) {
        //     newExpressions.Add(e);
        //     foreach (SubstitutionRule sr in this.substitutionRules) {
        //         List<Expression> substitutions = sr.Substitute(this, e, context);
        //         if (substitutions != null) {
        //             foreach (Expression substitution in substitutions) {
        //                 HashSet<Expression> substitutedExpressions = GenerateSubexpressions(substitution, context);
        //                 foreach (Expression ie in substitutedExpressions) {
        //                     newExpressions.Add(ie);
        //                 }
        //             }
        //         }
        //     }
        // }

        // return newExpressions;
    }

    private static HashSet<Expression> Add(HashSet<Expression> path, Expression expr) {
        HashSet<Expression> newPath = new HashSet<Expression>();
        foreach (Expression e in path) {
            newPath.Add(e);
        }
        newPath.Add(expr);
        return newPath;
    }

    public bool Proves(Expression expr) {
        return Proves(expr, new HashSet<Expression>());
    }

    // return true if this model proves expr.
    protected bool Proves(Expression expr, HashSet<Expression> path) {

        // base case
        if (this.Contains(expr)) {
            return true;
        }

        if (path.Contains(expr)) {
            return false;
        }

        // TODO: make this BFS instead of DFS

        // this should do it for inference chains not involving evaluation
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<List<IPattern>[]> admissibleSubstitutions = sr.Substitute(this, expr, EntailmentContext.Downward);
            
            if (admissibleSubstitutions == null) {
                continue;
            }

            foreach (List<IPattern>[] conjunctSubstitution in admissibleSubstitutions) {
                bool proved = true;

                List<IPattern> toFindList = new List<IPattern>();

                foreach (IPattern p in conjunctSubstitution[0]) {
                    Expression e = p.ToExpression();
                    if (e == null) {
                        toFindList.Add(p);
                    } else if (!this.Proves(e, Add(path, expr))) {
                        proved = false;
                        break;
                    }
                }

                if (!proved) {
                    continue;
                }

                foreach (IPattern p in conjunctSubstitution[1]) {
                    Expression e = p.ToExpression();
                    if (e == null) {
                        toFindList.Add(new ExpressionPattern(Expression.NOT, p));
                    } else if (!this.Proves(new Phrase(Expression.NOT, e), Add(path, expr))) {
                        proved = false;
                        break;
                    }
                }

                if (!proved) {
                    continue;
                }

                if (toFindList.Count == 0) {
                    if (proved) {
                        return true;
                    }
                } else {
                    IPattern[] toFindArray = new IPattern[toFindList.Count];
                        
                    int counter = 0;
                    foreach (IPattern p in toFindList) {
                        toFindArray[counter] = p;
                        counter++;
                    }
                    // TODO: find a way for Find() or something else to RECURSIVELY prove
                    // the potential bindings for use
                    if (this.Find(toFindArray) != null) {
                        return true;
                    }
                }
            }
        }

        // foreach (Expression e in this.GenerateSubexpressions(expr, EntailmentContext.Downward)) {
        //     if (!e.Equals(expr) && this.Proves(e, path.Add(expr))) {
        //         return true;
        //     }
        // }

        return false;
    }

    public abstract HashSet<Dictionary<MetaVariable, Expression>> Find(params IPattern[] patterns);
}
