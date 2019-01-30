using System;

// see "SemanticType.cs" for more info
public class Q : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "q";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(Q);
    }

    public override int GetHashCode() {
        return 89;
    }
}
