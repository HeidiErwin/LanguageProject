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

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression e) {
        return this.top.GetBindings(e, new List<Dictionary<MetaVariable, Expression>>());
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

    public static readonly EvaluationRule EVERY = new EvaluationRule(
        new ExpressionPattern(Expression.EVERY, new MetaVariable(SemanticType.PREDICATE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.PREDICATE, 0), EntailmentContext.Downward)},
        new ExpressionPattern(Expression.EVERY, new MetaVariable(SemanticType.PREDICATE, -1)));

    public static readonly EvaluationRule DEFAULT_TRUTH_FUNCTION_1 = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0), new MetaVariable(SemanticType.TRUTH_VALUE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0), new MetaVariable(SemanticType.TRUTH_VALUE, -1)));

    public static readonly EvaluationRule DEFAULT_TRUTH_FUNCTION_2 = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_2, 0),
            new MetaVariable(SemanticType.TRUTH_VALUE, 0),
            new MetaVariable(SemanticType.TRUTH_VALUE, 1)),
        new EvaluationPattern[]{
            new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.TRUTH_VALUE, 1), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.TRUTH_FUNCTION_2, 0),
            new MetaVariable(SemanticType.TRUTH_VALUE, -1),
            new MetaVariable(SemanticType.TRUTH_VALUE, -2)));

    public static readonly EvaluationRule DEFAULT_DETERMINER = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.DETERMINER, 0), new MetaVariable(SemanticType.PREDICATE, 0)),
        new EvaluationPattern[]{new EvaluationPattern(new MetaVariable(SemanticType.PREDICATE, 0), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.DETERMINER, 0), new MetaVariable(SemanticType.PREDICATE, -1)));

    public static readonly EvaluationRule DEFAULT_PREDICATE = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.PREDICATE, 0), new MetaVariable(SemanticType.INDIVIDUAL, 0)),
        new EvaluationPattern[]{
            new EvaluationPattern(new MetaVariable(SemanticType.PREDICATE, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 0), EntailmentContext.Upward)},
        new ExpressionPattern(new MetaVariable(SemanticType.PREDICATE, -1), new MetaVariable(SemanticType.INDIVIDUAL, -2)));

    public static readonly EvaluationRule DEFAULT_RELATION_2 = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.RELATION_2, 0),
            new MetaVariable(SemanticType.INDIVIDUAL, 0),
            new MetaVariable(SemanticType.INDIVIDUAL, 1)),
        new EvaluationPattern[]{
            new EvaluationPattern(new MetaVariable(SemanticType.RELATION_2, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 1), EntailmentContext.Upward)},
        new ExpressionPattern(
            new MetaVariable(SemanticType.RELATION_2, -1),
            new MetaVariable(SemanticType.INDIVIDUAL, -2),
            new MetaVariable(SemanticType.INDIVIDUAL, -3)));

    public static readonly EvaluationRule DEFAULT_RELATION_3 = new EvaluationRule(
        new ExpressionPattern(new MetaVariable(SemanticType.RELATION_3, 0),
            new MetaVariable(SemanticType.INDIVIDUAL, 0),
            new MetaVariable(SemanticType.INDIVIDUAL, 1),
            new MetaVariable(SemanticType.INDIVIDUAL, 2)),
        new EvaluationPattern[]{
            new EvaluationPattern(new MetaVariable(SemanticType.RELATION_3, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 0), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 1), EntailmentContext.Upward),
            new EvaluationPattern(new MetaVariable(SemanticType.INDIVIDUAL, 2), EntailmentContext.Upward)},
        new ExpressionPattern(
            new MetaVariable(SemanticType.RELATION_3, -1),
            new MetaVariable(SemanticType.INDIVIDUAL, -2),
            new MetaVariable(SemanticType.INDIVIDUAL, -3),
            new MetaVariable(SemanticType.INDIVIDUAL, -4)));
}
