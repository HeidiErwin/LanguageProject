using System;
using System.Collections.Generic;

public class SubstitutionRule {
    protected IPattern[] conditions;
    protected IPattern top;
    protected IPattern bottom;
    protected EntailmentContext? exclusiveContext;
    
    public SubstitutionRule(IPattern[] conditions, IPattern top, IPattern bottom, EntailmentContext? exclusiveContext) {
        this.conditions = conditions;
        this.top = top;
        this.bottom = bottom;
        this.exclusiveContext = exclusiveContext;
    }

    public SubstitutionRule(IPattern[] conditions, IPattern top, IPattern bottom): this(conditions, top, bottom, null) {}

    public SubstitutionRule(IPattern top, IPattern bottom, EntailmentContext? exclusiveContext):
        this(new IPattern[0], top, bottom, exclusiveContext) {}

    public SubstitutionRule(IPattern top, IPattern bottom): this(new IPattern[0], top, bottom, null) {}

    public Expression Substitute(Expression expr, EntailmentContext context) {
        if (this.exclusiveContext != null && context != this.exclusiveContext) {
            return null;
        }

        if (context == EntailmentContext.None) {
            return null;
        }

        IPattern match = null;
        IPattern substitution = null;

        if (context == EntailmentContext.Upward) {
            match = this.top;
            substitution = this.bottom;
        }

        if (context == EntailmentContext.Downward) {
            match = this.bottom;
            substitution = this.top;
        }

        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        
        if (match.Matches(expr, bindings)) {
            IPattern currentPattern = substitution;
            foreach (MetaVariable x in bindings.Keys) {
                currentPattern = currentPattern.Bind(x, bindings[x]);
            }
            return currentPattern.ToExpression();
        }

        return null;
    }

    public override String ToString() {
        return top.ToString() + " |- " + bottom.ToString();
    }
}
