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

        if (input.Length != that.GetNumArgs()) {
            return false;
        }
        
        for (int i = 0; i < input.Length; i++) {
            if (input[i] != that.GetInputType(i)) {
                return false;
            }
        }

        return output.Equals(that.GetOutputType());
    }

    public override int GetHashCode() {
        int hash = 5381 * output.GetHashCode();
        for (int i = 0; i < input.Length; i++) {
            hash = 33 * hash + (input[i] == null ? i : input[i].GetHashCode());
        }
        return hash;
    }

    public override string ToString() {
        StringBuilder s = new StringBuilder();
        s.Append("(");

        for (int i = 0; i < input.Length; i++) {
            s.Append(input[i].ToString());
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append(" -> " + output.ToString() + ")");

        return s.ToString();
    }
}
