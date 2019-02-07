using System;

// see "SemanticType.cs" for more info
public class I : SemanticType {

    public override bool IsAtomic() {
        return true;
    }

    public override String ToString() {
        return "i";
    }

    public override bool Equals(Object o) {
        return o.GetType() == typeof(I);
    }

    public override int GetHashCode() {
        return 97;
    }
}
