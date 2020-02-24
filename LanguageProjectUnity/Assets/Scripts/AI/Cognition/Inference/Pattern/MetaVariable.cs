using System;
using System.Collections.Generic;

// First order bindings -> variables can only bind to expressions
using FOBindings = System.Collections.Generic.Dictionary<MetaVariable, Expression>;
// Higher-order bindings -> variables can bind to other variables
using HOBindings = System.Collections.Generic.Dictionary<MetaVariable, IPattern>;

public class MetaVariable : IPattern {
    protected SemanticType type;
    protected int localID;

    public MetaVariable(SemanticType type, int localID) {
        this.type = type;
        this.localID = localID;
    }

    public SemanticType GetSemanticType() {
        return type;
    }

    public List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr, List<Dictionary<MetaVariable, Expression>> inputBindings) {
        if (expr == null || !expr.type.Equals(this.type)) {
            return null;
        }

        List<FOBindings> outputBindings = new List<FOBindings>();

        if (inputBindings.Count == 0) {
            FOBindings newBinding = new FOBindings();
            newBinding.Add(this, expr);
            outputBindings.Add(newBinding);
            return outputBindings;
        }

        bool matchedOne = false;

        foreach (FOBindings binding in inputBindings) {
            if (!binding.ContainsKey(this) || binding[this].Equals(expr)) {
                matchedOne = true;
                FOBindings newBinding = new FOBindings();
                
                foreach (KeyValuePair<MetaVariable, Expression> kv in binding) {
                    newBinding.Add(kv.Key, kv.Value);
                }

                if (!binding.ContainsKey(this)) {
                    newBinding.Add(this, expr);    
                }
                outputBindings.Add(newBinding);
            }
        }

        return matchedOne ? outputBindings : null;
    }

    public List<FOBindings> GetBindings(Expression expr) {
        return GetBindings(expr, new List<FOBindings>());
    }

    public List<HOBindings> Unify(IPattern that) {
        UnityEngine.Debug.Log("Stub: MetaVariable.Unify()");
        return null;
    }

    public bool Matches(Expression expr) {
        return expr != null && this.type.Equals(expr.type);
    }

    public bool Matches(Expression expr, List<FOBindings> possibleBindings) {
        if (expr == null) {
            return false;
        }

        bool oneMatched = false;
        foreach (FOBindings binding in possibleBindings) {
            if (this.Matches(expr, binding)) {
                oneMatched = true;
            } else {
                // here, we want to remove this binding from possible bindings:
                // if it can't match the variable, then it's not possible.
            }
        }

        return oneMatched;
    }

    public bool Matches(Expression expr, FOBindings binding) {        
        if (binding.ContainsKey(this)) {
            return binding[this].Equals(expr);
        } else {
            bool matches = this.Matches(expr);

            if (matches) {
                binding.Add(this, expr);
            }

            return matches;
        }
    }

    public HashSet<MetaVariable> GetFreeMetaVariables() {
        HashSet<MetaVariable> xs = new HashSet<MetaVariable>();
        xs.Add(this);
        return xs;
    }

    public IPattern Bind(MetaVariable x, Expression expr) {
        if (this.Equals(x)) {
            return expr;
        } else {
            return this;
        }
    }

    public IPattern Bind(Dictionary<MetaVariable, Expression> binding) {
        if (binding.ContainsKey(this)) {
            return binding[this];
        } else {
            return this;
        }
    }

    public List<IPattern> Bind(List<Dictionary<MetaVariable, Expression>> bindings) {
        List<IPattern> output = new List<IPattern>();
        foreach (Dictionary<MetaVariable, Expression> binding in bindings) {
            bool bound = false;
            foreach (KeyValuePair<MetaVariable, Expression> kv in binding) {
                MetaVariable x = kv.Key;
                Expression e = kv.Value;

                if (x.localID == this.localID && x.type == this.type) {
                    output.Add(e);
                    bound = true;
                }
            }
            if (!bound) {
                output.Add(this);    
            }
        }
        return output;
    }

    public Expression ToExpression() {
        return null;
    }

    public override int GetHashCode() {
        return localID;
    }

    public void AddToDomain(Model m) {}

    public override bool Equals(Object o) {
        MetaVariable that = o as MetaVariable;
        return this.type.Equals(that.type) && this.localID == that.localID;
    }

    public override String ToString() {
        return "{" + localID + ":" + type + "}";
    }
}
