using System.Collections.Generic;

public class EvaluationRule {
    protected IPattern top;
    protected EvaluationPattern bottom;

    public EvaluationRule(IPattern top, EvaluationPattern bottom) {
        this.top = top;
        this.bottom = bottom;
    }

    public EvaluationPattern Evaluate(Expression expr, EntailmentContext context) {
        Dictionary<MetaVariable, Expression> bindings = new Dictionary<MetaVariable, Expression>();
        
        if (!top.Matches(expr, bindings)) {
            return null;
        }

        EvaluationPattern currentEvaluationPattern = bottom;

        foreach (MetaVariable x in bindings.Keys) {
            currentEvaluationPattern = (EvaluationPattern) currentEvaluationPattern.Bind(x, bindings[x]);
        }

        Expression boundEvaluation = currentEvaluationPattern.ToExpression();

        if (boundEvaluation == null) {
            return null;
        }

        return (EvaluationPattern) new EvaluationPattern(boundEvaluation,
                                     currentEvaluationPattern.context).UpdateContext(context);
    }
}
