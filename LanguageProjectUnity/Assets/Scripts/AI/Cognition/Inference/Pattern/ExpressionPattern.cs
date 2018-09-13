using System;
using System.Text;
using System.Collections.Generic;

// TODO for this class: check the semantic types
// to see if they match

public class ExpressionPattern : IPattern {
    protected IPattern headPattern;
    protected IPattern[] argPattern;
    protected int numArgs;
    protected HashSet<MetaVariable> freeMetaVariables;

    public ExpressionPattern(IPattern headPattern, params IPattern[] argPattern) {
        this.headPattern = headPattern;
        this.argPattern = argPattern;
        this.numArgs = argPattern.Length;

        this.freeMetaVariables = new HashSet<MetaVariable>();

        for (int i = 0; i < numArgs; i++) {
            foreach (MetaVariable x in this.argPattern[i].GetFreeMetaVariables()) {
                this.freeMetaVariables.Add(x);
            }
        }
    }

    public bool Matches(Expression expr) {
        return Matches(expr, new Dictionary<MetaVariable, Expression>());
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        Expression headWord = new Word(expr.headType, expr.headString);

        if (!headPattern.Matches(headWord, bindings)) {
            return false;
        }

        for (int i = 0; i < numArgs; i++) {
            if (!this.argPattern[i].Matches(expr.GetArg(i), bindings)) {
                return false;
            }
        }

        return true;
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return freeMetaVariables;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        // if (!freeMetaVariables.Contains(x)) {
        //     UnityEngine.Debug.Log(x + "AGAIN");
        //     UnityEngine.Debug.Log(this);
        //     return this;
        // }
        // 
        // TODO: add that code in, maybe figuring out the bug.

        IPattern[] newArgPattern = new IPattern[numArgs];

        for (int i = 0; i < numArgs; i++) {
            newArgPattern[i] = argPattern[i].Bind(x, expr);
        }

        return new ExpressionPattern(headPattern.Bind(x, expr), newArgPattern);
    }

    public Expression ToExpression() {
        if (freeMetaVariables.Count != 0) {
            return null;
        }

        Expression headExpression = headPattern.ToExpression();
        Expression[] argExpressions = new Expression[numArgs];
        
        for (int i = 0; i < numArgs; i++) {
            argExpressions[i] = argPattern[i].ToExpression();
        }

        return new Phrase(headExpression, argExpressions);
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        s.Append(headPattern.ToString());
        s.Append("(");
        for (int i = 0; i < numArgs; i++) {
            if (argPattern[i] == null) {
                s.Append("_");
            } else {
                s.Append(argPattern[i].ToString());    
            }
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append(")");

        return s.ToString();
    }
}
