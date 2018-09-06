using System.Collections.Generic;

public class EvaluationRule {
    protected IPattern top;
    protected EvaluationPattern bottom;

    public EvaluationRule(IPattern top, EvaluationPattern bottom) {
        this.top = top;
        this.bottom = bottom;
    }
}
