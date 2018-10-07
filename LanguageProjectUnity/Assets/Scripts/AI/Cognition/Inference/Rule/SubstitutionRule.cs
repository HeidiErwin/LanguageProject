using System;
using System.Text;
using System.Collections.Generic;

public class SubstitutionRule {
    protected IPattern[] conditions;
    protected IPattern[] top;
    protected IPattern[] bottom;
    protected EntailmentContext? exclusiveContext;
    
    public SubstitutionRule(IPattern[] conditions, IPattern[] top, IPattern[] bottom, EntailmentContext? exclusiveContext) {
        this.conditions = conditions;
        this.top = top;
        this.bottom = bottom;
        this.exclusiveContext = exclusiveContext;
    }

    public SubstitutionRule(IPattern[] conditions, IPattern[] top, IPattern[] bottom): this(conditions, top, bottom, null) {}

    public SubstitutionRule(IPattern[] top, IPattern[] bottom, EntailmentContext? exclusiveContext):
        this(new IPattern[0], top, bottom, exclusiveContext) {}

    public SubstitutionRule(IPattern[] top, IPattern[] bottom): this(new IPattern[0], top, bottom, null) {}

    public List<List<Expression>> Substitute(Model m, Expression expr, EntailmentContext context) {
        if (this.exclusiveContext != null && context != this.exclusiveContext) {
            return null;
        }

        if (context == EntailmentContext.None) {
            return null;
        }

        IPattern[] match = null;
        IPattern[] substitution = null;

        if (context == EntailmentContext.Upward) {
            match = this.top;
            substitution = this.bottom;
        }

        if (context == EntailmentContext.Downward) {
            match = this.bottom;
            substitution = this.top;
        }

        List<List<Expression>> admissibleSubstitutions = new List<List<Expression>>();

        // go through the match row, and act on the patterns
        // that match expr.
        for (int i = 0; i < match.Length; i++) {
            List<Dictionary<MetaVariable, Expression>> bindings = match[i].GetBindings(expr);
            if (bindings == null) {
                continue;
            }

            if (bindings.Count == 0) {
                // edge case: successful match but no bindings
               // List<Dictionary<MetaVariable, Expression>> domain = m.Find(conditions);  //commented out so game will run

                for (int j = 0; j < match.Length; j++) {
                    if (j == i) {
                        continue;
                    }

                    // TODO
                }
            }

            foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
                // for each binding, check conditions and gather substitutions individually
            }

            List<IPattern>[] conditionPartials = new List<IPattern>[conditions.Length];
            for (int j = 0; j < conditions.Length; j++) {
                conditionPartials[i] = conditions[i].BindAll(bindings);
            }

            for (int j = 0; j < match.Length; j++) {
                if (j == i) {
                    continue;
                }

                List<IPattern> partials = match[j].BindAll(bindings);
                
                foreach (IPattern p in partials) {
                    Expression fullyBound = p.ToExpression();
                    if (fullyBound == null) {
                        // TODO: something with m.Find()
                    } else {

                    }
                }
            }
        }

        return admissibleSubstitutions; // TODO


        // Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        //
        // if (match.Matches(expr, bindings)) {
        //     IPattern currentPattern = substitution;
        //     foreach (MetaVariable x in bindings.Keys) {
        //         currentPattern = currentPattern.Bind(x, bindings[x]);
        //     }
            
        //     Expression ifBound = currentPattern.ToExpression();
        //     if (ifBound != null) {
        //         List<Expression> substitutions = new List<Expression>();
        //         substitutions.Add(ifBound);
        //         return substitutions;
        //     }

        //     IPattern[] conditionPartials = new IPattern[conditions.Length];
        //     for (int i = 0; i < conditionPartials.Length; i++) {
        //         conditionPartials[i] = conditions[i];
        //         foreach (MetaVariable x in bindings.Keys) {
        //             conditionPartials[i] = conditionPartials[i].Bind(x, bindings[x]);
        //         }
        //     }

        //     HashSet<Dictionary<MetaVariable, Expression>> assignments = m.Find(conditionPartials);
        //     if (assignments != null) {
        //         List<Expression> substitutions = new List<Expression>();
        //         foreach (Dictionary<MetaVariable, Expression> assignment in assignments) {
        //             IPattern newCurrentPattern = currentPattern;
        //             foreach (MetaVariable x in assignment.Keys) {
        //                 newCurrentPattern = currentPattern.Bind(x, assignment[x]);
        //             }
        //             Expression fullyAssigned = newCurrentPattern.ToExpression();
        //             if (fullyAssigned == null) {
        //                 return null;
        //             }
        //             substitutions.Add(fullyAssigned);
        //         }
        //         return substitutions;
        //     }
        // }
        return null;
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();

        s.Append(top.ToString());

        if (conditions != null) {
            s.Append(" [");

            for (int i = 0; i < conditions.Length; i++) {
                s.Append(conditions[i] + ", ");
            }
            s.Append("]");
        }

        return top.ToString() + " |- " + bottom.ToString();
    }
}
