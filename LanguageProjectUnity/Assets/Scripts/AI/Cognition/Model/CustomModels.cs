public class CustomModels {
    public static Model BobModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 7), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 8), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 9), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 10), Expression.WAYSIDE_PARK));

        m.Add(new Phrase(Expression.PERSON, Expression.BOB));
        m.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        // m.Add(new Phrase(Expression.KING, Expression.BOB));

        return m;
    }

    public static Model EvanModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 7), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 8), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 9), Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, new Parameter(SemanticType.INDIVIDUAL, 10), Expression.WAYSIDE_PARK));

        m.Add(new Phrase(Expression.PERSON, Expression.EVAN));
        m.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        // m.Add(new Phrase(Expression.KING, Expression.EVAN));

        return m;
    }
}
