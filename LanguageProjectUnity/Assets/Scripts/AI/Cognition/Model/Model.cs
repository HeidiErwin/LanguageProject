using System;
using System.Text;
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
    protected List<EvaluationRule> evaluationRules = new List<EvaluationRule>();
    protected HashSet<SubstitutionRule> substitutionRules = new HashSet<SubstitutionRule>();
    protected Dictionary<SemanticType, HashSet<Expression>> domain = new Dictionary<SemanticType, HashSet<Expression>>();
    protected Dictionary<Expression, bool> triedExpressions;

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
        r.AddToDomain(this);
    }

    public void AddToDomain(Expression e) {
        if (e == null) {
            return;
        }

        if (!domain.ContainsKey(e.type)) {
            domain.Add(e.type, new HashSet<Expression>());
        }

        domain[e.type].Add(e);

        for (int i = 0; i < e.GetNumArgs(); i++) {
            AddToDomain(e.GetArg(i));
            Expression partial = e.Remove(i);
            if (!partial.Equals(e)) {
                AddToDomain(partial);
            }
        }
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
        // Debug.Log("Proving " + expr + "in:");
        // Debug.Log(this);
        triedExpressions = new Dictionary<Expression, bool>();
        return Proves(expr, EntailmentContext.Downward, null);
    }

    // return true if this model proves expr.
    protected bool Proves(Expression expr, EntailmentContext entailmentContext, IPattern sententialContext) {
        // BASE CASES
        if (triedExpressions.ContainsKey(expr)) {
            return triedExpressions[expr];
        }
        triedExpressions.Add(expr, false);

        Expression fullExpression = expr;
        if (sententialContext != null) {
            Expression bound = sententialContext.Bind(new MetaVariable(expr.type, 0), expr).ToExpression();
            if (bound != null) {
                fullExpression = bound;
            }
        }

        // this should be Proves(), not Contains().
        // looping problem needs to be resolved, however.
        if (this.Contains(fullExpression)) {
            triedExpressions[fullExpression] = true;
            return true;
        }

        // RECURSIVE CASES
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<List<IPattern>[]> admissibleSubstitutions = sr.Substitute(this, expr, entailmentContext);
            
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
                    } else if (!this.Proves(e, entailmentContext, sententialContext)) {
                        proved = false;
                        break;
                    }
                }

                if (!proved) {
                    continue;
                }

                foreach (IPattern p in conjunctSubstitution[1]) {
                    if (p == null) {
                        continue;
                    }
                    Expression e = p.ToExpression();
                    if (e == null) {
                        toFindList.Add(new ExpressionPattern(Expression.NOT, p));
                    } else if (!this.Proves(new Phrase(Expression.NOT, e), entailmentContext, sententialContext)) {
                        proved = false;
                        break;
                    }
                }

                if (!proved) {
                    continue;
                }

                if (toFindList.Count == 0) {
                    if (proved) {
                        triedExpressions[expr] = true;
                        return true;
                    }
                } else {
                    IPattern[] toFindArray = new IPattern[toFindList.Count];
                        
                    int counter = 0;
                    foreach (IPattern p in toFindList) {
                        if (p != null) {
                            toFindArray[counter] = p;
                            counter++;
                        }
                    }
                    // TODO: find a way for Find() or something else to RECURSIVELY prove
                    // the potential bindings for use
                    if (this.Find(entailmentContext, sententialContext, toFindArray) != null) {
                        triedExpressions[expr] = true;
                        return true;
                    }
                }
            }
        }

        if (expr.GetHead().Equals(Expression.NOT)) {
            Expression subsentence = expr.GetArg(0);
            if (subsentence != null && Proves(
                    subsentence,
                    EvaluationPattern.MergeContext(entailmentContext, EntailmentContext.Downward),
                    (sententialContext == null) ?
                    new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, 0)) :
                    new ExpressionPattern(Expression.NOT, sententialContext))) {
                // note: the new sententialcontext should actually be: search for the only metavariable
                // and wrap a not around IT. But it doesn't matter so long as negation is the only thing
                // happening.
                return true;
            }
        }

        return false;
    }

    public HashSet<Dictionary<MetaVariable, Expression>> Find(EntailmentContext entailmentContext, IPattern sententialcontext, params IPattern[] patterns) {

        HashSet<Dictionary<MetaVariable, Expression>> successfulBindings = new HashSet<Dictionary<MetaVariable, Expression>>();
        successfulBindings.Add(new Dictionary<MetaVariable, Expression>());
        for (int i = 0; i < patterns.Length; i++) {
            HashSet<Dictionary<MetaVariable, Expression>> newSuccessfulBindings = new HashSet<Dictionary<MetaVariable, Expression>>();
            // 1. for this IPattern, bind all the successful bindings.
            foreach (Dictionary<MetaVariable, Expression> successfulBinding in successfulBindings) {

                // 2. for each possible successful binding, try to supplement it with successful bindings
                // at this current pattern.
                IPattern currentPattern = patterns[i].Bind(successfulBinding);
                
                HashSet<Dictionary<MetaVariable, Expression>> oldAttemptedBindings = new HashSet<Dictionary<MetaVariable, Expression>>();
                oldAttemptedBindings.Add(new Dictionary<MetaVariable, Expression>());

                foreach (MetaVariable x in currentPattern.GetFreeMetaVariables()) {
                    HashSet<Dictionary<MetaVariable, Expression>> newAttemptedBindings = new HashSet<Dictionary<MetaVariable, Expression>>();

                    if (!domain.ContainsKey(x.GetSemanticType())) {
                        return null;
                    }

                    foreach (Expression e in domain[x.GetSemanticType()]) {
                        foreach (Dictionary<MetaVariable, Expression> oldAttemptedBinding in oldAttemptedBindings) {
                            Dictionary<MetaVariable, Expression> newAttemptedBinding = new Dictionary<MetaVariable, Expression>();
                            foreach (KeyValuePair<MetaVariable, Expression> oldPair in oldAttemptedBinding) {
                                newAttemptedBinding.Add(oldPair.Key, oldPair.Value);
                            }
                            newAttemptedBinding.Add(x, e);
                            newAttemptedBindings.Add(newAttemptedBinding);
                        }
                    }

                    oldAttemptedBindings = newAttemptedBindings;
                }                

                bool provedOne = false;
                foreach (Dictionary<MetaVariable, Expression> attemptedBinding in oldAttemptedBindings) {
                    Expression e = currentPattern.Bind(attemptedBinding).ToExpression();
                    // NOTE: e should never be NULL. Problem with domain or GetFreeMetaVariables() otherwise
                    if (e != null && this.Proves(e, entailmentContext, sententialcontext)) {
                        provedOne = true;
                        Dictionary<MetaVariable, Expression> newSuccessfulBinding = new Dictionary<MetaVariable, Expression>();
                        foreach (KeyValuePair<MetaVariable, Expression> kv in successfulBinding) {
                            newSuccessfulBinding.Add(kv.Key, kv.Value);
                        }
                        foreach (KeyValuePair<MetaVariable, Expression> kv in attemptedBinding) {
                            newSuccessfulBinding.Add(kv.Key, kv.Value);
                        }
                        newSuccessfulBindings.Add(newSuccessfulBinding);
                    }
                }
                if (!provedOne) {
                    return null;
                }
            }
            successfulBindings = newSuccessfulBindings;
        }

        return successfulBindings;
    }

    public String DomainString() {
        StringBuilder s = new StringBuilder();

        s.Append("BEGIN DOMAIN\n");

        foreach (SemanticType t in domain.Keys) {
            s.Append(t);
            s.Append(":\n");
            foreach (Expression e in domain[t]) {
                s.Append("\t" + e + "\n");
            }
        }
        s.Append("END DOMAIN\n");
        return s.ToString();
    }

    public static String ConditionsString(params IPattern[] patterns) {
        StringBuilder s = new StringBuilder();
        s.Append("BEGIN CONDITIONS\n");

        foreach (IPattern pattern in patterns) {
            s.Append("\t" + pattern + "\n");
        }

        s.Append("END CONDITIONS");

        return s.ToString();
    }

    public static String BindingString(Dictionary<MetaVariable, Expression> bindings) {
        StringBuilder s = new StringBuilder();
        s.Append("{\n");
        foreach (KeyValuePair<MetaVariable, Expression> kv in bindings) {
            s.Append("\t" + kv.Key + " |-> " + kv.Value + "\n");
        }
        s.Append("}\n");

        return s.ToString();
    }

    // protected List<List<Expression>[]> UnfurlSubstitution(List<List<IPattern>[]> substitutions) {
    //     List<List<Expression>[]> substitutionExpressions = new List<List<Expression>[]>();
        
    //     foreach (List<IPattern>[] conjunctionPattern in substitutions) {
    //         foreach ([]) {

    //         }
    //     }
    // }

    // TODO: currently this is being bypassed, but return to this
    // once you have a better idea of how to do it.
    // protected HashSet<Expression> GenerateSubexpressions(Expression expr, EntailmentContext context) {
        // HashSet<Expression> expressions = new HashSet<Expression>();
        // expressions.Add(expr);

        // BEGIN NEW CODE
        // foreach (EvaluationRule er in this.evaluationRules) {
        //     Dictionary<MetaVariable, Expression> bindings = er.GetBindings(expr);
        //     if (bindings != null) {
        //         IPattern result = er.result.Bind(bindings);

        //         List<IPattern> results = new List<IPattern>();
        //         results.Add(result);
        //         List<IPattern> newResults = new List<IPattern>();

        //         for (int i = 0; i < er.Length(); i++) {
        //             Expression argument = er.Get(i).pattern.Bind(bindings).ToExpression();

        //             if (argument == null) {
        //                 break;
        //             }

        //             foreach (SubstitutionRule sr in this.substitutionRules) {
        //                 List<List<IPattern>[]> substitutions = 
        //                     sr.Substitute(this, argument, EvaluationPattern.MergeContext(context, er.Get(i).context));

        //                 foreach (List<IPattern>[] conjunctionPattern in substitutions) {
        //                     foreach (IPattern p in conjunction[0]) {

        //                     }
        //                 }
        //                 // go through all the 
        //             }
        //         }
        //     }
        // }
        // 
        // END NEW CODE

        // return null;
            
        // BEGIN OLD CODE
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
        // END OLD CODE

        // return newExpressions;
    // }
}
