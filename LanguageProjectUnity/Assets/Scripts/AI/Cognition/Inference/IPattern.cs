// A pattern may consist of an expression or a variable.

public interface IPattern {
    int GetLocalID();
    bool Matches(Expression expr);
    IPattern Bind(int id, Expression expr);
}
