using System;
using System.Text;
using System.Collections.Generic;

// TODO for this class: check the semantic types
// to see if they match

public class ExpressionPattern : IPattern {
    protected IPattern headPattern;
    protected IPattern[] argPatterns;
    protected int numArgs;
    protected HashSet<MetaVariable> freeMetaVariables;
    protected SemanticType type;

    public ExpressionPattern(IPattern headPattern, params IPattern[] argPatterns) {
        if (headPattern.GetSemanticType().IsAtomic()) {
            throw new ArgumentException("head pattern of expression pattern must not be of an atomic semantic type.");
        }
        for (int i = 0; i < argPatterns.Length; i++) {
            if (!argPatterns[i].GetSemanticType().Equals(headPattern.GetSemanticType().GetInputType(i))) {
                throw new ArgumentException("Semantic type mismatch in expression pattern.");
            }
        }

        this.type = headPattern.GetSemanticType().GetOutputType();
        this.headPattern = headPattern;
        this.argPatterns = argPatterns;
        this.numArgs = argPatterns.Length;

        this.freeMetaVariables = new HashSet<MetaVariable>();

        for (int i = 0; i < numArgs; i++) {
            foreach (MetaVariable x in this.argPatterns[i].GetFreeMetaVariables()) {
                this.freeMetaVariables.Add(x);
            }
        }
    }

    private int choose(int n, int k) {
        int result = 1;
        int limit = k <= n - k ? k : n - k;

        for (int i = 1; i <= k; i++) {
            result *= (n + 1 - i) / i;
        }

        return result;
    }

    public SemanticType GetSemanticType() {
        return type;
    }

    // public bool GenerateArgumentArrangements(int k, List<Expression[]> argumentArrangments) {
    //     List<Expression[]> argumentArrangements = new List<Expression[]>();

    //     for (int i = 0; i < argPatterns.Length; i++) {
    //         for (int j = 0; j < expr.GetNumArgs(); j++) {
    //             if (argPatterns[i].) {

    //             }
    //         }
    //     }
    // }

    public bool Matches(Expression expr, List<Dictionary<MetaVariable, Expression>> bindings) {
        // 1. check type
        if (!this.type.Equals(expr.type)) {
            return false;
        }

        // decompose the expression into forms amenable to matching this expression pattern

        return true; // TODO
    }

    public bool Matches(Expression expr) {
        return Matches(expr, new Dictionary<MetaVariable, Expression>());
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        if (expr == null) {
            return false;
        }

        if (!this.type.Equals(expr.type)) {
            return false;
        }
        
        Expression headExpression = new Word(expr.headType, expr.headString);

        if (!headPattern.Matches(headExpression, bindings)) {
            return false;
        }

        for (int i = 0; i < numArgs; i++) {
            if (!this.argPatterns[i].Matches(expr.GetArg(i), bindings)) {
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

        IPattern[] newArgPatterns = new IPattern[numArgs];

        for (int i = 0; i < numArgs; i++) {
            if (argPatterns[i] != null) {
                newArgPatterns[i] = argPatterns[i].Bind(x, expr);    
            }
        }

        return new ExpressionPattern(headPattern.Bind(x, expr), newArgPatterns);
    }

    public Expression ToExpression() {
        if (freeMetaVariables.Count != 0) {
            return null;
        }

        Expression headExpression = headPattern.ToExpression();
        Expression[] argExpressions = new Expression[numArgs];
        
        for (int i = 0; i < numArgs; i++) {
            if (argPatterns[i] != null) {
                argExpressions[i] = argPatterns[i].ToExpression();
            }
        }

        return new Phrase(headExpression, argExpressions);
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        s.Append(headPattern.ToString());
        s.Append("(");
        for (int i = 0; i < numArgs; i++) {
            if (argPatterns[i] == null) {
                s.Append("_");
            } else {
                s.Append(argPatterns[i].ToString());    
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
