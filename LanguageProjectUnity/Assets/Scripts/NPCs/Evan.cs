using System;
using UnityEngine;

public class Evan : NPC {
    void Start() {
        base.Start();

        this.name = Expression.EVAN;

        CustomModels.AddDoorModel(this.model);
        // Substitution Rules

        // Action Rules
        model.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR)),
            new Phrase(Expression.WOULD, new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

        model.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, new Phrase(Expression.THE, Expression.DOOR)),
            new Phrase(Expression.WOULD, new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))),
            new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));


        // particular beliefs

        // self-knowledge
        model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, Expression.EVAN));
        model.Add(new Phrase(Expression.KING, Expression.SELF));
        // model.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.BOB, Expression.EVAN)));

        // beliefs about Bob
        // model.Add(new Phrase(Expression.PERSON, Expression.BOB));
        // model.Add(new Phrase(Expression.ACTIVE, Expression.BOB));
        // model.Add(new Phrase(Expression.BELIEVE, Expression.BOB, new Phrase(Expression.KING, Expression.BOB)));
    }
}
