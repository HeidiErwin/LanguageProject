using System;

// the primitive linguistic expression, whose name, semantic type,
// and meaning are directly given.
public class Word : Expression {
    public Word(SemanticType type, String name) : base(type) {
        this.name = name;
    }
}