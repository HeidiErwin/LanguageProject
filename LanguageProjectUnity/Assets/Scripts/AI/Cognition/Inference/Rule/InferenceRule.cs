using System;
using System.Text;
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

    public InferenceRule(IPattern[] top, IPattern[] bottom): this(top, bottom, null) {}

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
                HashSet<IPattern> patterns = new HashSet<IPattern>();
                for (int j = 0; j < matchRow.Length; j++) {
                    if (j == i) {
                        continue;
                    }

                    IPattern bound = matchRow[j];
                    foreach (MetaVariable x in bindings.Keys) {
                        bound = bound.Bind(x, bindings[x]);
                    }
                    
                    Expression fullyBound = bound.ToExpression();
                    if (fullyBound == null) {
                        patterns.Add(new ExpressionPattern(Expression.NOT, bound));
                    } else if (!m.Proves(new Phrase(Expression.NOT, fullyBound.ToExpression()))) {
                        return false;
                    }
                }
                for (int j = 0; j < otherRow.Length; j++) {
                    IPattern bound = otherRow[j];
                    foreach (MetaVariable x in bindings.Keys) {
                        bound = bound.Bind(x, bindings[x]);
                    }

                    Expression fullyBound = bound.ToExpression();
                    if (fullyBound == null) {
                        patterns.Add(bound);
                    } else if (!m.Proves(fullyBound.ToExpression())) {
                        return false;
                    }
                }
                
                IPattern[] patternsArray = new IPattern[patterns.Count];
                patterns.CopyTo(patternsArray);
                if (patternsArray.Length == 0 || m.Find(patternsArray) != null) {
                    return true;
                }
            }
        }
        return false;
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        for (int i = 0; i < top.Length; i++) {
            s.Append(top[i]);
            s.Append(" ");
        }

        s.Append("|- ");

        for (int i = 0; i < bottom.Length; i++) {
            s.Append(bottom[i]);
            s.Append(" ");
        }

        return s.ToString();
    }
}
