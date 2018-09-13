using System;
using System.Collections.Generic;

public class SubsententialRule {
    protected IPattern top;
    protected IPattern bottom;
    protected EntailmentContext? exclusiveContext;
    
    public SubsententialRule(IPattern top, IPattern bottom, EntailmentContext? exclusiveContext) {
        this.top = top;
        this.bottom = bottom;
        this.exclusiveContext = exclusiveContext;
    }

    public SubsententialRule(IPattern top, IPattern bottom): this(top, bottom, null) {}

    public Expression Infer(Expression expr, EntailmentContext context) {
        if (this.exclusiveContext != null && context != this.exclusiveContext) {
            return null;
        }

        if (context == EntailmentContext.None) {
            return null;
        }

        if (context == EntailmentContext.Upward) {
            return InferUpward(expr);
        }

        if (context == EntailmentContext.Downward) {
            return InferDownward(expr);
        }

        return null;
    }

    private Expression InferUpward(Expression expr) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        IPattern currentPattern = bottom;

        if (top.Matches(expr, bindings)) {
            foreach (MetaVariable x in bindings.Keys) {
                currentPattern = currentPattern.Bind(x, bindings[x]);
            }
        } else {
            return null;
        }

        return currentPattern.ToExpression();
    }

    private Expression InferDownward(Expression expr) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        IPattern currentPattern = top;

        if (bottom.Matches(expr, bindings)) {
            foreach (MetaVariable x in bindings.Keys) {
                currentPattern = currentPattern.Bind(x, bindings[x]);
            }
        } else {
            return null;
        }

        return currentPattern.ToExpression();
    }

    public override String ToString() {
        return top.ToString() + " |- " + bottom.ToString();
    }
}
