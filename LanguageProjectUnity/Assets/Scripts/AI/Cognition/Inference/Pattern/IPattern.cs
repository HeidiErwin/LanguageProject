using System.Collections.Generic;
// First order bindings -> variables can only bind to expressions
using FOBindings = System.Collections.Generic.Dictionary<MetaVariable, Expression>;
// Higher-order bindings -> variables can bind to other variables
using HOBindings = System.Collections.Generic.Dictionary<MetaVariable, IPattern>;
// A pattern may consist of an expression or a variable.
public interface IPattern {
    SemanticType GetSemanticType();
    bool Matches(Expression expr);
    List<FOBindings> GetBindings(Expression expr, List<Dictionary<MetaVariable, Expression>> inputBindings);
    List<FOBindings> GetBindings(Expression expr);
    List<HOBindings> Unify(IPattern that);
    void AddToDomain(Model m);
    HashSet<MetaVariable> GetFreeMetaVariables();
    IPattern Bind(MetaVariable x, Expression expr);
    IPattern Bind(FOBindings bindings);
    List<IPattern> Bind(List<FOBindings> bindings);
    Expression ToExpression();
}
