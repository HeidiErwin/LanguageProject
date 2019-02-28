using System;
using System.Text;
using System.Collections.Generic;

// TODO 2/27: make custom return type
// for Substitute() which includes information
// about default assumptions being made

public class SubstitutionRule {
    public class Result {
        public List<IPattern> positives { private set; get; }
        public List<IPattern> negatives { private set; get; }
        public List<IPattern> assumptions { private set; get; }
        public Result(List<IPattern> positives, List<IPattern> negatives, List<IPattern> assumptions) {
            this.positives = positives;
            this.negatives = negatives;
            this.assumptions = assumptions;
        }
    }

    protected IPattern[] conditions;
    protected List<IPattern>[] top;
    protected List<IPattern>[] bottom;
    public bool isTransposable {get; protected set;}
    
    public SubstitutionRule(IPattern[] conditions, List<IPattern>[] top, List<IPattern>[] bottom, bool isTransposable) {
        this.conditions = conditions;
        this.top = top;
        this.bottom = bottom;
        this.isTransposable = isTransposable;
    }

    public SubstitutionRule(IPattern[] conditions, List<IPattern>[] top, List<IPattern>[] bottom):
        this(conditions, top, bottom, true) {}

    public SubstitutionRule(List<IPattern>[] top, List<IPattern>[] bottom, bool isTransposable):
        this(new IPattern[0], top, bottom, isTransposable) {}

    public SubstitutionRule(List<IPattern>[] top, List<IPattern>[] bottom): this(new IPattern[0], top, bottom) {}

    public void AddToDomain(Model m) {
        for (int i = 0; i < conditions.Length; i++) {
            conditions[i].AddToDomain(m);
        }

        for (int i = 0; i < top.Length; i++) {
            foreach (IPattern p in top[i]) {
                p.AddToDomain(m);
            }
        }
        
        for (int i = 0; i < bottom.Length; i++) {
            foreach (IPattern p in bottom[i]) {
                p.AddToDomain(m);
            }
        }
    }

    public bool IsLastRank(Expression expr) {
        List<IPattern>[] match = this.bottom;

        for (int i = 0; i < match.Length - 1; i++) {
            foreach (IPattern matchPattern in match[i]) {
                if (matchPattern.Matches(expr)) {
                    return false;
                }
            }
        }
        return true;
    }

    public List<Result> Substitute(Model m, Expression expr) {
        List<IPattern>[] match = this.bottom;
        List<IPattern>[] substitution = this.top;

        List<Result> admissibleSubstitutions = new List<Result>();

        // go through the match row, and act on the patterns
        // that match expr.
        for (int i = 0; i < match.Length; i++) {
            foreach (IPattern matchPattern in match[i]) {
                List<Dictionary<MetaVariable, Expression>> bindings = matchPattern.GetBindings(expr);
                if (bindings == null) {
                    continue;
                }

                if (bindings.Count == 0) {
                    // edge case: successful match but no bindings
                    
                    List<Dictionary<MetaVariable, Expression>> domain;
                    if (conditions == null || conditions.Length == 0) {
                        domain = new List<Dictionary<MetaVariable, Expression>>();
                    } else {
                        domain = m.Find(conditions);
                    }

                    if (domain == null) {
                        // this means nothing satisfied the specified conditions.
                        continue;
                    }

                    // add all the conjuncts on the top side.
                    List<IPattern> conjunctPositives = new List<IPattern>();
                    for (int j = 0; (j <= i) && (j < substitution.Length); j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctPositives.Add(conjunctPattern);
                        }
                    }

                    // add all the higher-rank conjuncts as assumptions.
                    List<IPattern> conjunctAssumptions = new List<IPattern>();
                    for (int j = i + 1; j < substitution.Length; j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctAssumptions.Add(conjunctPattern);
                        }
                    }

                    // add all the negated disjuncts ranked >= this match.
                    List<IPattern> conjunctNegatives = new List<IPattern>();
                    for (int j = 0; j <= i; j++) {
                        foreach (IPattern conjunctPattern in match[j]) {
                            // careful with this equality operator
                            if (conjunctPattern == matchPattern) {
                                // if this is a default choice
                                // of disjunct, we want to include
                                // it in the assumptions made.
                                if (j < match.Length - 1) {
                                    conjunctAssumptions.Add(conjunctPattern);
                                }
                                continue;
                            }
                            conjunctNegatives.Add(conjunctPattern);
                        }
                    }

                    if (domain.Count == 0) {
                        admissibleSubstitutions.Add(new Result(conjunctPositives, conjunctNegatives, conjunctAssumptions));
                    }

                    foreach (Dictionary<MetaVariable, Expression> binding in domain) {
                        List<IPattern> conditionBoundConjunctPositives = new List<IPattern>();
                        List<IPattern> conditionBoundConjunctNegatives = new List<IPattern>();
                        List<IPattern> conditionBoundConjunctAssumptions = new List<IPattern>();
                        
                        foreach (IPattern pattern in conjunctPositives) {
                            conditionBoundConjunctPositives.Add(pattern.Bind(binding));
                        }

                        foreach (IPattern pattern in conjunctNegatives) {
                            conditionBoundConjunctNegatives.Add(pattern.Bind(binding));
                        }

                        foreach (IPattern pattern in conjunctAssumptions) {
                            conditionBoundConjunctAssumptions.Add(pattern.Bind(binding));
                        }

                        admissibleSubstitutions.Add(new Result(
                            conditionBoundConjunctPositives,
                            conditionBoundConjunctNegatives,
                            conditionBoundConjunctAssumptions));
                    }
                }

                // CAREFUL: this code isn't (entirely) redundant
                foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
                    // each of these bindings is a separate match.
                    IPattern[] boundConditions = new IPattern[conditions.Length];
                    
                    for (int j = 0; j < conditions.Length; j++) {
                        boundConditions[j] = conditions[j].Bind(binding);
                    }

                    List<Dictionary<MetaVariable, Expression>> domain;
                    if (conditions == null || conditions.Length == 0) {
                        domain = new List<Dictionary<MetaVariable, Expression>>();
                    } else {
                        domain = m.Find(boundConditions);
                    }

                    if (domain == null) {
                        // this means nothing satisfied the specified conditions.
                        continue;
                    }

                    
                    
                    List<IPattern> conjunctAssumptions = new List<IPattern>();

                    List<IPattern> conjunctPositives = new List<IPattern>();
                    for (int j = 0; (j <= i) && (j < substitution.Length); j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctPositives.Add(conjunctPattern.Bind(binding));
                        }
                    }

                    for (int j = i + 1; j < substitution.Length; j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctAssumptions.Add(conjunctPattern.Bind(binding));
                        }
                    }

                    List<IPattern> conjunctNegatives = new List<IPattern>();
                    for (int j = 0; j <= i; j++) {
                        foreach (IPattern conjunctPattern in match[j]) {
                            if (conjunctPattern == matchPattern) {
                                if (j < match.Length - 1) {
                                    conjunctAssumptions.Add(conjunctPattern.Bind(binding));
                                }
                                continue;
                            }
                            conjunctNegatives.Add(conjunctPattern.Bind(binding));
                        }
                    }

                    if (domain.Count == 0) {
                        admissibleSubstitutions.Add(new Result(conjunctPositives, conjunctNegatives, conjunctAssumptions));
                    }

                    foreach (Dictionary<MetaVariable, Expression> assignment in domain) {
                        List<IPattern> conditionBoundConjunctPositives = new List<IPattern>();
                        List<IPattern> conditionBoundConjunctNegatives = new List<IPattern>();
                        List<IPattern> conditionBoundConjunctAssumptions = new List<IPattern>();
                        
                        foreach (IPattern pattern in conjunctPositives) {
                            conditionBoundConjunctPositives.Add(pattern.Bind(assignment));
                        }

                        foreach (IPattern pattern in conjunctNegatives) {
                            conditionBoundConjunctNegatives.Add(pattern.Bind(assignment));
                        }

                        foreach (IPattern pattern in conjunctAssumptions) {
                            conditionBoundConjunctAssumptions.Add(pattern.Bind(assignment));
                        }

                        admissibleSubstitutions.Add(new Result(
                            conditionBoundConjunctPositives,
                            conditionBoundConjunctNegatives,
                            conditionBoundConjunctAssumptions));
                    }
                }
            }
        }

        if (admissibleSubstitutions.Count == 0) {
            return null;
        }
        
        return admissibleSubstitutions;
    }

    public SubstitutionRule Contrapositive() {
        List<IPattern>[] newTop = new List<IPattern>[this.bottom.Length];
        List<IPattern>[] newBottom = new List<IPattern>[this.top.Length];

        for (int i = 0; i < bottom.Length; i++) {
            int newRank = bottom.Length - (i + 1);
            newTop[newRank] = new List<IPattern>();
            foreach (IPattern pattern in bottom[i]) {
                newTop[newRank].Add(new ExpressionPattern(Expression.NOT, pattern));
            }
        }

        for (int i = 0; i < top.Length; i++) {
            int newRank = top.Length - (i + 1);
            newBottom[newRank] = new List<IPattern>();
            foreach (IPattern pattern in top[i]) {
                newBottom[newRank].Add(new ExpressionPattern(Expression.NOT, pattern));
            }
        }

        return new SubstitutionRule(conditions, newTop, newBottom);
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();

        if (conditions != null) {
            s.Append(" [");

            for (int i = 0; i < conditions.Length; i++) {
                s.Append(conditions[i] + ", ");
            }
            s.Append("]");
        }

        for (int i = 0; i < top.Length; i++) {
            s.Append(" |");
            foreach (IPattern pattern in top[i]) {
                s.Append(pattern);
                s.Append(", ");
            }
            s.Append("|");
        }

        s.Append(" |- ");

        for (int i = 0; i < bottom.Length; i++) {
            s.Append(" |");
            foreach (IPattern pattern in bottom[i]) {
                s.Append(pattern);
                s.Append(", ");
            }
            s.Append("|");
        }

        return s.ToString();
    }
}
