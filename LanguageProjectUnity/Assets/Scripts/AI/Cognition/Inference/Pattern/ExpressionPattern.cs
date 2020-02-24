using System;
using System.Text;
using System.Collections.Generic;

// TODO for this class: check the semantic types
// to see if they match

// First order bindings -> variables can only bind to expressions
using FOBindings = System.Collections.Generic.Dictionary<MetaVariable, Expression>;
// Higher-order bindings -> variables can bind to other variables
using HOBindings = System.Collections.Generic.Dictionary<MetaVariable, IPattern>;

public class ExpressionPattern : IPattern {
    protected IPattern headPattern;
    protected IPattern[] argPatterns;
    protected HashSet<MetaVariable> freeMetaVariables;
    protected SemanticType type;

    public ExpressionPattern(IPattern headPattern, params IPattern[] argPatterns) {
        if (headPattern.GetSemanticType().IsAtomic()) {
            throw new ArgumentException("Head pattern of expression pattern must not be of an atomic semantic type.");
        }

        for (int i = 0; i < argPatterns.Length; i++) {
            if (!argPatterns[i].GetSemanticType().Equals(headPattern.GetSemanticType().GetInputType(i))) {
                throw new ArgumentException("Semantic type mismatch in expression pattern.");
            }
        }

        this.type = headPattern.GetSemanticType().GetOutputType();
        this.headPattern = headPattern;
        this.argPatterns = argPatterns;

        this.freeMetaVariables = new HashSet<MetaVariable>();

        foreach (MetaVariable x in headPattern.GetFreeMetaVariables()) {
            this.freeMetaVariables.Add(x);    
        }

        for (int i = 0; i < argPatterns.Length; i++) {
            foreach (MetaVariable x in this.argPatterns[i].GetFreeMetaVariables()) {
                this.freeMetaVariables.Add(x);
            }
        }
    }

    public SemanticType GetSemanticType() {
        return type;
    }

    public void AddToDomain(Model m) {
        Expression headExpression = headPattern.ToExpression();
        if (headExpression != null) {
            headExpression.AddToDomain(m);
        }

        for (int i = 0; i < argPatterns.Length; i++) {
            IPattern argPattern = argPatterns[i];
            if (argPattern != null) {
                argPattern.AddToDomain(m);
            }
        }
    }

    // patternIndex is the current index of the pattern's arguments
    // expressionIndex is the current index of the expression's arguments
    // partialArrangment is the current argument setup
    // e is the partially decomposed head
    public Dictionary<Expression, Expression[]> GenerateArgumentArrangements(Expression head, int patternIndex, int expressionIndex, Expression[] partialArrangement) {
        Dictionary<Expression, Expression[]> argumentArrangements = new Dictionary<Expression, Expression[]>();
    
        // if there aren't enough arguments left to match the pattern,
        // then this arrangement is no good.
        if (head.GetNumArgs() - expressionIndex < argPatterns.Length - patternIndex) {
            return argumentArrangements;
        }

        // if there are no more argument patterns to fill,
        // then this arrangement is good to go.
        if (patternIndex == argPatterns.Length) {
            argumentArrangements.Add(head, partialArrangement);
            return argumentArrangements;
        }

        for (int i = expressionIndex; i < head.GetNumArgs(); i++) {
            if (this.argPatterns[patternIndex].GetSemanticType().Equals(head.GetArg(i).type)) {
                // this means that this argument can potential match the pattern of the corresponding argument.
                Expression[] partialArrangementCopy = new Expression[partialArrangement.Length];
                
                for (int j = 0; j < patternIndex; j++) {
                    partialArrangementCopy[j] = partialArrangement[j];
                }
                partialArrangementCopy[patternIndex] = head.GetArg(i);

                Dictionary<Expression, Expression[]> filledInArrangements = GenerateArgumentArrangements(head.Remove(i), patternIndex + 1, i + 1, partialArrangementCopy);

                foreach (KeyValuePair<Expression, Expression[]> kv in filledInArrangements) {
                    argumentArrangements.Add(kv.Key, kv.Value);
                }
            }
        }

        return argumentArrangements;
    }

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr, List<Dictionary<MetaVariable, Expression>> inputBindings) {
        // 1. check type
        if (!this.type.Equals(expr.type)) {
            return null;
        }

        List<Dictionary<MetaVariable, Expression>> outputBindings = new List<Dictionary<MetaVariable, Expression>>();

        // decompose the expression into forms amenable to matching this expression pattern
        Dictionary<Expression, Expression[]> argumentArrangements = GenerateArgumentArrangements(expr, 0, 0, new Expression[argPatterns.Length]);

        bool oneMatched = false;
        // now that we have each decomposition, we want to go through each of them
        // and try to match it with the pattern.
        // More than one decomposition can match the pattern, so we want to return
        // a list of the possible bindings for each potential match.
        foreach (KeyValuePair<Expression, Expression[]> kv in argumentArrangements) {
            Expression head = kv.Key;
            Expression[] args = kv.Value;

            List<FOBindings> currentBindings = headPattern.GetBindings(head, inputBindings);

            if (currentBindings == null) {
                continue;
            }

            for (int i = 0; i < args.Length; i++) {
                currentBindings = argPatterns[i].GetBindings(args[i], currentBindings);
                if (currentBindings == null) {
                    break;
                }
            }

            if (currentBindings == null) {
                continue;
            }

            foreach (Dictionary<MetaVariable, Expression> binding in currentBindings) {
                outputBindings.Add(binding);
            }

            oneMatched = true;
        }

        return oneMatched ? outputBindings : null;
    }

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr) {
        return GetBindings(expr, new List<Dictionary<MetaVariable, Expression>>());
    }

    public List<HOBindings> Unify(IPattern that) {
        UnityEngine.Debug.Log("Stub: ExpressionPattern.Unify()");
        // NOTE: Doesn't permute arguments yet. Might be too slow to do it for both expressions.
        return null;
    }

    public bool Matches(Expression expr) {
        return GetBindings(expr) != null;
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        return freeMetaVariables;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (!freeMetaVariables.Contains(x)) {
            return this;
        }

        IPattern[] newArgPatterns = new IPattern[argPatterns.Length];

        for (int i = 0; i < argPatterns.Length; i++) {
            if (argPatterns[i] != null) {
                newArgPatterns[i] = argPatterns[i].Bind(x, expr);    
            }
        }

        return new ExpressionPattern(headPattern.Bind(x, expr), newArgPatterns);
    }

    public IPattern Bind(Dictionary<MetaVariable, Expression> binding) {
        IPattern currentPattern = this;

        foreach (KeyValuePair<MetaVariable, Expression> kv in binding) {
            currentPattern = currentPattern.Bind(kv.Key, kv.Value);
        }

        return currentPattern;
    }

    public List<IPattern> Bind(List<Dictionary<MetaVariable, Expression>> bindings) {
        List<IPattern> output = new List<IPattern>();
        
        foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
            output.Add(Bind(binding));
        }

        return output;
    }

    public Expression ToExpression() {
        if (freeMetaVariables.Count != 0) {
            return null;
        }

        Expression headExpression = headPattern.ToExpression();
        
        if (headExpression == null) {
            return null;
        }

        Expression[] argExpressions = new Expression[argPatterns.Length];
        
        for (int i = 0; i < argPatterns.Length; i++) {
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
        for (int i = 0; i < argPatterns.Length; i++) {
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
