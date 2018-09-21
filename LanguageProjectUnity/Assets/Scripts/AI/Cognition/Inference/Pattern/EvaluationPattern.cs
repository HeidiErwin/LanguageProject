using System;
using System.Collections.Generic;

public enum EntailmentContext {
    Upward,
    Downward,
    None
}

public class EvaluationPattern {
    public IPattern pattern { get; protected set; }
    public EntailmentContext context { get; protected set; }

    public EvaluationPattern(IPattern pattern, EntailmentContext context) {
        this.pattern = pattern;
        this.context = context;
    }

    public static EntailmentContext MergeContext(EntailmentContext a, EntailmentContext b) {
        if (a == EntailmentContext.None || b == EntailmentContext.None) {
            return EntailmentContext.None;
        }

        if (a == EntailmentContext.Upward) {
            return b;
        }

        if (a == EntailmentContext.Downward) {
            if (b == EntailmentContext.Upward) {
                return EntailmentContext.Downward;
            } else {
                return EntailmentContext.Upward;
            } 
        }

        return EntailmentContext.None;
    }

    public override String ToString() {
        String contextString = ".";
        if (context == EntailmentContext.Upward) {
            contextString = "+";
        }
        if (context == EntailmentContext.Downward) {
            contextString = "-";
        }
        return pattern.ToString() + contextString;
    }
}
