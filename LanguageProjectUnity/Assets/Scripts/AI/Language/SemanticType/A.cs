using System;

// see "SemanticType.cs" for more info
public class A : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "a";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(A);
    }

    public override int GetHashCode() {
        return 89;
    }
}
