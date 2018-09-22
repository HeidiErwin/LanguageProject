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
    protected SemanticType type;

    public ExpressionPattern(IPattern headPattern, params IPattern[] argPattern) {
        if (headPattern.GetSemanticType().IsAtomic()) {
            throw new ArgumentException("head pattern of expression pattern must not be of an atomic semantic type.");
        }
        for (int i = 0; i < argPattern.Length; i++) {
            if (!argPattern[i].GetSemanticType().Equals(headPattern.GetSemanticType().GetInputType(i))) {
                throw new ArgumentException("Semantic type mismatch in expression pattern.");
            }
        }

        this.type = headPattern.GetSemanticType().GetOutputType();
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

    public SemanticType GetSemanticType() {
        return type;
    }

    public bool Matches(Expression expr, List<Dictionary<MetaVariable, Expression>> bindings) {
        if (!this.type.Equals(expr.type)) {
            return false;
        }

        // TODO pattern matching with multiple variable bindings
        // and matching for expressions of higher semantic type
        // e.g. F(x) matching R(a, b)
        return false;
    }

    public bool Matches(Expression expr) {
        return Matches(expr, new Dictionary<MetaVariable, Expression>());
    }

    public bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings) {
        if (!this.type.Equals(expr.type)) {
            return false;
        }
        
        Expression headExpression = new Word(expr.headType, expr.headString);

        // to handle partially applied expressions, do the following:
        // 1. check the semantic type of the headPattern
        // 2. need to ALL permutations for this -_-
        // TODO this
        // while (!headPattern.GetSemanticType().Equals(headExpression.type)) {
        //     if (headExpression.type) {

        //     }
        // }



        if (!headPattern.Matches(headExpression, bindings)) {
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
            if (argPattern[i] != null) {
                newArgPattern[i] = argPattern[i].Bind(x, expr);    
            }
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
            if (argPattern[i] != null) {
                argExpressions[i] = argPattern[i].ToExpression();
            }
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
