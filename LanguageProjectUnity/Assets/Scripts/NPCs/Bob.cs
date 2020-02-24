using System;
using UnityEngine;
using static Expression;

public class Bob : NPC {

    void Start() {
        base.Start();
        CustomModels.AddFPExperimentModel(this.model);
        this.name = BOB;

        if (GameObject.Find("GameController").GetComponent<GameController>().is2D) {
            CustomModels.AddDoorModel(this.model);    
        } else {
            CustomModels.AddFPExperimentModel(this.model);
        }

        MetaVariable xi0 = new MetaVariable(SemanticType.INDIVIDUAL, 0);
        MetaVariable xi1 = new MetaVariable(SemanticType.INDIVIDUAL, 1);

        // model.Add(new ActionRule(
        //     VERUM,
        //     new ExpressionPattern(WOULD, new ExpressionPattern(POSSESS, xi0, RUBY)),
        //     new ExpressionPattern(POSSESS, xi0, RUBY)));

        // model.Add(new ActionRule(
        //     new Phrase(AT, SELF, EVAN),
        //     new Phrase(WOULD,
        //         new Phrase(INTEND, EVAN,
        //             new Phrase(OPEN, DOOR))),
        //     new Phrase(OPEN, DOOR)));

        // model.Add(new ActionRule(
        //     new Phrase(AT, SELF, EVAN),
        //     new Phrase(WOULD,
        //         new Phrase(INTEND, EVAN,
        //             new Phrase(CLOSED, DOOR))),
        //     new Phrase(CLOSED, DOOR)));

        // // self-knowledge
        model.Add(new Phrase(IDENTITY, SELF, BOB));
        // model.Add(new Phrase(KING, SELF));
        model.Add(new Phrase(POSSESS, SELF, RUBY));
        model.Add(new Phrase(NOT, new Phrase(POSSESS, SELF, SAPPHIRE)));
        model.Add(new Phrase(NOT, new Phrase(POSSESS, SELF, EMERALD)));
        model.Add(new Phrase(NOT, new Phrase(IDENTITY, BOB, EVAN)));


        // testing location
        model.Add(new Phrase(AT, NARROW_TREE, new Phrase(LOCATION, ZERO, FIVE)));
        model.Add(new Phrase(AT, BROAD_TREE, new Phrase(LOCATION, FIVE, FIVE)));

        // model.Add(new Phrase(BETTER, new Phrase(AT, SELF, new Phrase(LOCATION, FIVE, ZERO)), NEUTRAL));

        // beliefs about Evan
        model.Add(new Phrase(PERSON, EVAN));
        model.Add(new Phrase(ABLE, EVAN, new Phrase(OPEN, DOOR)));
        // model.Add(new Phrase(INDIFFERENT, EVAN, new Phrase(OPEN, DOOR), NEUTRAL));
        // model.Add(new Phrase(ACTIVE, EVAN));
        // // model.Add(new Phrase(BELIEVE, EVAN, new Phrase(new Phrase(KING, BOB))));
        // // model.Add(new Phrase(BELIEVE, EVAN, new Phrase(KING, EVAN)));

        model.Add(new Phrase(IN_THE_ROOM, SELF));
        model.Add(new Phrase(POSSESS, SELF, RUBY));

        model.Add(new Phrase(ABLE, SELF, new Phrase(AT, SELF, EVAN)));

        model.Add(new Phrase(IF,
            new Phrase(AT, SELF, EVAN),
            new Phrase(ABLE, SELF,
                new Phrase(EXPRESS_CONFORMITY, SELF, EVAN,
                    new Phrase(WOULD, new Phrase(OPEN, DOOR))))));

        // // utilities
        model.Add(new Phrase(BETTER,
            new Phrase(POSSESS, SELF, EMERALD), NEUTRAL));

        model.Add(new Phrase(BETTER,
            new Phrase(POSSESS, EVAN, RUBY), NEUTRAL));

                model.Add(new Phrase(BETTER,
                new Phrase(OPEN, DOOR),
                NEUTRAL));
    }
}
