using System;
using System.Collections.Generic;

public enum EntailmentContext {
    Upward,
    Downward,
    None
}

public class EvaluationPattern : IPattern {
    IPattern pattern;
    public EntailmentContext context { get; protected set; }

    public EvaluationPattern(IPattern pattern, EntailmentContext context) {
        this.pattern = pattern;
        this.context = context;
    }

    public EvaluationPattern UpdateContext(EntailmentContext context) {
        if (this.context == EntailmentContext.None) {
            return this;
        }

        if (context == EntailmentContext.None) {
            return new EvaluationPattern(pattern, EntailmentContext.None);
        }

        if (this.context == EntailmentContext.Upward) {
            return new EvaluationPattern(pattern, context);
        }

        if (this.context == EntailmentContext.Downward) {
            if (context == EntailmentContext.Upward) {
                return new EvaluationPattern(pattern, EntailmentContext.Downward);
            } else {
                return new EvaluationPattern(pattern, EntailmentContext.Upward);
            } 
        }

        return this; // shouldn't reach this branch
    }

    public bool Matches(Expression expr) {
        return pattern.Matches(expr);
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        return pattern.Matches(expr, bindings);
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return pattern.GetFreeMetaVariables();
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        return new EvaluationPattern(pattern.Bind(x, expr), context);
    }

    public Expression ToExpression() {
        return pattern.ToExpression();
    }
}
