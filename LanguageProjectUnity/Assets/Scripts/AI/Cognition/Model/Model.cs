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
    // protected Dictionary<Expression, float> utilities = new Dictionary<Expression, float>();
    protected HashSet<Expression> preferables = new HashSet<Expression>();
    protected Expression currentGoal;
    public List<Expression> currentPlan { get; protected set; }
    public bool decisionLock = false;

    // loop detection
    private int loopCounter = 0;
    private bool isLastPlan = false;

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
        } else {
            if (e.GetHead().Equals(Expression.BETTER) || e.GetHead().Equals(Expression.AS_GOOD_AS)) {
                preferables.Add(e.GetArg(0));
                preferables.Add(e.GetArg(1));
            }
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
        decisionLock = false;
        return true;
    }

    // right now, the cost of actions isn't taken into account.
    // So we first choose a goal based on what we instrinsically
    // value most, and then plan to satisfy that goal state.
    // This still maximizes expected value (except re: action cost)
    // since it only accepts a goal which is thought to be achievable
    // (i.e. it's expectation is not 0)
    public void DecideGoal() {
        Expression maxGoal = Expression.NEUTRAL;
        List<Expression> maxPlan = null;
        // float maxUtility = Single.NegativeInfinity;

        foreach (Expression preferable in this.preferables) {
            if (this.Proves(new Phrase(Expression.BETTER, preferable, maxGoal))) {
                List<Expression> plan = Plan(preferable);
                if (plan != null && plan.Count != 0) {
                    maxGoal = preferable;
                    maxPlan = plan;
                }
            }
        }

        if (maxGoal.Equals(Expression.NEUTRAL)) {
            decisionLock = true;
        }
        currentGoal = maxGoal;
        currentPlan = maxPlan;
    }

    // TODO make it so plan works through multiple plans
    // and picks the best one
    public List<Expression> Plan(Expression goal) {
        HashSet<Expression> planBasis = GetBasis(true, goal);
        if (planBasis == null) {
            return null;
        }
        List<Expression> plan = new List<Expression>();
        foreach (Expression p in planBasis) {
            if (p.GetHead().Equals(Expression.WOULD)) {
                plan.Add(p);
            }
        }
        return plan;
    }

    public bool Proves(Expression expr) {
        // triedExpressions = new Dictionary<Expression, HashSet<Expression>>();
        return GetBasis(expr) != null;
    }

    public HashSet<Expression> GetBasis(Expression expr) {
        return GetBasis(false, expr);
    }

    public HashSet<Expression> GetBasis(bool plan, Expression expr) {
        this.loopCounter = 0;
        this.lastSuppositions = new List<Expression>();
        return GetBasis(plan, expr, new List<Expression>());
    }

    public HashSet<Expression> GetBasis(Expression expr, List<Expression> suppositions) {
        return GetBasis(false, expr, suppositions);
    }

    // returns the set of sentences that prove expr, if any.
    // returns NULL if this model doesn't prove expr.
    // TODO: for planning, need to sequence and compare
    // costs from multiple paths
    public HashSet<Expression> GetBasis(bool plan, Expression expr, List<Expression> suppositions) {
        if (this.loopCounter > 100) {
            return null;
        }

        IPattern secondOrderAttitudePattern =
            new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0),
                new MetaVariable(SemanticType.INDIVIDUAL, 0),
                new ExpressionPattern(new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 1),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1),
                    new MetaVariable(SemanticType.TRUTH_VALUE, 0)));

        if (secondOrderAttitudePattern.Matches(expr)) {
            return null;
        }

        // if it's a plan instead of a proof or vice versa,
        // we need to reset
        if (isLastPlan != plan) {
            triedExpressions.Clear();
        }
        isLastPlan = plan;

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

        if (this.Contains(expr)) {
            basis.Add(expr);
            triedExpressions[expr] = basis;
            return basis;
        }

        if (!expr.type.Equals(SemanticType.TRUTH_VALUE)) {
            return null;
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
            HashSet<Expression> antecedentBasis = GetBasis(plan, antecedent, suppositions);
            if (antecedentBasis != null) {
                antecedentBasis.Add(new Phrase(Expression.IF, antecedent, expr));
                triedExpressions[expr] = antecedentBasis;
                return antecedentBasis;
            }
        }

        // END MODUS PONENS

        // UNIVERSAL ELIMINATION
        MetaVariable xp0 = new MetaVariable(SemanticType.PREDICATE, 0);
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        IPattern predicatePattern =
            new ExpressionPattern(xp0, xi0);

        List<Dictionary<MetaVariable, Expression>> decomposition = predicatePattern.GetBindings(expr);
        if (decomposition != null) {
            foreach (Dictionary<MetaVariable, Expression> binding in decomposition) {
                Expression subject = binding[xi0];
                Expression predicate = binding[xp0];

                IPattern universalPattern =
                    new ExpressionPattern(Expression.ALL, xp0, predicate);

                foreach (Expression e in GetAll()) {
                    List<Dictionary<MetaVariable, Expression>> domains =
                        universalPattern.GetBindings(e);

                    if (domains == null) {
                        continue;
                    }

                    Expression domain = domains[0][xp0];
                    HashSet<Expression> domainBasis = GetBasis(plan, new Phrase(domain, subject), suppositions);
                    if (domainBasis != null) {
                        domainBasis.Add(new Phrase(Expression.ALL, domain, predicate));
                        triedExpressions[expr] = domainBasis;
                        return domainBasis;
                    }
                }
            }
        }

        // END UNIVERSAL ELIMINATION

        // SENTENTIAL ATTITUDES
        Expression perceiveExpression = new Phrase(Expression.PERCEIVE, Expression.SELF, expr);
        if (this.Contains(perceiveExpression)) {
            basis.Add(perceiveExpression);
            basis.Add(new Phrase(Expression.VERIDICAL, Expression.SELF, expr));
        }

        IPattern believePattern = new ExpressionPattern(Expression.BELIEVE, xi0, expr);
        IPattern makePattern = new ExpressionPattern(Expression.MAKE, xi0, expr);

        foreach (Expression e in GetAll()) {
            List<Dictionary<MetaVariable, Expression>> believeBindings = believePattern.GetBindings(e);
            if (believeBindings != null) {
                Expression believer = believeBindings[0][xi0];
                basis.Add(new Phrase(Expression.BELIEVE, believer, e));
                basis.Add(new Phrase(Expression.CORRECT, believer, e));
                return basis;
            }

            List<Dictionary<MetaVariable, Expression>> makeBindings = makePattern.GetBindings(e);
            if (makeBindings != null) {
                Expression maker = makeBindings[0][xi0];
                basis.Add(new Phrase(Expression.MAKE, maker, e));
                basis.Add(new Phrase(Expression.SUCCESS, maker, e));
                return basis;
            }
        }
        // END SENTENTIAL ATTITUDES

        // TRANSITIVITY OF BETTER
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        IPattern betterPattern = new ExpressionPattern(Expression.BETTER, xt0, xt1);

        List<Dictionary<MetaVariable, Expression>> preferands = betterPattern.GetBindings(expr);

        if (preferands != null) {
            Expression better = preferands[0][xt0];
            Expression worse = preferands[0][xt1];

            foreach (Expression p in preferables) {
                HashSet<Expression> betterBasis = 
                    GetBasis(plan, new Phrase(Expression.BETTER, better, p), suppositions);

                if (betterBasis != null) {
                    HashSet<Expression> worseBasis =
                        GetBasis(plan, new Phrase(Expression.BETTER, p,  worse), suppositions);

                    if (worseBasis != null) {
                        HashSet<Expression> transitiveBasis = new HashSet<Expression>();
                        foreach (Expression b in betterBasis) {
                            transitiveBasis.Add(b);
                        }
                        foreach (Expression b in worseBasis) {
                            transitiveBasis.Add(b);
                        }
                        return transitiveBasis;
                    }
                }
            }
        }
        // END TRANSITIVITY OF BETTER
        
        // TRANSITIVITY OF AS_GOOD_AS
        IPattern asGoodAsPattern = new ExpressionPattern(Expression.AS_GOOD_AS, xt0, xt1);
        preferands = asGoodAsPattern.GetBindings(expr);

        if (preferands != null) {
            Expression first = preferands[0][xt0];
            Expression second = preferands[0][xt1];

            foreach (Expression p in preferables) {
                HashSet<Expression> firstBasis = 
                    GetBasis(plan, new Phrase(Expression.AS_GOOD_AS, first, p), suppositions);

                if (firstBasis != null) {
                    HashSet<Expression> secondBasis =
                        GetBasis(plan, new Phrase(Expression.AS_GOOD_AS, p, second), suppositions);

                    if (secondBasis != null) {
                        HashSet<Expression> transitiveBasis = new HashSet<Expression>();
                        foreach (Expression b in firstBasis) {
                            transitiveBasis.Add(b);
                        }
                        foreach (Expression b in secondBasis) {
                            transitiveBasis.Add(b);
                        }
                        return transitiveBasis;
                    }
                }
            }
        }
        // END TRANSITIVITY OF AS_GOOD_AS

        // BOUND
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
                HashSet<Expression> promiseBasis = GetBasis(plan, new Phrase(Expression.NOT, promise));
                if (promiseBasis != null) {
                    basis.Add(new Phrase(Expression.BOUND, promiser[0][xi0], promise));
                    basis.Add(new Phrase(Expression.NOT, promise));
                    triedExpressions[expr] = basis;
                    return basis;
                }
            }
        }

        // END BOUND

        // CONDITIIONAL PROOF
        // MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);

        IPattern conditionalPattern = new ExpressionPattern(Expression.IF, xt0, xt1);
        List<Dictionary<MetaVariable, Expression>> conditionalBindings = conditionalPattern.GetBindings(expr);

        if (conditionalBindings != null) {
            return GetBasis(plan, conditionalBindings[0][xt1], ImAdd(conditionalBindings[0][xt0], suppositions));
        }

        // END CONDITIONAL PROOF
        
        // BELIEF
        // if the model proves X, it also proves believess(self, X)
        IPattern selfBeliefPattern = new ExpressionPattern(Expression.BELIEVE, Expression.SELF, xt0);
        List<Dictionary<MetaVariable, Expression>> selfBeliefBindings = selfBeliefPattern.GetBindings(expr);

        if (selfBeliefBindings != null) {
            HashSet<Expression> contentBasis = GetBasis(false, selfBeliefBindings[0][xt0], suppositions);
            triedExpressions[expr] = contentBasis;
            return contentBasis;
        }
        // END BELIEF

        // LACK OF BELIEF
        // if the model fails to prove X, it proves ~believes(self, X)
        // NOTE this isn't the same as believes(self, ~X)
        IPattern notSelfBeliefPattern = new ExpressionPattern(Expression.NOT, selfBeliefPattern);
        List<Dictionary<MetaVariable, Expression>> notSelfBeliefBindings = notSelfBeliefPattern.GetBindings(expr);

        if (notSelfBeliefBindings != null) {
            if (GetBasis(false, notSelfBeliefBindings[0][xt0], suppositions) == null) {
                basis.Add(expr);
                triedExpressions[expr] = basis;
                return basis;
            }
        }
        // END LACK OF BELIEF

        // RECURSIVE CASES
        foreach (SubstitutionRule sr in this.substitutionRules) {
            List<SubstitutionRule.Result> admissibleSubstitutions = sr.Substitute(this, plan, suppositions, expr);
            
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
                        HashSet<Expression> subBasis = this.GetBasis(plan, e, suppositions);
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
                        HashSet<Expression> subBasis = this.GetBasis(plan, new Phrase(Expression.NOT, e), suppositions);
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
                    List<Dictionary<MetaVariable, Expression>> bindings = Find(plan, suppositions, toFindArray);
                    Dictionary<MetaVariable, Expression> provedBinding = null;
                    if (bindings != null) {
                        foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
                            foreach (IPattern p in toFindList) {
                                Expression fullyBound = p.Bind(binding).ToExpression();
                                if (fullyBound == null) {
                                    proved = false;
                                    break;
                                }
                                HashSet<Expression> subBasis = GetBasis(plan, fullyBound, suppositions);
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
        // ACTIONS
        if (plan) {
            // Check to see if this expression is actionable.
            HashSet<Expression> abilityBasis = GetBasis(plan, new Phrase(Expression.ABLE, Expression.SELF, expr), suppositions);
            if (abilityBasis != null) {
                abilityBasis.Add(new Phrase(Expression.WOULD, expr));
                return abilityBasis;
            }

            // check if anyone else is able to make the expr true.
            // SPECIAL PURPOSE: this can be handled generally but
            // will involve a more efficient algorithm.
            IPattern otherAbilityPattern = new ExpressionPattern(Expression.ABLE, xi0, expr);
            foreach (Expression e in GetAll()) {
                List<Dictionary<MetaVariable, Expression>> otherAbilityBinding = otherAbilityPattern.GetBindings(e);
                if (otherAbilityBinding == null) {
                    continue;
                }

                // we've found another person who can do what we want
                Expression other = otherAbilityBinding[0][xi0];

                // if they don't mind helping, just request it
                if (GetBasis(false,
                    new Phrase(Expression.NOT, new Phrase(Expression.PREFER, other, Expression.NEUTRAL, expr)), suppositions) != null) {
                    HashSet<Expression> expressBasis = GetBasis(true,
                            new Phrase(Expression.EXPRESS_CONFORMITY,
                                Expression.SELF, other, new Phrase(Expression.WOULD, expr)),
                        suppositions);
                    triedExpressions[expr] = expressBasis;
                    return expressBasis;
                }

                // otherwise, we'll need to make an offer.
                // TODO: jot down algorithm here.
                foreach (Expression e2 in GetAll()) {
                    // TODO
                }

                
            }
        }
        // END  ACTIONS
        return null;
    }

    public List<Dictionary<MetaVariable, Expression>> Find(bool plan, List<Expression> suppositions, params IPattern[] patterns) {
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
                    if (e != null && (this.GetBasis(plan, e, suppositions) != null)) {
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
