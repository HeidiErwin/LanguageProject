using System.Collections.Generic;

public class InferenceRule {
    protected IPattern[] top;
    protected IPattern[] bottom;
    protected EntailmentContext? exclusiveContext;

    public InferenceRule(IPattern[] top, IPattern[] bottom, EntailmentContext? exclusiveContext) {
        // TODO: check that all patterns have type t.
        this.top = top;
        this.bottom = bottom;
        this.exclusiveContext = exclusiveContext;
    }

    // TODO rename this
    public bool CanInfer(Model m, Expression expr, EntailmentContext context) {
        if (this.exclusiveContext != null && context != this.exclusiveContext) {
            return false;
        }

        IPattern[] matchRow = null;
        IPattern[] otherRow = null;

        if (context == EntailmentContext.Upward) {
            matchRow = this.top;
            otherRow = this.bottom;
        } else {
            matchRow = this.bottom;
            otherRow = this.top;
        }

        int matchIndex = -1;

        for (int i = 0; i < matchRow.Length; i++) {
            Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
            if (matchRow[i].Matches(expr, bindings)) {
                for (int j = 0; j < matchRow.Length; j++) {
                    if (j == i) {
                        continue;
                    }

                    IPattern bound = matchRow[j];
                    foreach (MetaVariable x in bindings.Keys) {
                        bound = bound.Bind(x, bindings[x]);
                    }
                    
                    Expression fullyBound = bound.ToExpression();
                    // TODO change this to the quantified variable kind of check. This will need to be for
                    // inferences involving sides with more free variables than the matched sentence
                    if (fullyBound == null || !m.Proves(new Phrase(Expression.NOT, fullyBound.ToExpression()))) {
                        return false;
                    }
                }
                for (int j = 0; j < otherRow.Length; j++) {
                    IPattern bound = otherRow[j];
                    foreach (MetaVariable x in bindings.Keys) {
                        bound = bound.Bind(x, bindings[x]);
                    }

                    Expression fullyBound = bound.ToExpression();
                    // TODO same as line 46
                    if (fullyBound == null || !m.Proves(fullyBound.ToExpression())) {
                        return false;
                    }
                }
                return true;
            }
        }

        return false;
    }
}
