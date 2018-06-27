using System;

// see "SemanticType.cs" for more info
public class E : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "e";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(E);
    }
}
