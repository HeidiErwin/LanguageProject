using System.Linq;
using System.Collections.Generic;

public class Testing {
    static void PrintInfoString(Expression e) {
        System.Console.WriteLine(e + ": " + e.GetSemanticType());
    }

    static void Main() {
        SemanticType individual = new E();
        SemanticType truthValue = new T();

        List<SemanticType> e = new List<SemanticType>();
        e.Add(individual);

        List<SemanticType> ee = new List<SemanticType>();
        ee.Add(individual);
        ee.Add(individual);

        List<SemanticType> eee = new List<SemanticType>();
        eee.Add(individual);
        eee.Add(individual);
        eee.Add(individual);

        SemanticType predicate = new Arrow(e, truthValue);
        SemanticType relation2 = new Arrow(ee, truthValue);
        SemanticType relation3 = new Arrow(eee, truthValue);

        List<SemanticType> et = new List<SemanticType>();
        et.Add(predicate);
        List<SemanticType> etet = new List<SemanticType>();
        etet.Add(predicate);
        etet.Add(predicate);

        SemanticType quantifierPhrase = new Arrow(et, truthValue);
        SemanticType quantifier = new Arrow(etet, truthValue);
        SemanticType determiner = new Arrow(et, individual);

        // System.Console.WriteLine("individual: " + individual);
        // System.Console.WriteLine("truth value: " + truthValue);
        // System.Console.WriteLine("predicate: " + predicate);
        // System.Console.WriteLine("2-place relation: " + relation2);
        // System.Console.WriteLine("quantifier: " + quantifier);
        // System.Console.WriteLine("quantifier phrase: " + quantifierPhrase);
        // System.Console.WriteLine("determiner: " + determiner);

        Expression bill = new Word(individual, "Bill");
        Expression heidi = new Word(individual, "Heidi");
        Expression apple = new Word(predicate, "apple");
        Expression red = new Word(predicate, "red");
        Expression the = new Word(determiner, "the");
        Expression round = new Word(predicate, "round");
        Expression helps = new Word(relation2, "helps");
        Expression gives = new Word(relation3, "gives");
        Expression every = new Word(quantifier, "every");

        Expression billIsRed = new Phrase(red, bill);
        Expression billHelps = new Phrase(helps, bill);
        Expression helpsHeidi = new Phrase(helps, heidi, 1);

        PrintInfoString(billHelps);
        PrintInfoString(helpsHeidi);

        Expression billGives = new Phrase(gives, bill, 1);
        Expression billGivesHeidi = new Phrase(billGives, heidi);

        PrintInfoString(billGives);
        PrintInfoString(billGivesHeidi);

        Expression theRoundThingIsRed = new Phrase(red, new Phrase(the, round));

        Expression billHelpsHeidi = new Phrase(billHelps, heidi);
        Expression everyRedThingIsRound = new Phrase(new Phrase(every, red), round);
        Expression billGivesHeidiTheApple = new Phrase(billGivesHeidi, new Phrase(the, apple));

        Expression verum = new Word(truthValue, "T");

        // PrintInfoString(bill);
        // PrintInfoString(heidi);
        // PrintInfoString(red);
        // PrintInfoString(every);
        // PrintInfoString(the);

        PrintInfoString(billIsRed);
        PrintInfoString(billHelpsHeidi);
        PrintInfoString(everyRedThingIsRound);
        PrintInfoString(theRoundThingIsRed);
        PrintInfoString(billGivesHeidiTheApple);

        System.Console.WriteLine("Syntactic Equality:");
        System.Console.WriteLine(bill.Equals(new Word(individual, "Bill")));
        System.Console.WriteLine(!bill.Equals(heidi));
        System.Console.WriteLine(billIsRed.Equals(billIsRed));
        System.Console.WriteLine(!billIsRed.Equals(billHelpsHeidi));

        System.Console.WriteLine("Model:");
        IModel model = new PrefixModel();
        System.Console.WriteLine(model.Add(billIsRed));
        System.Console.WriteLine(!model.Add(billIsRed));
        System.Console.WriteLine(model.Add(billGivesHeidiTheApple));
        System.Console.WriteLine(model.Add(theRoundThingIsRed));

        System.Console.WriteLine(model.Contains(billIsRed));
        System.Console.WriteLine(!model.Contains(billHelpsHeidi));

        System.Console.WriteLine(!model.Remove(billHelpsHeidi));
        System.Console.WriteLine(model.Remove(billIsRed));

        System.Console.WriteLine(!model.Contains(billIsRed));

        System.Console.WriteLine(model.Add(verum));
        System.Console.WriteLine(model.Contains(verum));
        System.Console.WriteLine(model.Remove(verum));
        System.Console.WriteLine(!model.Contains(verum));

        System.Console.WriteLine(model.Add(helps));
        System.Console.WriteLine(model.Contains(helps));
        System.Console.WriteLine(model.Remove(helps));
        System.Console.WriteLine(!model.Contains(helps));

        System.Console.WriteLine(model.Add(red));
        System.Console.WriteLine(model.Contains(red));
        System.Console.WriteLine(model.Remove(red));
        System.Console.WriteLine(!model.Contains(red));
    }
}
