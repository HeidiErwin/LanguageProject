using System;

public abstract class Function : ISemanticValue {
    protected String id;
    
    public Function(String id) {
        this.id = id;
    }
}