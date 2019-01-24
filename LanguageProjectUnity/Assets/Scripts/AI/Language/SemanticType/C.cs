using System;

// see "SemanticType.cs" for more info
public class C : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "c";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(C);
    }

    public override int GetHashCode() {
        return 67;
    }
}
