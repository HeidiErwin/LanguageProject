using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// see "SemanticType.cs" for more info
public class Arrow : SemanticType {
    private List<SemanticType> input;
    private SemanticType output;

    public Arrow(List<SemanticType> input, SemanticType output) {
        // require that the output has an atomic semantic type. (for now)
        if (!output.IsAtomic()) {
            throw new ArgumentException();    
        }

        this.input  = input;
        this.output = output;
    }

    public override bool IsAtomic() {
        return false;
    }

    public override int GetNumArgs() {
        return input.Count;
    }

    public override SemanticType GetInputType(int index) {
        return input[index];
    }

    public override List<SemanticType> GetInputType() {
        return input;
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
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append(" -> " + output.ToString() + ")");

        return s.ToString();
    }
}
