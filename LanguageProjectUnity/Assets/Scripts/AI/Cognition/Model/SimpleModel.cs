using System;
using System.Text;
using System.Collections.Generic;

// a model represented as simply as possible:
// a set of sentences.
public class SimpleModel : Model {
    private HashSet<Expression> model = new HashSet<Expression>();

    public override bool Contains(Expression e) {
        foreach (Expression expr in model) {
            if (expr.Equals(e)) {
                return true;
            }
        }
        return model.Contains(e);
    }

    public override bool Add(Expression e) {
        if (Contains(e)) {
            return false;
        }
        model.Add(e);
        return true;
    }

    public override bool Remove(Expression e) {
        if (Contains(e)) {
            model.Remove(e);
            return true;
        }

        return false;
    }

    public override HashSet<Dictionary<MetaVariable, Expression>> Find(params IPattern[] patterns) {
        HashSet<Dictionary<MetaVariable, Expression>> possibleBindings = new HashSet<Dictionary<MetaVariable, Expression>>();
        
        if (patterns.Length >= 1) {
            IPattern currentPattern = patterns[0];
            bool anyMatched = false;

            foreach (Expression e in this.model) {
                List<Dictionary<MetaVariable, Expression>> currentBindings = currentPattern.GetBindings(e);
                if (currentBindings != null) {
                    foreach (Dictionary<MetaVariable, Expression> currentBinding in currentBindings) {
                        possibleBindings.Add(currentBinding);
                    }
                    anyMatched = true;
                }
            }
            
            if (!anyMatched) {
                return null;
            }
        }

        for (int i = 1; i < patterns.Length; i++) {
            // for every possible binding, B:
            // take p and bind the current pattern with B.
            // Clone B.
            // when a pattern matches with an expression, add that set of bindings to possible bindings, and remove the old set of bindings
            // if no expressions matched, then return null
            HashSet<Dictionary<MetaVariable, Expression>> nextPossibleBindings = new HashSet<Dictionary<MetaVariable, Expression>>();
            bool anyMatched = false;
            foreach (Dictionary<MetaVariable, Expression> possibleBinding in possibleBindings) {
                IPattern currentPattern = patterns[i].Bind(possibleBinding);
                
                foreach (Expression e in this.model) {
                    List<Dictionary<MetaVariable, Expression>> possibleBindingSingleton = new List<Dictionary<MetaVariable, Expression>>();
                    possibleBindingSingleton.Add(possibleBinding);
                    List<Dictionary<MetaVariable, Expression>> currentBindings = currentPattern.GetBindings(e, possibleBindingSingleton);
                    
                    if (currentBindings != null) {
                        foreach (Dictionary<MetaVariable, Expression> currentBinding in currentBindings) {
                            nextPossibleBindings.Add(currentBinding);
                        }
                        anyMatched = true;
                    }
                }    
            }

            if (!anyMatched) {
                return null;
            }

            possibleBindings = nextPossibleBindings;
        }

        return possibleBindings;
    }

    public override String ToString() {
        StringBuilder s = new StringBuilder();
        s.Append("{\n");
        foreach (Expression e in this.model) {
            s.Append("    " + e + "\n");
        }
        s.Append("}");
        return s.ToString();
    }
}
