using System.Collections.Generic;

public class EvaluationRule {
    protected IPattern top;
    protected EvaluationPattern[] evaluations;
    public IPattern result { get; protected set; }

    public EvaluationRule(IPattern top, EvaluationPattern[] evaluations, IPattern result) {
        this.top = top;
        this.evaluations = evaluations;
        this.result = result;
    }

    public bool Matches(Expression e, Dictionary<MetaVariable, Expression> bindings) {
        return this.top.Matches(e, bindings);
    }

    public int Length() {
        return this.evaluations.Length;
    }

    public EvaluationPattern Get(int index) {
        return this.evaluations[index];
    }

    public static readonly EvaluationRule NOT_RULE =
        new EvaluationRule(new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, 0)),
            new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Downward)},
            new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, 0)));

    // public static readonly EvaluationRule 
}
