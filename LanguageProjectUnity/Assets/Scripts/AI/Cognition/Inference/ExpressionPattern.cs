public class ExpressionPattern : IPattern {
    protected IPattern headPattern;
    protected IPattern[] argPattern;
    protected int numArgs;

    public ExpressionPattern(IPattern headPattern, IPattern[] argPattern) {
        this.headPattern = headPattern;
        this.argPattern = argPattern;
        this.numArgs = argPattern.Length;
    }

    public int GetLocalID() {
        return -1;
    }

    public bool Matches(Expression expr) {
        IPattern currentPattern = this;

        // if (headPattern.Matches(expr)) {
        //     .Bind(headPattern.GetLocalID(), expr)
        // } else {
        //     return false;
        // }

        return false;
    }

    public IPattern Bind(int id, Expression expr) {
        IPattern[] newArgPattern = new IPattern[numArgs];

        for (int i = 0; i < numArgs; i++) {
            newArgPattern[i] = argPattern[i].Bind(id, expr);
        }
        return new ExpressionPattern(headPattern.Bind(id, expr), newArgPattern);
    }
}
