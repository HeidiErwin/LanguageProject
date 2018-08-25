using System.Linq;
using System.Collections.Generic;

public class Testing {
    static void PrintInfoString(Expression e) {
        System.Console.WriteLine(e + ": " + e.type);
    }

    static void Main() {
        Expression bill = new Word(SemanticType.INDIVIDUAL, "Bill");
        Expression heidi = new Word(SemanticType.INDIVIDUAL, "Heidi");
        Expression apple = new Word(SemanticType.PREDICATE, "apple");
        Expression fruit = new Word(SemanticType.PREDICATE, "fruit");
        Expression red = new Word(SemanticType.PREDICATE, "red");
        // Expression colored = new Word(SemanticType.PREDICATE, "colored");
        Expression the = new Word(SemanticType.DETERMINER, "the");
        Expression round = new Word(SemanticType.PREDICATE, "round");
        Expression helps = new Word(SemanticType.RELATION_2, "helps");
        Expression gives = new Word(SemanticType.RELATION_3, "gives");

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
        Expression billGivesHeidiTheApple = new Phrase(billGivesHeidi, new Phrase(the, apple));

        Expression verum = new Word(SemanticType.TRUTH_VALUE, "T");
        Expression falsum = new Word(SemanticType.TRUTH_VALUE, "F");

        // PrintInfoString(bill);
        // PrintInfoString(heidi);
        // PrintInfoString(red);
        // PrintInfoString(every);
        // PrintInfoString(the);

        PrintInfoString(billIsRed);
        PrintInfoString(billHelpsHeidi);
        PrintInfoString(theRoundThingIsRed);
        PrintInfoString(billGivesHeidiTheApple);

        System.Console.WriteLine("Syntactic Equality:");
        System.Console.WriteLine(bill.Equals(new Word(SemanticType.INDIVIDUAL, "Bill")));
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

        System.Console.WriteLine("Pattern Matching:");
        System.Console.WriteLine(verum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        System.Console.WriteLine(!falsum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        IPattern sentenceVariable = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        System.Console.WriteLine(sentenceVariable.Matches(verum));
        System.Console.WriteLine(sentenceVariable.Matches(falsum));
        System.Console.WriteLine(sentenceVariable.Matches(billHelpsHeidi));
        System.Console.WriteLine(!sentenceVariable.Matches(billGives));
        System.Console.WriteLine(!sentenceVariable.Matches(bill));

        IPattern reflexivePattern = new ExpressionPattern(helps, new IPattern[]{new MetaVariable(SemanticType.INDIVIDUAL, 0), new MetaVariable(SemanticType.INDIVIDUAL, 0)});

        System.Console.WriteLine(helps.Equals(helps));
        System.Console.WriteLine(reflexivePattern.Matches(new Phrase(billHelps, bill)));
        System.Console.WriteLine(!reflexivePattern.Matches(billHelpsHeidi));

        System.Console.WriteLine("Inference:");
        SubsententialRule appleEntailsFruit = new SubsententialRule(apple, fruit);
        System.Console.WriteLine(appleEntailsFruit.InferUpward(red) == null);
        System.Console.WriteLine(appleEntailsFruit.InferUpward(fruit) == null);
        System.Console.WriteLine(appleEntailsFruit.InferUpward(apple));
    }
}
