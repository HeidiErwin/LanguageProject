public class Testing {
    static void PrintInfoString(Expression e) {
        System.Console.WriteLine(e + ": " + e.GetSemanticType());
    }

    static void Main() {
        E individual = new E();
        T truthValue = new T();
        SemanticType predicate = new Arrow(individual, truthValue);
        SemanticType relation2 = new Arrow(individual, predicate);
        SemanticType quantifierPhrase = new Arrow(predicate, truthValue);
        SemanticType quantifier = new Arrow(predicate, quantifierPhrase);
        SemanticType determiner = new Arrow(predicate, individual);

        System.Console.WriteLine("individual: " + individual);
        System.Console.WriteLine("truth value: " + truthValue);
        System.Console.WriteLine("predicate: " + predicate);
        System.Console.WriteLine("2-place relation: " + relation2);
        System.Console.WriteLine("quantifier phrase: " + quantifierPhrase);
        System.Console.WriteLine("determiner: " + determiner);

        Expression bill = new Word(individual, "Bill");
        Expression heidi = new Word(individual, "Heidi");
        Expression red = new Word(predicate, "red");
        Expression the = new Word(determiner, "the");
        Expression round = new Word(predicate, "round");
        Expression helps = new Word(relation2, "helps");
        Expression every = new Word(quantifier, "every");

        Expression billIsRed = new Phrase(red, bill);
        Expression heidiHelpsBill = new Phrase(new Phrase(helps, bill), heidi);
        Expression everyRedRound = new Phrase(new Phrase(every, red), round);
        Expression theRoundRed = new Phrase(red, new Phrase(the, round));

        PrintInfoString(bill);
        PrintInfoString(heidi);
        PrintInfoString(red);
        PrintInfoString(every);
        PrintInfoString(the);

        PrintInfoString(billIsRed);
        PrintInfoString(heidiHelpsBill);
        PrintInfoString(everyRedRound);
        PrintInfoString(theRoundRed);
    }    
}
