using System;
using System.Text;
using System.Collections.Generic;

// a model represented as simply as possible:
// a set of sentences.
public class SimpleModel : Model {
    private HashSet<Expression> model = new HashSet<Expression>();

    public override bool Contains(Expression e) {
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
