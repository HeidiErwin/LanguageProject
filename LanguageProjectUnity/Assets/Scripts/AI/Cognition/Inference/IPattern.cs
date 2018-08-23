using System.Collections.Generic;

// A pattern may consist of an expression or a variable.

public interface IPattern {
    bool Matches(Expression expr);
    HashSet<MetaVariable> GetFreeMetaVariables();
    IPattern Bind(MetaVariable x, Expression expr);
}
