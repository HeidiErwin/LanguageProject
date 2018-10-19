using System;

// See "SemanticType.cs" for more info
public class T : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "t";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(T);
    }

    public override int GetHashCode() {
        return 53;
    }
}
