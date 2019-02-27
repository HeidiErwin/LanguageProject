using System;
using System.Text;
using System.Collections.Generic;

public class SubstitutionRule {
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

    public List<List<IPattern>[]> Substitute(Model m, Expression expr) {
        List<IPattern>[] match = this.bottom;
        List<IPattern>[] substitution = this.top;

        List<List<IPattern>[]> admissibleSubstitutions = new List<List<IPattern>[]>();

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

                    List<IPattern>[] conjunctPatterns = new List<IPattern>[2];
                    conjunctPatterns[0] = new List<IPattern>();
                    conjunctPatterns[1] = new List<IPattern>();

                    // add all the conjuncts on the top side.
                    for (int j = 0; j <= substitution.Length; j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctPatterns[0].Add(conjunctPattern);
                        }
                    }

                    // add all the negated disjuncts ranked >= this match.
                    for (int j = 0; j <= i; j++) {
                        foreach (IPattern conjunctPattern in match[j]) {
                            // careful with this equality operator
                            if (conjunctPattern == matchPattern) {
                                continue;
                            }
                            conjunctPatterns[1].Add(conjunctPattern);
                        }
                    }

                    if (domain.Count == 0) {
                        admissibleSubstitutions.Add(conjunctPatterns);
                    }

                    foreach (Dictionary<MetaVariable, Expression> binding in domain) {
                        List<IPattern>[] conditionBoundConjunctPatterns = new List<IPattern>[2];
                        conditionBoundConjunctPatterns[0] = new List<IPattern>();
                        conditionBoundConjunctPatterns[1] = new List<IPattern>();
                        
                        foreach (IPattern pattern in conjunctPatterns[0]) {
                            conditionBoundConjunctPatterns[0].Add(pattern.Bind(binding));
                        }

                        foreach (IPattern pattern in conjunctPatterns[1]) {
                            conditionBoundConjunctPatterns[0].Add(pattern.Bind(binding));
                        }

                        admissibleSubstitutions.Add(conditionBoundConjunctPatterns);
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

                    List<IPattern>[] conjunctPatterns = new List<IPattern>[2];
                    conjunctPatterns[0] = new List<IPattern>();
                    conjunctPatterns[1] = new List<IPattern>();

                    for (int j = 0; j < substitution.Length; j++) {
                        foreach (IPattern conjunctPattern in substitution[j]) {
                            conjunctPatterns[0].Add(conjunctPattern.Bind(binding));
                        }
                    }

                    for (int j = 0; j <= i; j++) {
                        foreach (IPattern conjunctPattern in match[j]) {
                            if (conjunctPattern == matchPattern) {
                                continue;
                            }
                            conjunctPatterns[1].Add(conjunctPattern.Bind(binding));
                        }
                    }

                    if (domain.Count == 0) {
                        admissibleSubstitutions.Add(conjunctPatterns);
                    }

                    foreach (Dictionary<MetaVariable, Expression> assignment in domain) {
                        List<IPattern>[] conditionBoundConjunctPatterns = new List<IPattern>[2];
                        conditionBoundConjunctPatterns[0] = new List<IPattern>();
                        conditionBoundConjunctPatterns[1] = new List<IPattern>();
                        
                        foreach (IPattern pattern in conjunctPatterns[0]) {
                            conditionBoundConjunctPatterns[0].Add(pattern.Bind(assignment));
                        }

                        foreach (IPattern pattern in conjunctPatterns[1]) {
                            conditionBoundConjunctPatterns[1].Add(pattern.Bind(assignment));
                        }

                        admissibleSubstitutions.Add(conditionBoundConjunctPatterns);
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
            newTop[i] = new List<IPattern>();
            foreach (IPattern pattern in bottom[i]) {
                newTop[i].Add(new ExpressionPattern(Expression.NOT, pattern));
            }
        }

        for (int i = 0; i < top.Length; i++) {
            newBottom[i] = new List<IPattern>();
            foreach (IPattern pattern in top[i]) {
                newBottom[i].Add(new ExpressionPattern(Expression.NOT, pattern));
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
