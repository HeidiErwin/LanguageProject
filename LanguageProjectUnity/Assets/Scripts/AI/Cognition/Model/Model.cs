using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum EvidentialSource {
    Perception,
    Testimony,
    Expectation,
    Base
}

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
    protected HashSet<ActionRule> actionRules = new HashSet<ActionRule>();
    // protected HashSet<Expression> primitiveAbilites = new HashSet<ActionRule>();
    protected Dictionary<SemanticType, HashSet<Expression>> domain = new Dictionary<SemanticType, HashSet<Expression>>();
    protected Dictionary<Expression, bool> triedExpressions;
    protected HashSet<Expression> proofBase;

    // a queue of goals to be performed in sequence
    // protected List<Expression> goals;
    // protected Dictionary<Expression, float> utilities = new Dictionary<Expression, float>();

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

        if (r.isTransposable) {
            substitutionRules.Add(r.Contrapositive());
        }
    }

    public void Add(ActionRule r) {
        actionRules.Add(r);
    }

    public void AddToDomain(Expression e) {
        if (e == null) {
            return;
        }

        if (!domain.ContainsKey(e.type)) {
            domain.Add(e.type, new HashSet<Expression>());    
        }

        if (!e.type.Equals(SemanticType.TRUTH_VALUE)) {
            domain[e.type].Add(e);
        }
        
        for (int i = 0; i < e.GetNumArgs(); i++) {
            AddToDomain(e.GetArg(i));
            Expression partial = e.Remove(i);
            if (!partial.Equals(e)) {
                AddToDomain(partial);
            }
        }
    }

    public IEnumerator<bool> CoProves(Expression expr) {
        yield return Proves(expr);
    }

    public bool Proves(Expression expr) {
        triedExpressions = new Dictionary<Expression, bool>();
        proofBase = new HashSet<Expression>();
        return Proves(expr, null);
    }

    // public void SetUtility(Expression expr, float utility) {
    //     this.utilities[expr] = utility;
    // }

    // OIT 1, NIT 2, OE 3, OCT 4, OP 5, NCT 6, NE 7, NP 8, B 9
    public int EstimatePlausibility(Expression e, bool isNew) {
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        IPattern perceptionPattern = new ExpressionPattern(Expression.PERCEIVE, Expression.SELF, xt0);
        IPattern testimonyPattern = new ExpressionPattern(Expression.BELIEVE, xi0, xt0);
        IPattern expectationPattern = new ExpressionPattern(Expression.MAKE, Expression.SELF, xt0);

        List<Dictionary<MetaVariable, Expression>> bindings = perceptionPattern.GetBindings(e);
        if (bindings != null) {
            if (isNew) {
                return 8;
            } else {
                return 5;
            }
        }

        bindings = testimonyPattern.GetBindings(e);
        if (bindings != null) {
            bool isCredible = this.Proves(new Phrase(Expression.CREDIBLE, bindings[0][xi0]));
            if (isCredible) {
                if (isNew) {
                    return 6;    
                }
                return 4;
            } else {
                if (isNew) {
                    return 2;
                }
                return 1;
            }
        }

        bindings = expectationPattern.GetBindings(e);
        if (bindings != null) {
            if (isNew) {
                return 7;
            }
            return 3;
        }

        return 9;
    }

    // returns true if the belief is accepted
    // returns false if the belief is rejected    
    // TODO: make a more sophicated update policy
    public bool UpdateBelief(Expression input) {
        List<Expression> toRemove = new List<Expression>();
        while (this.Proves(new Phrase(Expression.NOT, input))) {
            Expression leastPlausible = input;
            int lowestPlausibility = EstimatePlausibility(input, true);
            foreach (Expression e in this.proofBase) {
                int currentPlausibility = EstimatePlausibility(e, false);
                if (currentPlausibility < lowestPlausibility) {
                    leastPlausible = e;
                    lowestPlausibility = currentPlausibility;
                }
            }
            if (leastPlausible.Equals(input)) {
                return false;
            }
            toRemove.Add(leastPlausible);
        }

        foreach (Expression e in toRemove) {
            this.Remove(e);
        }

        this.Add(input);
        return true;
    }

    // naive, non-schematic action planner
    public List<Expression> Plan(Expression goal, List<Expression> actionSequence) {
        // in this case, we've reach a point where the goal has been satisfied
        if (Proves(goal)) {
            return actionSequence;
        }

        // search through action rules and recur on their preconditions as subgoals.
        foreach (ActionRule r in this.actionRules) {
            if (r.result.Equals(goal)) {
                List<Expression> newActionSequence = new List<Expression>();
                
                newActionSequence.Add(r.action);
                foreach (Expression a in actionSequence) {
                    newActionSequence.Add(a);
                }

                List<Expression> solvedSequence = Plan(r.condition, newActionSequence);
                if (solvedSequence != null) {
                    return solvedSequence;
                }
            }
        }

        // no known courses of action bring about the intended result.
        return null;
    }

    // return true if this model proves expr.
    protected bool Proves(Expression expr, object xxxx) {
        // BASE CASES
        if (triedExpressions.ContainsKey(expr)) {
            return triedExpressions[expr];
        }
        triedExpressions.Add(expr, false);

        // this should be Proves(), not Contains().
        // looping problem needs to be resolved, however.
        if (this.Contains(expr)) {
            triedExpressions[expr] = true;
            proofBase.Add(expr);
            return true;
        }

        IPattern secondOrderAttitudePattern =
            new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0),
                new MetaVariable(SemanticType.INDIVIDUAL, 0),
                new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 1),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1),
                    new MetaVariable(SemanticType.TRUTH_VALUE, 0)));

        if (secondOrderAttitudePattern.Matches(expr)) {
            return false;
        }

        // RECURSIVE CASES
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<List<IPattern>[]> admissibleSubstitutions = sr.Substitute(this, expr);
            
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
                    } else if (!this.Proves(e, null)) {
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
                    } else if (!this.Proves(new Phrase(Expression.NOT, e), null)) {
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
                    if (this.Find(toFindArray) != null) {
                        triedExpressions[expr] = true;
                        return true;
                    }
                }
            }
        }
        proofBase = new HashSet<Expression>();
        return false;
    }

    public HashSet<Dictionary<MetaVariable, Expression>> Find(params IPattern[] patterns) {
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
                    if (e != null && this.Proves(e, null)) {
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
