public class DefaultModel {
    public static Model Make() {
        Model m = new SimpleModel();

        // // SPECIAL EVALUATION RULES
        // m.Add(EvaluationRule.NOT);
        // m.Add(EvaluationRule.EVERY);

        // // DEFAULT EVALUATION RULES
        // m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_1);
        // m.Add(EvaluationRule.DEFAULT_TRUTH_FUNCTION_2);
        // m.Add(EvaluationRule.DEFAULT_DETERMINER);
        // m.Add(EvaluationRule.DEFAULT_PREDICATE);
        // m.Add(EvaluationRule.DEFAULT_RELATION_2);
        // m.Add(EvaluationRule.DEFAULT_RELATION_3);

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xi1 = new MetaVariable(SemanticType.INDIVIDUAL, 1);
        MetaVariable xi2 = new MetaVariable(SemanticType.INDIVIDUAL, 2);
        MetaVariable xp0 = new MetaVariable(SemanticType.PREDICATE, 0);
        MetaVariable xp1 = new MetaVariable(SemanticType.PREDICATE, 1);
        MetaVariable xr20 = new MetaVariable(SemanticType.RELATION_2, 0);
        MetaVariable xitr0 = new MetaVariable(SemanticType.INDIVIDUAL_TRUTH_RELATION, 0);
        MetaVariable xtf10 = new MetaVariable(SemanticType.TRUTH_FUNCTION_1, 0);
        MetaVariable xqp0 = new MetaVariable(SemanticType.QUANTIFIER_PHRASE, 0);

        Expression not = Expression.NOT;

        // verum and falsum
        m.Add(Expression.VERUM);
        m.Add(new Phrase(not, Expression.FALSUM));

        // SELF-KNOWLEDGE
        m.Add(new Phrase(Expression.PERSON, Expression.SELF));
        m.Add(new Phrase(Expression.ACTIVE, Expression.SELF));

        // ACTION RULES
        m.Add(new ActionRule(Expression.VERUM,
            new Phrase(Expression.WOULD, new Phrase(Expression.NEAR, Expression.SELF, Expression.THE_GREAT_DOOR)),
            new Phrase(Expression.NEAR, Expression.SELF, Expression.THE_GREAT_DOOR)));

        // SUBSTITUTION RULES
        
        // S |- believes(self, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{new MetaVariable(SemanticType.TRUTH_VALUE, 0)},
            new IPattern[]{new ExpressionPattern(Expression.BELIEVE, Expression.SELF, new MetaVariable(SemanticType.TRUTH_VALUE, 0))},
            false));

        // S |- T(S)
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.TRUE, xt0)},
            EntailmentContext.Downward, false));

        // T(S) |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.TRUE, xt0)},
            new IPattern[]{xt0},
            EntailmentContext.Upward));

        // S |- ~~S
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(not, xt0))},
            EntailmentContext.Downward, false));

        // ~~S |- S
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(not, new ExpressionPattern(not, xt0))},
            new IPattern[]{xt0},
            EntailmentContext.Upward, false));

        // t -> t
        // !F(x) |- G(!, F, x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xtf10, new ExpressionPattern(xp0, xi0))},
            new IPattern[]{new ExpressionPattern(Expression.GEACH_TF1, xtf10, xp0, xi0)},
            EntailmentContext.Downward, false));
        
        // Q(R(x, _)) |- G(Q, R, x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xqp0, new ExpressionPattern(xr20, xi0))},
        //     new IPattern[]{new ExpressionPattern(xqp0, xr20, xi0)}));

        // A, B |- A & B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            EntailmentContext.Downward));

        // A & B |- A
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            new IPattern[]{xt0},
            EntailmentContext.Upward));

        // A & B |- B
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            new IPattern[]{xt1},
            EntailmentContext.Upward));

        // A v B |- A, B
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            new IPattern[]{xt0, xt1},
            EntailmentContext.Upward));

        // A |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));

        // B |- A v B
        m.Add(new SubstitutionRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward));

        // reflexivity for identity
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi0)}));

        // symmetry for identity
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi1, xi0)}));

        // transitivity for identity
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi1),
                new ExpressionPattern(Expression.IDENTITY, xi1, xi2)},
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi2)}));

        // // substitution of identiticals
        // the rule if subsentential substitution was a thing
        // // [i = j] i |- j
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
        //     new IPattern[]{xi0},
        //     new IPattern[]{xi1}));
        //     
        // the cheat for now
        // [i = j] F(i) |- F(j)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xp0, xi0)},
            new IPattern[]{new ExpressionPattern(xp0, xi1)}));

        // [i = j] R(i, k) |- R(j, k)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xr20, xi0, xi2)},
            new IPattern[]{new ExpressionPattern(xr20, xi1, xi2)}));

        // [i = j] R(k, i) |- R(k, j)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xr20, xi2, xi0)},
            new IPattern[]{new ExpressionPattern(xr20, xi2, xi1)}));

        // [i = j] A(i, S) |- A(j, S)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.IDENTITY, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(xitr0, xi0, xt0)},
            new IPattern[]{new ExpressionPattern(xitr0, xi1, xt1)}));

        // F(x), G(x) |- some(F, G)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(xp0, xi0), new ExpressionPattern(xp1, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.SOME, xp0, xp1)}));

        // // [i != j, F(i), F(j)] G(i), G(j) |- Two(F, G)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.IDENTITY, xi0, xi1)),
        //         new ExpressionPattern(xp0, xi0),
        //         new ExpressionPattern(xp0, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(xp1, xi0),
        //         new ExpressionPattern(xp1, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.TWO, xp0, xp1)}));

        // // [i != j, F(i), F(j)] Two(F, G) |- G(i) & G(j)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.IDENTITY, xi0, xi1)),
        //         new ExpressionPattern(xp0, xi0),
        //         new ExpressionPattern(xp0, xi1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.TWO, xp0, xp1)},
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.AND,
        //         new ExpressionPattern(xp1, xi0),
        //         new ExpressionPattern(xp1, xi1))}));

        // every(F, G), F(x) |- G(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{
        //         new ExpressionPattern(Expression.EVERY, xp0, xp1),
        //         new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(xp1, xi0)}));

        // // |- every(F, F)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{},
        //     new IPattern[]{new ExpressionPattern(Expression.EVERY, xp0, xp0)}));
        
        // // F(x) |- exists(x)
        // m.Add(new SubstitutionRule(
        //     new IPattern[]{new ExpressionPattern(xp0, xi0)},
        //     new IPattern[]{new ExpressionPattern(Expression.EXISTS, xi0)}));

        // antisymmetry for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi0))},
            EntailmentContext.Downward));

        // transitivity for contained_within
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi1),
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi1, xi2)},
            new IPattern[]{
                new ExpressionPattern(Expression.CONTAINED_WITHIN, xi0, xi2)}));

        // uniqueness of king
        // king(i), king(j) |- i = j
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.KING, xi0),
                new ExpressionPattern(Expression.KING, xi1)},
            new IPattern[]{
                new ExpressionPattern(Expression.IDENTITY, xi0, xi1)}));

        // reflexivity for overlaps_with
        m.Add(new SubstitutionRule(
            new IPattern[]{},
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi0)}));

        // symmetry for overlaps_with
        m.Add(new SubstitutionRule(
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi0, xi1)},
            new IPattern[]{
                new ExpressionPattern(Expression.OVERLAPS_WITH, xi1, xi0)}));

        // cow(x) |- animal(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.COW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.ANIMAL, xi0)}));

        // person(x) |- animal(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.ANIMAL, xi0)}));

        // active(x) |- ~inactive(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.ACTIVE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.INACTIVE, xi0))}));

        // inactive(x) |- ~active(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.INACTIVE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.ACTIVE, xi0))}));

        // open(x) |- ~closed(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.OPEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CLOSED, xi0))}));

        // closed(x) |- ~open(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CLOSED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.OPEN, xi0))}));

        // person(x) |- ~fountain(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.FOUNTAIN, xi0))}));

        // person(x) |- ~lamp(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.LAMP, xi0))}));

        // person(x) |- ~cow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.COW, xi0))}));

        // person(x) |- ~open(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.OPEN, xi0))}));

        // person(x) |- ~closed(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.PERSON, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CLOSED, xi0))}));

        // COLOR EXCLUSION
        // black(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // black(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // black(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // black(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // black(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // black(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // black(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLACK, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        // red(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // red(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // red(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // red(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // red(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // red(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // red(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.RED, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        // green(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // green(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // green(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // green(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // green(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // green(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // green(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.GREEN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        
        // blue(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // blue(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // blue(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // blue(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // blue(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // blue(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // blue(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.BLUE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        
        // cyan(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // cyan(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // cyan(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // cyan(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // cyan(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // cyan(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // cyan(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.CYAN, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        
        // magenta(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // magenta(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // magenta(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // magenta(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // magenta(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // magenta(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // magenta(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.MAGENTA, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        
        // yellow(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // yellow(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // yellow(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // yellow(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // yellow(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // yellow(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // yellow(x) |- ~white(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.YELLOW, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.WHITE, xi0))}));

        // =======================================================================================================
        
        // white(x) |- ~black(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLACK, xi0))}));

        // white(x) |- ~red(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.RED, xi0))}));

        // white(x) |- ~green(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.GREEN, xi0))}));

        // white(x) |- ~blue(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.BLUE, xi0))}));

        // white(x) |- ~cyan(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.CYAN, xi0))}));

        // white(x) |- ~magenta(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.MAGENTA, xi0))}));

        // white(x) |- ~yellow(x)
        m.Add(new SubstitutionRule(
            new IPattern[]{new ExpressionPattern(Expression.WHITE, xi0)},
            new IPattern[]{new ExpressionPattern(Expression.NOT, new ExpressionPattern(Expression.YELLOW, xi0))}));

        // =======================================================================================================

        return m;
    }
}
