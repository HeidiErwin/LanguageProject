using System;
using System.Collections;
using System.Collections.Generic;

// see "SemanticType.cs" for more info
public class Arrow : SemanticType {
    private SemanticType input;
    private SemanticType output;

    public Arrow(SemanticType input, SemanticType output) {
        this.input  = input;
        this.output = output;
    }

    public override SemanticType GetInputType() {
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

        return this.input.Equals(that.input)
            && this.output.Equals(that.output);
    }

    public override string ToString() {
        return "(" + input.ToString()
            + "->" + output.ToString() + ")";
    }
}
