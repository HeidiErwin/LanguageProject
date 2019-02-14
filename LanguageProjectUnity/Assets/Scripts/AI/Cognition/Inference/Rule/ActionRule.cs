using System;
using System.Text;
using System.Collections.Generic;

public class ActionRule {
    public IPattern condition {protected set; get;}
    public IPattern action {protected set; get;}
    public IPattern result {protected set; get;}

    public ActionRule(IPattern condition, IPattern action, IPattern result) {
        this.condition = condition;
        this.action = action;
        this.result = result;
    }

    public override string ToString() {
        return "[" + condition + "] " + action + " |- " + result;
    }
}
