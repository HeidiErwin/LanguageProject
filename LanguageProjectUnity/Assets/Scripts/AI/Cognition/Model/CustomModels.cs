public class CustomModels {
    public static Model BobModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.PERSON, Expression.BOB));
        m.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        // m.Add(new Phrase(Expression.KING, Expression.BOB));

        return m;
    }

    public static Model EvanModel() {
        Model m = DefaultModel.Make();

        m.Add(new Phrase(Expression.PERSON, Expression.EVAN));
        m.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        // m.Add(new Phrase(Expression.KING, Expression.EVAN));

        return m;
    }
}
