using System.Collections.Generic;

public class SyntheticRule {
    protected IPattern[] top;
    protected IPattern[] bottom;

    public SyntheticRule(IPattern[] top, IPattern[] bottom) {
        this.top = top;
        this.bottom = bottom;
    }
}
