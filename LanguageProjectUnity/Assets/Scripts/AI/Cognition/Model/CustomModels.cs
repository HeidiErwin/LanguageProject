public class CustomModels {
    private static Expression bob = Expression.BOB;
    private static Expression evan = Expression.EVAN;
    private static Expression redArea = new Parameter(SemanticType.INDIVIDUAL, 7);
    private static Expression blueArea = new Parameter(SemanticType.INDIVIDUAL, 8);
    private static Expression yellowArea = new Parameter(SemanticType.INDIVIDUAL, 9);
    private static Expression greenArea = new Parameter(SemanticType.INDIVIDUAL, 10);

    public static Model BobModel() {
        Model m = DefaultModel.Make();

        // Substitution Rules

        // primitive abilities

        // Action Rules
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, evan)),
            new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN)));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, evan),
            new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, evan, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)))),
            new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, evan),
            new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, evan, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)))),
            new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));

        // m.Add(new ActionRule(
        //     new Phrase(Expression.NEAR, Expression.BOB, new Phrase(Expression.THE, Expression.DOOR)),
        //     new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))),
        //     new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

        // m.Add(new ActionRule(
        //     new Phrase(Expression.NEAR, Expression.BOB, new Phrase(Expression.THE, Expression.DOOR)),
        //     new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))),
        //     new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));

        // particular beliefs
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));

        // self-knowledge
        m.Add(new Phrase(Expression.IDENTITY, Expression.SELF, Expression.BOB));
        // m.Add(new Phrase(Expression.PERSON, bob));
        // m.Add(new Phrase(Expression.ACTIVE, bob));
        m.Add(new Phrase(Expression.KING, Expression.SELF));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, Expression.SELF, redArea));
        m.Add(new Phrase(Expression.IDENTITY, bob, Expression.BOB_2));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.SELF, evan)));

        // beliefs about Evan
        m.Add(new Phrase(Expression.PERSON, evan));
        m.Add(new Phrase(Expression.ACTIVE, evan));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, evan, greenArea));
        m.Add(new Phrase(Expression.BELIEVE, evan, new Phrase(Expression.KING, evan)));

        return m;
    }

    public static Model EvanModel() {
        Model m = DefaultModel.Make();

        // Substitution Rules

        // Action Rules
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)),
            new Phrase(Expression.NEAR, Expression.SELF, Expression.BOB)));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR)),
            new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR)),
            new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));


        // particular beliefs
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));

        // self-knowledge
        // m.Add(new Phrase(Expression.PERSON, evan));
        // m.Add(new Phrase(Expression.ACTIVE, evan));
        
        m.Add(new Phrase(Expression.KING, Expression.SELF));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, Expression.SELF, greenArea));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, bob, evan)));

        // beliefs about Bob
        m.Add(new Phrase(Expression.PERSON, bob));
        m.Add(new Phrase(Expression.ACTIVE, bob));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, bob, redArea));
        m.Add(new Phrase(Expression.BELIEVE, bob, new Phrase(Expression.KING, bob)));

        return m;
    }
}
