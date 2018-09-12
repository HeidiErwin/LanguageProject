using System;
using System.Text;
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

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        s.Append(top.ToString());
        s.Append( " |- ");

        for (int i = 0; i < this.evaluations.Length; i++) {
            s.Append(this.evaluations[i].ToString());
            s.Append("; ");
        }
        
        s.Append(result.ToString());

        return s.ToString();
    }

    public static readonly EvaluationRule NOT = new EvaluationRule(
        new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Downward)},
        new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, -1)));

    public static readonly EvaluationRule DEFAULT_TRUTH_FUNCTION_1 = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0), new MetaVariable(SemanticType.TRUTH_VALUE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0), new MetaVariable(SemanticType.TRUTH_VALUE, -1)));

    public static readonly EvaluationRule EVERY = new EvaluationRule(
        new ExpressionPattern(Expression.EVERY, new MetaVariable(SemanticType.PREDICATE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.PREDICATE, 0), EntailmentContext.Downward)},
        new ExpressionPattern(Expression.EVERY, new MetaVariable(SemanticType.PREDICATE, -1)));

    public static readonly EvaluationRule DEFAULT_DETERMINER = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.DETERMINER, 0), new MetaVariable(SemanticType.PREDICATE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.PREDICATE, 0), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.DETERMINER, 0), new MetaVariable(SemanticType.PREDICATE, -1)));

    // public static readonly EvaluationRule NOT = new EvaluationRule(
    //     new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, 0)),
    //     new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Downward)},
    //     new ExpressionPattern(Expression.NOT, new MetaVariable(SemanticType.TRUTH_VALUE, -1)));
}
