using System;
using UnityEngine;

using static Expression;

public class Evan : NPC {
    void Start() {
        base.Start();
        CustomModels.AddFPExperimentModel(this.model);

        this.name = EVAN;

        // CustomModels.AddDoorModel(this.model);
        // Substitution Rules
        
        model.Add(
            new Phrase(ABLE,
                SELF,
                new Phrase(AT, SELF, BOB)));

        model.Add(
            new Phrase(ABLE, 
                SELF,
                new Phrase(AT, SELF, DOOR)));
        
        model.Add(new Phrase(IF,
                new Phrase(AT, SELF, DOOR),
                new Phrase(ABLE, SELF, new Phrase(OPEN, DOOR))));

        model.Add(new Phrase(IF,
                new Phrase(OPEN, DOOR),
                new Phrase(ABLE, SELF, new Phrase(AT, SELF, GOAL))));

        // Action Rules
        // model.Add(new ActionRule(
        //     new Phrase(AT, SELF, DOOR),
        //     new Phrase(WOULD, new Phrase(OPEN, DOOR)),
        //     new Phrase(OPEN, DOOR)));

        // model.Add(new ActionRule(
        //     new Phrase(AT, SELF, DOOR),
        //     new Phrase(WOULD, new Phrase(CLOSED, DOOR)),
        //     new Phrase(CLOSED, DOOR)));

        // // particular beliefs

        // self-knowledge
        model.Add(new Phrase(IDENTITY, SELF, EVAN));
        model.Add(new Phrase(IN_THE_ROOM, SELF));
        // model.Add(new Phrase(KING, SELF));
        model.Add(new Phrase(NOT, new Phrase(IDENTITY, BOB, EVAN)));

        // // beliefs about Bob
        // // model.Add(new Phrase(PERSON, BOB));
        // // model.Add(new Phrase(ACTIVE, BOB));
        // // model.Add(new Phrase(BELIEVE, BOB, new Phrase(KING, BOB)));

        // // // // // utilities
        // model.SetUtility(new Phrase(AT, SELF, GOAL), 5f);
        // model.SetUtility(new Phrase(AT, SELF, BOB), 2f);
        
        // model.Add(new Phrase(BETTER,
        //     new Phrase(AT, SELF, BOB),
        //     NEUTRAL));
        
        // model.Add(new Phrase(BETTER,
        //     new Phrase(AT, SELF, GOAL),
        //     new Phrase(AT, SELF, BOB)));
    }
}
