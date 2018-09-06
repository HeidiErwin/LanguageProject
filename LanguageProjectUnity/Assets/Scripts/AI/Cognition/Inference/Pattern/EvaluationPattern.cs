using System;
using System.Collections.Generic;

public enum EntailmentContext {
    Upward,
    Downward,
    None
}

public class EvaluationPattern : IPattern {
    IPattern pattern;
    EntailmentContext context;

    public EvaluationPattern(IPattern pattern, EntailmentContext context) {
        this.pattern = pattern;
        this.context = context;
    }

    public EvaluationPattern UpdateContext(EntailmentContext context) {
        
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
