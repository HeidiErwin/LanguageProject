using System;
using System.Text;
using System.Collections.Generic;

public class ActionRule {
    public Expression condition {protected set; get;}
    public Expression action {protected set; get;}
    public Expression result {protected set; get;}

    public ActionRule(Expression condition, Expression action, Expression result) {
        this.condition = condition;
        this.action = action;
        this.result = result;
    }
}
