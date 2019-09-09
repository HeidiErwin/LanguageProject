using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public enum EvidentialSource {
    Perception,
    Testimony,
    Expectation,
    Inference,
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
    protected HashSet<SubstitutionRule> substitutionRules = new HashSet<SubstitutionRule>();
    protected HashSet<ActionRule> actionRules = new HashSet<ActionRule>();
    // protected HashSet<Expression> primitiveAbilites = new HashSet<IPattern>();
    protected Dictionary<SemanticType, HashSet<Expression>> domain = new Dictionary<SemanticType, HashSet<Expression>>();
    protected Dictionary<Expression, HashSet<Expression>> triedExpressions = new Dictionary<Expression, HashSet<Expression>>();
    protected List<Expression> lastSuppositions = new List<Expression>();

    // the utilities of certain sentences.
    // Ultimately, we want this to be a priority queue
    // so that we can quickly get the maximum element
    protected Dictionary<Expression, float> utilities = new Dictionary<Expression, float>();
    protected Expression currentGoal;
    public List<Expression> currentPlan { get; protected set; }
    public bool decisionLock = false;

    // loop detection
    private int loopCounter = 0;

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

    public abstract HashSet<Expression> GetAll();

    // Adding rules into the model
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

    public float GetUtility(Expression e) {
        return utilities[e];
    }

    public void SetUtility(Expression e, float u) {
        utilities[e] = u;
        decisionLock = false;
    }

    public void AddUtility(Expression e, float du) {
        if (utilities.ContainsKey(e)) {
            utilities[e] += du;
        } else {
            utilities[e] = du;
        }
        decisionLock = false;
    }

    public void AddToDomain(Expression e) {
        if (e == null) {
            return;
        }

        if (!domain.ContainsKey(e.type)) {
            domain.Add(e.type, new HashSet<Expression>());    
        }

        if (!e.type.Equals(SemanticType.TRUTH_VALUE)) {
            // TODO add an exception clause for conjunction/disjunction
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

    // public void SetUtility(Expression expr, float utility) {
    //     this.utilities[expr] = utility;
    // }

    // OIT 1, NIT 2, OE 3, Inf 4, OCT 5, OP 6, NCT 7, NE 8, NP 9, B 10
    public int EstimatePlausibility(Expression e, bool isNew) {
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        IPattern perceptionPattern = new ExpressionPattern(Expression.PERCEIVE, Expression.SELF, xt0);
        IPattern testimonyPattern = new ExpressionPattern(Expression.BELIEVE, xi0, xt0);
        IPattern expectationPattern = new ExpressionPattern(Expression.MAKE, Expression.SELF, xt0);

        List<Dictionary<MetaVariable, Expression>> bindings = perceptionPattern.GetBindings(e);
        if (bindings != null) {
            if (isNew) {
                return 9;
            } else {
                return 6;
            }
        }

        bindings = testimonyPattern.GetBindings(e);
        if (bindings != null) {
            bool isCredible = true; // this.Proves(new Phrase(Expression.CREDIBLE, bindings[0][xi0]));
            if (isCredible) {
                if (isNew) {
                    return 7;    
                }
                return 5;
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
                return 8;
            }
            return 3;
        }

        if (this.Contains(e)) {
            return 10;
        }

        if (e.Equals(new Word(SemanticType.TRUTH_VALUE, "example"))) {
            return 4;
        }

        if (e.Equals(new Phrase(Expression.NOT, new Word(SemanticType.TRUTH_VALUE, "example")))) {
            return 5;
        }

        return 4;
    }

    // returns true if the belief is accepted
    // returns false if the belief is rejected    
    // TODO: make a more sophicated update policy
    public bool UpdateBelief(Expression input) {
        if (this.Proves(input)) {
            return true;
        }

        List<Expression> removed = new List<Expression>();
        Expression negatedInput = new Phrase(Expression.NOT, input);
        HashSet<Expression> basis = GetBasis(negatedInput);

        while (basis != null) {
            Expression leastPlausible = input;
            int lowestPlausibility = EstimatePlausibility(leastPlausible, true);
            foreach (Expression e in basis) {
                int currentPlausibility = EstimatePlausibility(e, false);
                if (currentPlausibility < lowestPlausibility) {
                    leastPlausible = e;
                    lowestPlausibility = currentPlausibility;
                }
            }
            
            if (leastPlausible.Equals(input)) {
                foreach (Expression e in removed) {
                    this.Remove(new Phrase(Expression.NOT, e));
                    this.Add(e);
                }
                return false;
            }

            removed.Add(leastPlausible);
            if (this.Contains(leastPlausible)) {
                this.Remove(leastPlausible);
            } else {
                this.Add(new Phrase(Expression.NOT, leastPlausible));
            }
            
            triedExpressions.Clear();
            basis = GetBasis(negatedInput);
        }
        triedExpressions.Clear();
        this.Add(input);
        this.decisionLock = false;
        return true;
    }

    private static List<T> ImAdd<T>(T item, List<T> list) {
        List<T> newList = new List<T>();
        foreach (T x in list) {
            newList.Add(x);
        }
        newList.Add(item);
        return newList;
    }

    private static HashSet<T> ImAdd<T>(T item, HashSet<T> set) {
        HashSet<T> newSet = new HashSet<T>();
        foreach (T x in set) {
            newSet.Add(x);
        }
        newSet.Add(item);
        return newSet;
    }

    public bool ClearGoal() {
        if (currentGoal == null) {
            return false;
        }

        currentGoal = null;
        currentPlan = null;
        return true;
    }

    // right now, the cost of actions isn't taken into account.
    // So we first choose a goal based on what we instrinsically
    // value most, and then plan to satisfy that goal state.
    // This still maximizes expected value (except re: action cost)
    // since it only accepts a goal which is thought to be achievable
    // (i.e. it's expectation is not 0)
    public void DecideGoal() {
        Expression maxGoal = null;
        List<Expression> maxPlan = null;
        float maxUtility = Single.NegativeInfinity;
        foreach (KeyValuePair<Expression, float> us in this.utilities) {
            // Debug.Log(us.Key);
            if (us.Value >= maxUtility) {
                List<Expression> plan = Plan(us.Key);
                // if the goal is already true, or isn't
                // achievable, then don't set this as the goal.
                if (plan != null && plan.Count != 0) {
                    maxGoal = us.Key;
                    maxPlan = plan;
                    maxUtility = us.Value;
                }
            }
        }
        if (maxGoal == null) {
            decisionLock = true;
        }
        currentGoal = maxGoal;
        currentPlan = maxPlan;
    }

    public List<Expression> Plan(Expression goal) {
        return Plan(goal, new List<Expression>(), new HashSet<Expression>());
    }

    // naive, non-schematic action planner
    public List<Expression> Plan(Expression goal, List<Expression> actionSequence, HashSet<Expression> tried) {
        if (tried.Contains(goal)) {
            return null;
        }
        
        // in this case, we've reach a point where the goal has been satisfied
        if (Proves(goal)) {
            return actionSequence;
        }

        List<List<Expression>> possibleActions = new List<List<Expression>>();

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        // MODUS PONENS: checking to see if anything in the model satisifies
        // X, (X -> goal)
        IPattern consequentPattern =
            new ExpressionPattern(Expression.IF, xt0, goal);

        foreach (Expression e in GetAll()) {
            List<Dictionary<MetaVariable, Expression>> antecedents = consequentPattern.GetBindings(e);
            if (antecedents == null) {
                continue;
            }
            Expression antecedent = antecedents[0][xt0];
            List<Expression> antecedentSequence = Plan(antecedent, actionSequence, ImAdd<Expression>(goal, tried));
            if (antecedentSequence != null) {
                possibleActions.Add(antecedentSequence);
            }
        }

        // END MODUS PONENS

        IPattern secondOrderAttitudePattern =
            new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0),
                new MetaVariable(SemanticType.INDIVIDUAL, 0),
                new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 1),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1),
                    new MetaVariable(SemanticType.TRUTH_VALUE, 0)));

        if (secondOrderAttitudePattern.Matches(goal)) {
            return null;
        }

        // search through action rules and recur on their preconditions as subgoals.
        foreach (ActionRule r in this.actionRules) {
            List<Dictionary<MetaVariable, Expression>> bindings = r.result.GetBindings(goal);
            if (bindings != null) {
                Expression action = r.action.ToExpression();
                
                // TODO accept multiple matches
                if (bindings.Count > 0) {
                    action = r.action.Bind(bindings[0]).ToExpression();
                }
                if (action == null) {
                    // TODO handle binding issues later
                    return null;
                }
                List<Expression> newActionSequence = new List<Expression>();
                newActionSequence.Add(action);
                foreach (Expression a in actionSequence) {
                    newActionSequence.Add(a);
                }

                Expression newGoal = r.condition.ToExpression();

                if (bindings.Count > 0) {
                    newGoal = r.condition.Bind(bindings[0]).ToExpression();
                }

                List<Expression> solvedSequence = Plan(newGoal, newActionSequence, ImAdd<Expression>(goal, tried));
                if (solvedSequence != null) {
                    possibleActions.Add(solvedSequence);
                    // return solvedSequence;
                }
            }
        }

        // TODO: make it so, if there's no plan for this goal, form a
        // plan for what would entail the goal
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<SubstitutionRule.Result> admissibleSubstitutions = sr.Substitute(this, new List<Expression>(), goal);
            if (admissibleSubstitutions == null) {
                continue;
            }
            foreach (SubstitutionRule.Result cnfs in admissibleSubstitutions) {
                List<Expression> subgoals = new List<Expression>();

                // TODO: add Find() functionality eventually (bleh)
                bool bound = true;
                foreach (IPattern p in cnfs.positives) {
                    Expression positive = p.ToExpression();
                    if (positive != null) {
                        subgoals.Add(positive);
                    } else {
                        bound = false;
                        break;
                    }
                }
                if (!bound) {
                    continue;
                }

                foreach (IPattern p in cnfs.negatives) {
                    Expression negative = p.ToExpression();
                    if (negative != null) {
                        subgoals.Add(negative);
                    } else {
                        bound = false;
                        break;
                    }
                }

                if (!bound) {
                    continue;
                }

                List<Expression> solvedSubsequence = actionSequence;
                bool solved = true;
                foreach (Expression subgoal in subgoals) {
                    solvedSubsequence = Plan(subgoal, solvedSubsequence, ImAdd<Expression>(goal, tried));
                    if (solvedSubsequence == null) {
                        solved = false;
                        break;
                    }
                }
                if (solved) {
                    possibleActions.Add(solvedSubsequence);
                    // return solvedSubsequence;
                }
            }
        }

        // if there are known courses of action, choose among the best of them.
        // if there aren't any, then return NULL.
        List<Expression> bestCourse = null;

        foreach (List<Expression> course in possibleActions) {
            // eventually, this should be a utility estimate. For now,
            // it simply counts how many actions are required.
            if (bestCourse == null || bestCourse.Count > course.Count) {
                bestCourse = course;
            }
        }

        return bestCourse;
    }

    public bool Proves(Expression expr) {
        // triedExpressions = new Dictionary<Expression, HashSet<Expression>>();
        return GetBasis(expr) != null;
    }

    public HashSet<Expression> GetBasis(Expression expr) {
        this.loopCounter = 0;
        this.lastSuppositions = new List<Expression>();
        return GetBasis(expr, new List<Expression>());
    }

    // returns the set of sentences that prove expr, if any.
    // returns NULL if this model doesn't prove expr.
    public HashSet<Expression> GetBasis(Expression expr, List<Expression> suppositions) {
        if (this.loopCounter > 100) {
            Debug.Log("LOOP FAILURE: " + expr);
            return null;
        }

        // BEGIN: checking to see if suppositions match.
        // If they we need to reset our tried proofs.
        foreach (Expression supposition in suppositions) {
            if (!this.lastSuppositions.Contains(supposition)) {
                triedExpressions.Clear();
            }
        }

        foreach (Expression supposition in this.lastSuppositions) {
            if (!suppositions.Contains(supposition)) {
                triedExpressions.Clear();
            }
        }

        // END supposition check

        HashSet<Expression> basis = new HashSet<Expression>();
        if (suppositions.Contains(expr)) {
            return basis;
        }

        // BASE CASES
        if (triedExpressions.ContainsKey(expr)) {
            return triedExpressions[expr];
        }

        triedExpressions.Add(expr, null);
        this.loopCounter++;
        // Debug.Log("Trying to prove '" + expr + "'");

        if (this.Contains(expr)) {
            basis.Add(expr);
            triedExpressions[expr] = basis;
            return basis;
        }

        if (this.Contains(new Phrase(Expression.NOT, expr))) {
            return null;
        }

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);

        // MODUS PONENS: checking to see if anything in the model satisifies
        // X, X -> expr
        IPattern consequentPattern =
            new ExpressionPattern(Expression.IF, xt0, expr);

        foreach (Expression e in GetAll()) {
            List<Dictionary<MetaVariable, Expression>> antecedents = consequentPattern.GetBindings(e);
            if (antecedents == null) {
                continue;
            }
            Expression antecedent = antecedents[0][xt0];
            HashSet<Expression> antecedentBasis = GetBasis(antecedent);
            if (antecedentBasis != null) {
                antecedentBasis.Add(new Phrase(Expression.IF, antecedent, expr));
                triedExpressions[expr] = antecedentBasis;
                return antecedentBasis;
            }
        }

        // END MODUS PONENS

        // BOUND
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        IPattern trustworthyPattern =
            new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.TRUSTWORTHY, xi0));

        List<Dictionary<MetaVariable, Expression>> promiser = trustworthyPattern.GetBindings(expr);
        if (promiser != null) {
            IPattern promisePattern = new ExpressionPattern(Expression.BOUND, promiser[0][xi0], xt0);

            foreach (Expression e in GetAll()) {
                List<Dictionary<MetaVariable, Expression>> promises = promisePattern.GetBindings(e);
                if (promises == null) {
                    continue;
                }
                Expression promise = promises[0][xt0];
                HashSet<Expression> promiseBasis = GetBasis(new Phrase(Expression.NOT, promise));
                if (promiseBasis != null) {
                    basis.Add(new Phrase(Expression.BOUND, promiser[0][xi0], promise));
                    basis.Add(new Phrase(Expression.NOT, promise));
                    triedExpressions[expr] = basis;
                    return basis;
                }
            }
        }

        // END BOUND

        IPattern secondOrderAttitudePattern =
            new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0),
                new MetaVariable(SemanticType.INDIVIDUAL, 0),
                new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 1),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1),
                    new MetaVariable(SemanticType.TRUTH_VALUE, 0)));

        if (secondOrderAttitudePattern.Matches(expr)) {
            return null;
        }

        // CONDITIIONAL PROOF
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);

        IPattern conditionalPattern = new ExpressionPattern(Expression.IF, xt0, xt1);

        List<Dictionary<MetaVariable, Expression>> conditionalBindings = conditionalPattern.GetBindings(expr);

        if (conditionalBindings != null) {
            return GetBasis(conditionalBindings[0][xt1], ImAdd(conditionalBindings[0][xt0], suppositions));
        }

        // END CONDITIONAL PROOF

        // RECURSIVE CASES
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<SubstitutionRule.Result> admissibleSubstitutions = sr.Substitute(this, suppositions, expr);
            
            if (admissibleSubstitutions == null) {
                continue;
            }

            foreach (SubstitutionRule.Result conjunctSubstitution in admissibleSubstitutions) {
                basis.Clear();
                bool proved = true;

                List<IPattern> toFindList = new List<IPattern>();

                foreach (IPattern p in conjunctSubstitution.positives) {
                    Expression e = p.ToExpression();
                    if (e == null) {
                        toFindList.Add(p);
                    } else {
                        HashSet<Expression> subBasis = this.GetBasis(e, suppositions);
                        if (subBasis == null) {
                            proved = false;
                            break;
                        } else {
                            foreach (Expression b in subBasis) {
                                basis.Add(b);
                            }
                        }
                    }
                }

                if (!proved) {
                    continue;
                }

                foreach (IPattern p in conjunctSubstitution.negatives) {
                    if (p == null) {
                        continue;
                    }
                    Expression e = p.ToExpression();
                    if (e == null) {
                        toFindList.Add(new ExpressionPattern(Expression.NOT, p));
                    } else {
                        HashSet<Expression> subBasis = this.GetBasis(new Phrase(Expression.NOT, e), suppositions);
                        if (subBasis == null) {
                            proved = false;
                            break;
                        } else {
                            foreach (Expression b in subBasis) {
                                basis.Add(b);
                            }
                        }
                    }
                }

                foreach (IPattern p in conjunctSubstitution.assumptions) {
                    if (p == null) {
                        continue;
                    }
                    Expression e = p.ToExpression();
                    if (e != null) {
                        if (Contains(new Phrase(Expression.NOT, e))) {
                            proved = false;
                            break;
                        }
                    }
                }

                if (!proved) {
                    continue;
                }

                if (toFindList.Count == 0) {
                    if (proved) {
                        foreach (IPattern p in conjunctSubstitution.assumptions) {
                            Expression e = p.ToExpression();
                            basis.Add(e);
                        }
                        triedExpressions[expr] = basis;
                        return basis;
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
                    List<Dictionary<MetaVariable, Expression>> bindings = Find(suppositions, toFindArray);
                    Dictionary<MetaVariable, Expression> provedBinding = null;
                    if (bindings != null) {
                        foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
                            foreach (IPattern p in toFindList) {
                                Expression fullyBound = p.Bind(binding).ToExpression();
                                if (fullyBound == null) {
                                    proved = false;
                                    break;
                                }
                                HashSet<Expression> subBasis = GetBasis(fullyBound, suppositions);
                                if (subBasis == null) {
                                    proved = false;
                                    break;
                                } else {
                                    provedBinding = binding;
                                    foreach (Expression b in subBasis) {
                                        basis.Add(b);    
                                    }
                                }
                            }
                            if (!proved) {
                                break;
                            }
                        }

                        foreach (IPattern p in conjunctSubstitution.assumptions) {
                            Expression fullyBound = p.Bind(provedBinding).ToExpression();
                            basis.Add(fullyBound);
                        }
                        return basis;
                    }
                }
            }
        }
        return null;
    }

    public List<Dictionary<MetaVariable, Expression>> Find(List<Expression> suppositions, params IPattern[] patterns) {
        List<Dictionary<MetaVariable, Expression>> successfulBindings = new List<Dictionary<MetaVariable, Expression>>();
        successfulBindings.Add(new Dictionary<MetaVariable, Expression>());
        for (int i = 0; i < patterns.Length; i++) {
            List<Dictionary<MetaVariable, Expression>> newSuccessfulBindings = new List<Dictionary<MetaVariable, Expression>>();
            // 1. for this IPattern, bind all the successful bindings.
            bool provedOne = false;
            foreach (Dictionary<MetaVariable, Expression> successfulBinding in successfulBindings) {

                // 2. for each possible successful binding, try to supplement it with successful bindings
                // at this current pattern.
                IPattern currentPattern = patterns[i].Bind(successfulBinding);
                
                List<Dictionary<MetaVariable, Expression>> oldAttemptedBindings = new List<Dictionary<MetaVariable, Expression>>();
                oldAttemptedBindings.Add(new Dictionary<MetaVariable, Expression>());

                foreach (MetaVariable x in currentPattern.GetFreeMetaVariables()) {
                    List<Dictionary<MetaVariable, Expression>> newAttemptedBindings = new List<Dictionary<MetaVariable, Expression>>();

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

                foreach (Dictionary<MetaVariable, Expression> attemptedBinding in oldAttemptedBindings) {
                    Expression e = currentPattern.Bind(attemptedBinding).ToExpression();
                    // NOTE: e should never be NULL. Problem with domain or GetFreeMetaVariables() otherwise
                    if (e != null && (this.GetBasis(e, suppositions) != null)) {
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
