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

        if (context == EntailmentContext.Upward) {
            return SubstituteUpward(expr);
        }

        if (context == EntailmentContext.Downward) {
            return SubstituteDownward(expr);
        }

        return null;
    }

    private Expression SubstituteUpward(Expression expr) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        
        if (top.Matches(expr, bindings)) {
            IPattern currentPattern = bottom;
            foreach (MetaVariable x in bindings.Keys) {
                currentPattern = currentPattern.Bind(x, bindings[x]);
            }
            return currentPattern.ToExpression();
        }

        return null;
    }

    private Expression SubstituteDownward(Expression expr) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        
        if (bottom.Matches(expr, bindings)) {
            IPattern currentPattern = top;
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
