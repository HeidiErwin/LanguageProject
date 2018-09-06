using System;
using System.Collections.Generic;
using UnityEngine;

public class ExpressionPattern : IPattern {
    protected IPattern headPattern;
    protected IPattern[] argPattern;
    protected int numArgs;
    protected HashSet<MetaVariable> freeMetaVariables;

    public ExpressionPattern(IPattern headPattern, IPattern[] argPattern) {
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
        Debug.Log(expr);
        Debug.Log(expr.headType);
        Debug.Log(expr.headString);
        Expression headWord = new Word(expr.headType, expr.headString);
        Debug.Log(expr);
        if (!headPattern.Matches(headWord, bindings)) {
            return false;
        }

        if (headPattern.GetType() == typeof(MetaVariable)) {
            bindings.Add((MetaVariable)headPattern, headWord);
        }

        for (int i = 0; i < numArgs; i++) {
            if (!this.argPattern[i].Matches(expr.GetArg(i), bindings)) {
                return false;
            }

            if (this.argPattern[i].GetType() == typeof(MetaVariable)) {
                MetaVariable x = (MetaVariable)this.argPattern[i];
                if (!bindings.ContainsKey(x)) {
                    bindings.Add(x, expr.GetArg(i));
                }
            }
        }

        return true;
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return freeMetaVariables;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (!freeMetaVariables.Contains(x)) {
            return this;
        }

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

        Expression headExpression = (Expression) headPattern;
        Expression[] argExpressions = new Expression[numArgs];
        
        for (int i = 0; i < numArgs; i++) {
            argExpressions[i] = (Expression) argPattern[i];
        }

        return new Phrase(headExpression, argExpressions);
    }
}
