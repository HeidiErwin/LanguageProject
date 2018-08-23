using System;
using System.Collections.Generic;

public class ExpressionPattern : IPattern {
    protected String headString;
    protected IPattern[] argPattern;
    protected int numArgs;
    protected HashSet<MetaVariable> freeMetaVariables;

    public ExpressionPattern(String headString, IPattern[] argPattern) {
        this.headString = headString;
        this.argPattern = argPattern;
        this.numArgs = argPattern.Length;

        this.freeMetaVariables = new HashSet<MetaVariable>();

        for (int i = 0; i < numArgs; i++) {
            foreach (MetaVariable x in this.argPattern[i].GetFreeMetaVariables()) {
                this.freeMetaVariables.Add(x);
            }
        }
    }

    private bool MatchesHelper(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        if (!this.headString.Equals(expr.headString)) {
            return false;
        }

        return false;

        // ExpressionPattern currentPattern = this;

        // for (int i = 0; i < numArgs; i++) {
        //     if (currentPattern.argPattern[i].Matches(expr.GetArg(i))) {
        //         if (currentPattern.argPattern[i].GetType() == typeof(MetaVariable)) {
        //             MetaVariable x = (MetaVariable) currentPattern.argPattern[i];
        //             bindings.Add(x);
        //         }
        //     }
        // }
    }

    public bool Matches(Expression expr) {
        return MatchesHelper(expr, new Dictionary<MetaVariable, Expression>());
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return null;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (!freeMetaVariables.Contains(x)) {
            return this;
        }

        IPattern[] newArgPattern = new IPattern[numArgs];

        for (int i = 0; i < numArgs; i++) {
            newArgPattern[i] = argPattern[i].Bind(x, expr);
        }
        return new ExpressionPattern(headString, newArgPattern);
    }
}
