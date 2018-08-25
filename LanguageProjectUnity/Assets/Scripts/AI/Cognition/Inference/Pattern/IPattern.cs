using System.Collections.Generic;

// A pattern may consist of an expression or a variable.

public interface IPattern {
    bool Matches(Expression expr);
    bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings);
    HashSet<MetaVariable> GetFreeMetaVariables();
    IPattern Bind(MetaVariable x, Expression expr);
    Expression ToExpression();
}