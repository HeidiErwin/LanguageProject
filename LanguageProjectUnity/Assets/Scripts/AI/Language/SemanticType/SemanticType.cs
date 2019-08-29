using System.Collections.Generic;
using UnityEngine;

// this is a type system for logical forms.
// it's a way of restricting what's expressible
// as a logical form: think about it as a
// grammatical marker like singular/plural.
// 
// just like you can't grammatically say
// "he walk",
// you can't grammatically say "F_{e->t}(x_t)",
// but you can grammatically say "F_{e->t}(x_e)"
// 
// this allows logical forms to mark variables and constants
// with semantic types.
// 
// the following are possible semantic types, defined recursively:
// 
// semantictype := e
// semantictype := t
// semantictype := c
// semantictype := (semantictype, ..., semantictype -> e|t|c)
// 
// the third is implemented in "Arrow.cs".
// 
// "e" is used to restrict to expressions which refer to an individual
// "t" is used to restrict to expressions which refer to a truth value
// "c" is used to restrict to expressions which refer to a conformity value
// "X1, X2, ... Xn -> Y" is used to restrict to expressions which refer to
// n-argument functions from (X1, X2, ..., Xn)s to Ys.
// 
public abstract class SemanticType {
    // the fully opaque color of this semantic type.
    public Color color {
        get {
            if (this.Equals(SemanticType.INDIVIDUAL)) {
                return new Color32(220, 20, 60,  255);
            }

            if (this.Equals(SemanticType.TRUTH_VALUE)) {
                return new Color32(23, 108, 255,  255);
            }

            if (this.Equals(SemanticType.CONFORMITY_VALUE)) {
                return new Color32(0, 0, 0, 255);
            }

            if (this.Equals(SemanticType.PREDICATE)) {
                return new Color32(9, 128, 37, 255);
            }

            if (this.Equals(SemanticType.PREDICATE_MODIFIER)) {
                return new Color32(170, 255, 128, 255);
            }

            if (this.Equals(SemanticType.PREDICATE_MODIFIER_2)) {
                return new Color32(0, 204, 102, 255);
            }

            if (this.Equals(SemanticType.RELATION_2)) {
                return new Color32(240, 240, 60, 255);
            }

            if (this.Equals(SemanticType.RELATION_3)) {
                return new Color32(137, 132, 68, 255);
            }

            if (this.Equals(SemanticType.RELATION_2_REDUCER)) {
                return new Color32(47, 79, 79, 255);
            }

            if (this.Equals(SemanticType.QUANTIFIER)) {
                return new Color32(137, 28, 232, 255);
            }

            if (this.Equals(SemanticType.DETERMINER)) {
                return new Color32(255, 150, 20, 255);
            }

            if (this.Equals(SemanticType.QUANTIFIER_PHRASE)) {
                return new Color32(255, 200, 20, 255);
            }

            if (this.Equals(SemanticType.TRUTH_FUNCTION_1)) {
                return new Color32(59, 47, 67, 255);
            }

            if (this.Equals(SemanticType.GEACH_TRUTH_FUNCTION_1)) {
                return new Color32(59, 80, 67, 255);
            }

            if (this.Equals(SemanticType.TRUTH_FUNCTION_2)) {
                return new Color32(41, 222, 215, 255);
            }

            if (this.Equals(SemanticType.TRUTH_CONFORMITY_FUNCTION)) {
                return new Color32(23, 50, 125, 255);
            }

            if (this.Equals(SemanticType.TRUTH_ASSERTION_FUNCTION)) {
                return new Color32(153, 204, 255, 255);
            }

            if (this.Equals(SemanticType.INDIVIDUAL_TRUTH_RELATION)) {
                return new Color32(137, 134, 54, 255);
            }

            return new Color32(255, 255, 255, 255);
        }
    }
    
    // the semitransparent color of the output
    public Color outputColor {
        get {
            SemanticType determiningType;

            if (this.IsAtomic()) {
                determiningType = this;
            } else {
                determiningType = this.GetOutputType();
            }

            return determiningType.color + new Color(0.25f, 0.25f, 0.25f, 0f) - new Color(0, 0, 0, (1 - ExpressionPiece.EXPRESSION_OPACITY));
        }
    }

    // returns true if the semantic type is not a
    // functional type, as in e or t or whatever else.
    public abstract bool IsAtomic();
    
    // returns the number of arguments the function accepts,
    // if it's a functional type. If not, it returns 0.
    public virtual int GetNumArgs() {
        return 0;
    }

    // these apply only to the functional types,
    // returning the input or output type of this type,
    // and returning null if they're not a functional type
    public virtual SemanticType GetInputType(int n) {
        return null;
    }

    public virtual SemanticType GetOutputType() {
        return null;
    }

    // public abstract bool Matches(SemanticType t);

    public static readonly SemanticType INDIVIDUAL = new E();
    public static readonly SemanticType EVENT = new I();
    public static readonly SemanticType TRUTH_VALUE = new T();
    public static readonly SemanticType CONFORMITY_VALUE = new C();
    public static readonly SemanticType ASSERTION = new A();
    public static readonly SemanticType TRUTH_CONFORMITY_FUNCTION = new Arrow(new SemanticType[]{ TRUTH_VALUE }, CONFORMITY_VALUE);
    public static readonly SemanticType TRUTH_CONFORMITY_FUNCTION_2 = new Arrow(new SemanticType[]{ TRUTH_VALUE, TRUTH_VALUE }, CONFORMITY_VALUE);
    public static readonly SemanticType TRUTH_ASSERTION_FUNCTION = new Arrow(new SemanticType[]{ TRUTH_VALUE }, ASSERTION);
    public static readonly SemanticType INDIVIDUAL_FUNCTION_1 = new Arrow(new SemanticType[]{ INDIVIDUAL }, INDIVIDUAL);
    public static readonly SemanticType INDIVIDUAL_FUNCTION_2 = new Arrow(new SemanticType[]{ INDIVIDUAL, INDIVIDUAL }, INDIVIDUAL);
    public static readonly SemanticType PREDICATE  = new Arrow(new SemanticType[]{ INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType PREDICATE_MODIFIER = new Arrow(new SemanticType[]{ PREDICATE, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType PREDICATE_MODIFIER_2 = new Arrow(new SemanticType[]{ PREDICATE, PREDICATE, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType RELATION_2 = new Arrow(new SemanticType[]{ INDIVIDUAL, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType RELATION_FUNCTION_2 = new Arrow(new SemanticType[]{ RELATION_2, INDIVIDUAL, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType RELATION_3 = new Arrow(new SemanticType[]{ INDIVIDUAL, INDIVIDUAL, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType DETERMINER = new Arrow(new SemanticType[]{ PREDICATE }, INDIVIDUAL);
    public static readonly SemanticType QUANTIFIER = new Arrow(new SemanticType[]{ PREDICATE, PREDICATE }, TRUTH_VALUE);
    public static readonly SemanticType QUANTIFIER_PHRASE = new Arrow(new SemanticType[]{ PREDICATE }, TRUTH_VALUE);
    public static readonly SemanticType GEACH_QUANTIFIER_PHRASE = new Arrow(new SemanticType[]{ QUANTIFIER_PHRASE, RELATION_2, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType TRUTH_FUNCTION_1 = new Arrow(new SemanticType[]{ TRUTH_VALUE }, TRUTH_VALUE );
    public static readonly SemanticType TRUTH_FUNCTION_2 = new Arrow(new SemanticType[]{ TRUTH_VALUE, TRUTH_VALUE }, TRUTH_VALUE);
    public static readonly SemanticType EVENT_TRUTH_RELATION = new Arrow(new SemanticType[]{ EVENT, TRUTH_VALUE }, TRUTH_VALUE);
    public static readonly SemanticType INDIVIDUAL_TRUTH_RELATION = new Arrow(new SemanticType[]{ INDIVIDUAL, TRUTH_VALUE }, TRUTH_VALUE);
    public static readonly SemanticType EVENT_INDIVIDUAL_TRUTH_RELATION = new Arrow(new SemanticType[]{ EVENT, INDIVIDUAL, TRUTH_VALUE }, TRUTH_VALUE);
    public static readonly SemanticType RELATION_2_REDUCER = new Arrow(new SemanticType[]{ RELATION_2, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType GEACH_TRUTH_FUNCTION_1 =
        new Arrow(new SemanticType[]{ TRUTH_FUNCTION_1, PREDICATE, INDIVIDUAL }, TRUTH_VALUE);
    public static readonly SemanticType GEACH_TRUTH_FUNCTION_2 =
        new Arrow(new SemanticType[]{ TRUTH_FUNCTION_2, PREDICATE, PREDICATE, INDIVIDUAL }, TRUTH_VALUE);
}
