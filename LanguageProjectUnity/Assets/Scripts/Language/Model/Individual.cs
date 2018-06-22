using System;

public class Individual : ISemanticValue {
    private String id;
    
    public Individual(String id) {
        this.id = id;
    }

    public override String ToString() {
        return id + "'";
    }
}