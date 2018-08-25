using System.Collections.Generic;

public enum EntailmentContext {
    Upward,
    Downward,
    None
}

public class Evaluation {
    public IPattern pattern { get; private set; }
    public EntailmentContext entailmentContext { get; private set; }

    private Evaluation(IPattern pattern, EntailmentContext entailmentContext) {
        this.pattern = pattern;
        this.entailmentContext = entailmentContext;
    }
}

public class EvaluationRule {
    IPattern pattern;
    Evaluation evaluation;

    public EvaluationRule(IPattern pattern, Evaluation evaluation) {
        this.pattern = pattern;
        this.evaluation = evaluation;
    }

    public Expression Evaluate(Expression expr) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        if (!pattern.Matches(expr, bindings)) {
            return null;
        }

        IPattern currentPattern = evaluation.pattern;

        foreach (MetaVariable x in bindings.Keys) {
            currentPattern = currentPattern.Bind(x, bindings[x]);
        }

        return null; // TODO
    }
}
