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

    public List<Expression> Plan(Expression goal) {
        return Plan(goal, new List<Expression>());
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

        // TODO: make it so, if there's no plan for this goal, form a
        // plan for what would entail the goal
        // foreach (SubstitutionRule sr in this.substitutionRules) {
        // }

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
}
