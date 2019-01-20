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
        m.Add(new SubstitutionRule(
            new IPattern[]{new MetaVariable(SemanticType.TRUTH_VALUE, 0)},
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, bob, new MetaVariable(SemanticType.TRUTH_VALUE, 0))},
            false));

        // primitive abilities

        // Action Rules
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, bob, Expression.THE_GREAT_DOOR)),
            new Phrase(Expression.NEAR, bob, Expression.THE_GREAT_DOOR)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, bob, evan)),
            new Phrase(Expression.NEAR, bob, Expression.EVAN)));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, bob, evan),
            new Phrase(Expression.WOULD, new Phrase(Expression.DESIRE, evan, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR))),
            new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));

        // m.Add(new ActionRule(
        //     new Phrase(Expression.NEAR, Expression.BOB, Expression.THE_GREAT_DOOR),
        //     new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)),
        //     new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));

        // m.Add(new ActionRule(
        //     new Phrase(Expression.NEAR, Expression.BOB, Expression.THE_GREAT_DOOR),
        //     new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)),
        //     new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)));

        // particular beliefs
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));

        // self-knowledge
        m.Add(new Phrase(Expression.PERSON, bob));
        m.Add(new Phrase(Expression.ACTIVE, bob));
        m.Add(new Phrase(Expression.KING, bob));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, bob, redArea));
        m.Add(new Phrase(Expression.IDENTITY, bob, Expression.BOB_2));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, bob, evan)));

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
        m.Add(new SubstitutionRule(
            new IPattern[]{new MetaVariable(SemanticType.TRUTH_VALUE, 0)},
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, evan, new MetaVariable(SemanticType.TRUTH_VALUE, 0))},
            false));

        // Action Rules
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.EVAN, Expression.THE_GREAT_DOOR)),
            new Phrase(Expression.NEAR, Expression.EVAN, Expression.THE_GREAT_DOOR)));

        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB)),
            new Phrase(Expression.NEAR, Expression.EVAN, Expression.BOB)));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.EVAN, Expression.THE_GREAT_DOOR),
            new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)),
            new Phrase(Expression.OPEN, Expression.THE_GREAT_DOOR)));

        m.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.EVAN, Expression.THE_GREAT_DOOR),
            new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)),
            new Phrase(Expression.CLOSED, Expression.THE_GREAT_DOOR)));


        // particular beliefs
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, redArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, blueArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, greenArea, Expression.WAYSIDE_PARK));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, yellowArea, Expression.WAYSIDE_PARK));

        // self-knowledge
        m.Add(new Phrase(Expression.PERSON, evan));
        m.Add(new Phrase(Expression.ACTIVE, evan));
        m.Add(new Phrase(Expression.KING, evan));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, evan, greenArea));
        m.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, bob, evan)));

        // beliefs about Bob
        m.Add(new Phrase(Expression.PERSON, bob));
        m.Add(new Phrase(Expression.ACTIVE, bob));
        m.Add(new Phrase(Expression.CONTAINED_WITHIN, bob, redArea));
        m.Add(new Phrase(Expression.BELIEVE, bob, new Phrase(Expression.KING, bob)));

        return m;
    }
}
