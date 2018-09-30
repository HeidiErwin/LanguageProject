using System.Collections.Generic;

// A pattern may consist of an expression or a variable.

public interface IPattern {
    SemanticType GetSemanticType();
    bool Matches(Expression expr);
    List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr, List<Dictionary<MetaVariable, Expression>> inputBindings);
    List<Dictionary<MetaVariable, Expression>> GetBindings(Expression expr);
    // bool Matches(Expression expr, Dictionary<MetaVariable, Expression> bindings);
    HashSet<MetaVariable> GetFreeMetaVariables();
    IPattern Bind(MetaVariable x, Expression expr);
    List<IPattern> BindAll(List<Dictionary<MetaVariable, Expression>> bindings);
    Expression ToExpression();
}
