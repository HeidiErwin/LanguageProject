public class CustomModels {
    private static Expression bob = Expression.BOB;
    private static Expression evan = Expression.EVAN;
    private static Expression redArea = new Parameter(SemanticType.INDIVIDUAL, 7);
    private static Expression blueArea = new Parameter(SemanticType.INDIVIDUAL, 8);
    private static Expression yellowArea = new Parameter(SemanticType.INDIVIDUAL, 9);
    private static Expression greenArea = new Parameter(SemanticType.INDIVIDUAL, 10);

    public static Model BobModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));

        m.Add(new Phrase(Expression.PERSON, bob));
        m.Add(new Phrase(Expression.ACTIVE, bob));
        m.Add(new Phrase(Expression.KING, bob));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, bob, redArea));

        m.Add(new Phrase(Expression.PERSON, evan));
        m.Add(new Phrase(Expression.ACTIVE, evan));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, evan, greenArea));

        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, bob, evan)));

        return m;
    }

    public static Model EvanModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));

        m.Add(new Phrase(Expression.PERSON, evan));
        m.Add(new Phrase(Expression.ACTIVE, evan));
        m.Add(new Phrase(Expression.KING, evan));

        m.Add(new Phrase(Expression.CONTAINED_WITHIN, evan, greenArea));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, bob, evan)));

        return m;
    }
}
