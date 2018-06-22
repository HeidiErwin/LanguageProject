using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// see "SemanticType.cs" for more info
public class Arrow : SemanticType {
    private SemanticType[] input;
    private SemanticType output;

    public Arrow(SemanticType[] input, SemanticType output) {
        this.input  = input;
        this.output = output;
    }

    public override int GetNumArgs() {
        return input.Length;
    }

    public override SemanticType GetInputType(int index) {
        return input[index];
    }

    public override SemanticType GetOutputType() {
        return output;
    }

    public override bool Equals(Object o) {
        if (o.GetType() != typeof(Arrow)) {
            return false;
        }

        Arrow that = (Arrow) o;

        return this.input.SequenceEqual(that.input)
            && this.output.Equals(that.output);
    }

    public override string ToString() {
        StringBuilder s = new StringBuilder();
        s.Append("(");

        foreach (SemanticType t in input) {
            s.Append(t.ToString());
            s.Append(" | ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append("-> " + output.ToString() + ")");

        return s.ToString();
    }
}
