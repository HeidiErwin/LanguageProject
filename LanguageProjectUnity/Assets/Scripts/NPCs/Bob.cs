using System;
using UnityEngine;

public class Bob : NPC {

    void Start() {
        base.Start();
        this.name = Expression.BOB;

        if (GameObject.Find("GameController").GetComponent<GameController>().is2D) {
            CustomModels.AddDoorModel(this.model);    
        } else {
            CustomModels.AddFPExperimentModel(this.model);
        }

        model.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN),
            new Phrase(Expression.WOULD,
                new Phrase(Expression.INTEND, Expression.EVAN,
                    new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR)))),
            new Phrase(Expression.OPEN, new Phrase(Expression.THE, Expression.DOOR))));

        model.Add(new ActionRule(
            new Phrase(Expression.NEAR, Expression.SELF, Expression.EVAN),
            new Phrase(Expression.WOULD,
                new Phrase(Expression.INTEND, Expression.EVAN,
                    new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR)))),
            new Phrase(Expression.CLOSED, new Phrase(Expression.THE, Expression.DOOR))));

        // self-knowledge
        model.Add(new Phrase(Expression.IDENTITY, Expression.SELF, Expression.BOB));
        model.Add(new Phrase(Expression.KING, Expression.SELF));
        // model.Add(new Phrase(Expression.NOT, new Phrase(Expression.IDENTITY, Expression.BOB, Expression.EVAN)));

        // beliefs about Evan
        // model.Add(new Phrase(Expression.PERSON, Expression.EVAN));
        // model.Add(new Phrase(Expression.ACTIVE, Expression.EVAN));
        // model.Add(new Phrase(Expression.BELIEVE, Expression.EVAN, new Phrase(new Phrase(Expression.KING, Expression.BOB))));
        // model.Add(new Phrase(Expression.BELIEVE, Expression.EVAN, new Phrase(Expression.KING, Expression.EVAN)));
    }
}
