using System.Collections.Generic;

using static Expression;

public class CustomModels {
    public static void AddDoorModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD,
                new Phrase(AT, SELF, DOOR)),
            new Phrase(AT, SELF, DOOR)));

        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD, new Phrase(AT, SELF, BOB)),
            new Phrase(AT, SELF, BOB)));

        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD, new Phrase(AT, SELF, EVAN)),
            new Phrase(AT, SELF, EVAN)));

        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD, new Phrase(AT, SELF, DOOR)),
            new Phrase(AT, SELF, DOOR)));

        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD, new Phrase(AT, SELF, new Phrase(THE, COW))),
            new Phrase(AT, SELF, new Phrase(THE, COW))));

        // SUBSTITUTION RULES
        // open(x) |- not(closed(x))
        m.Add(new InferenceRule(
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(
                    OPEN,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0)))},
            new List<IPattern>[]{DefaultModel.BuildList(new ExpressionPattern(
                NOT,
                new ExpressionPattern(CLOSED, new MetaVariable(SemanticType.INDIVIDUAL, 0))))}));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(NOT, new Phrase(IDENTITY, BOB, EVAN)));
    }

    public static void AddWoodcutterModel(Model m) {
        // ACTION RULES
        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD,
                new Phrase(AT, SELF, new Phrase(THE, KING))),
            new Phrase(AT, SELF, new Phrase(THE, KING))));

        m.Add(new ActionRule(VERUM,
            new Phrase(WOULD,
                new Phrase(AT, SELF, new Phrase(THE, TREE))),
            new Phrase(AT, SELF, new Phrase(THE, TREE))));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(CREDIBLE, new Phrase(THE, KING)));
    }

    public static void AddFPExperimentModel(Model m) {
        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);

        // // ACTION RULES
        // m.Add(new ActionRule(
        //     new ExpressionPattern(EQUIVALENT,
        //         new Phrase(IN_THE_ROOM, SELF),
        //         new ExpressionPattern(IN_THE_ROOM, xi0)),
        //     new ExpressionPattern(WOULD,
        //         new ExpressionPattern(AT, SELF, xi0)),
        //     new ExpressionPattern(AT, SELF, xi0)));

        // m.Add(new ActionRule(
        //     new Phrase(OPEN, DOOR),
        //     new ExpressionPattern(WOULD,
        //         new ExpressionPattern(AT, SELF, xi0)),
        //     new ExpressionPattern(AT, SELF, xi0)));

        // COMMON KNOWLEDGE
        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(IN_THE_ROOM, BOB)));
        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(IN_THE_ROOM, EVAN)));
        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(IN_THE_ROOM, DOOR)));
        m.Add(new Phrase(PERCEIVE, SELF,
            new Phrase(NOT, new Phrase(IN_THE_ROOM, GOAL))));
        m.Add(new Phrase(CREDIBLE, PLAYER));


        // m.Add(new Phrase(POSSESS, BOB, RUBY));

        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(POSSESS, BOB, RUBY)));
        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(POSSESS, EVAN, SAPPHIRE)));
        m.Add(new Phrase(PERCEIVE, SELF, new Phrase(POSSESS, PLAYER, EMERALD)));
    }

    public static void AddVillageModel(Model m) {
        // MetaVariable xi0 = new MetaVariable(SemanticType.INDIVDUAL, 0);
    }
}
