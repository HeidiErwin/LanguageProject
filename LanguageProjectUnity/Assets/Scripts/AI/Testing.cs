using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {
    static void PrintInfoString(Expression e) {
        Debug.Log(e + ": " + e.type);
    }
    
    void Start() {
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

        // PrintInfoString(billHelps);
        // PrintInfoString(helpsHeidi);

        Expression billGives = new Phrase(gives, bill, 1);
        Expression billGivesHeidi = new Phrase(billGives, heidi);

        // PrintInfoString(billGives);
        // PrintInfoString(billGivesHeidi);

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

        // PrintInfoString(billIsRed);
        // PrintInfoString(billHelpsHeidi);
        // PrintInfoString(theRoundThingIsRed);
        // PrintInfoString(billGivesHeidiTheApple);

        // Debug.Log("Syntactic Equality:");
        // Debug.Log(bill.Equals(new Word(SemanticType.INDIVIDUAL, "Bill")));
        // Debug.Log(!bill.Equals(heidi));
        // Debug.Log(billIsRed.Equals(billIsRed));
        // Debug.Log(!billIsRed.Equals(billHelpsHeidi));

        // Debug.Log("Model:");
        // Model model = new SimpleModel();
        // Debug.Log(model.Add(billIsRed));
        // Debug.Log(!model.Add(billIsRed));
        // Debug.Log(model.Add(billGivesHeidiTheApple));
        // Debug.Log(model.Add(theRoundThingIsRed));

        // Debug.Log(model.Contains(billIsRed));
        // Debug.Log(!model.Contains(billHelpsHeidi));

        // Debug.Log(!model.Remove(billHelpsHeidi));
        // Debug.Log(model.Remove(billIsRed));

        // Debug.Log(!model.Contains(billIsRed));

        // Debug.Log(model.Add(verum));
        // Debug.Log(model.Contains(verum));
        // Debug.Log(model.Remove(verum));
        // Debug.Log(!model.Contains(verum));

        // Debug.Log(model.Add(helps));
        // Debug.Log(model.Contains(helps));
        // Debug.Log(model.Remove(helps));
        // Debug.Log(!model.Contains(helps));

        // Debug.Log(model.Add(red));
        // Debug.Log(model.Contains(red));
        // Debug.Log(model.Remove(red));
        // Debug.Log(!model.Contains(red));

        // Debug.Log("Pattern Matching:");
        // Debug.Log(verum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        // Debug.Log(!falsum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        // IPattern sentenceVariable = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        // Debug.Log(sentenceVariable.Matches(verum));
        // Debug.Log(sentenceVariable.Matches(falsum));
        // Debug.Log(sentenceVariable.Matches(billHelpsHeidi));
        // Debug.Log(!sentenceVariable.Matches(billGives));
        // Debug.Log(!sentenceVariable.Matches(bill));
        
        MetaVariable xi00 = new MetaVariable(SemanticType.INDIVIDUAL, 0);

        IPattern reflexivePattern = new ExpressionPattern(helps, xi00, xi00);

        // Debug.Log(reflexivePattern.Matches(new Phrase(billHelps, bill)));
        // Debug.Log(!reflexivePattern.Matches(billHelpsHeidi));

        // Debug.Log("Inference:");
        // SubstitutionRule appleEntailsFruit = new SubstitutionRule(apple, fruit);
        // Debug.Log(appleEntailsFruit.InferUpward(red) == null);
        // Debug.Log(appleEntailsFruit.InferUpward(fruit) == null);
        // Debug.Log(appleEntailsFruit.InferUpward(apple).Equals(fruit));

        // Debug.Log(appleEntailsFruit.InferDownward(red) == null);
        // Debug.Log(appleEntailsFruit.InferDownward(fruit).Equals(apple));
        // Debug.Log(appleEntailsFruit.InferDownward(apple) == null);

        // Debug.Log("Model Proves");
        Model im = new SimpleModel();
        Expression not = new Word(SemanticType.TRUTH_FUNCTION_1, "not");
        Expression every = new Word(SemanticType.DETERMINER, "every");
        Expression a = new Word(SemanticType.TRUTH_VALUE, "A");
        Expression b = new Word(SemanticType.TRUTH_VALUE, "B");

        Expression animal = new Word(SemanticType.PREDICATE, "animal");
        Expression dog = new Word(SemanticType.PREDICATE, "dog");
        Expression husky = new Word(SemanticType.PREDICATE, "husky");

        Expression mitka = new Word(SemanticType.INDIVIDUAL, "Mitka");
        Expression rocky = new Word(SemanticType.INDIVIDUAL, "Rocky");

        Expression mitkaIsAHusky = new Phrase(husky, mitka);
        Expression mitkaIsADog = new Phrase(dog, mitka);
        Expression mitkaIsAnAnimal = new Phrase(animal, mitka);

        Expression rockyIsNotAnAnimal = new Phrase(not, new Phrase(animal, rocky));
        Expression rockyIsNotADog = new Phrase(not, new Phrase(dog, rocky));
        Expression rockyIsNotAHusky = new Phrase(not, new Phrase(husky, rocky));

        Expression everyDogIsADog = new Phrase(dog, new Phrase(every, dog));
        Expression everyHuskyIsADog = new Phrase(dog, new Phrase(every, husky));
        Expression everyHuskyIsAnAnimal = new Phrase(animal, new Phrase(every, husky));

        MetaVariable xt0 = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        MetaVariable xt1 = new MetaVariable(SemanticType.TRUTH_VALUE, 1);

        // rules
        SubstitutionRule aImpliesB = new SubstitutionRule(a, b);
        SubstitutionRule dni = new SubstitutionRule(xt0,
            new ExpressionPattern(not, new ExpressionPattern(not, xt0)), EntailmentContext.Downward);
        SubstitutionRule tRule = new SubstitutionRule(xt0, new ExpressionPattern(Expression.TRUE, xt0), EntailmentContext.Downward);
        SubstitutionRule tRule2 = new SubstitutionRule(new ExpressionPattern(Expression.TRUE, xt0), xt0, EntailmentContext.Upward);
        
        // SubstitutionRule ntRule = new SubstitutionRule(new ExpressionPattern(Expression.NOT, new IPattern[]{xt0}),
        //    new ExpressionPattern(Expression.NOT, new IPattern[]{new ExpressionPattern(Expression.TRUE, new IPattern[]{xt0})}));
        
        SubstitutionRule huskyDog = new SubstitutionRule(husky, dog);
        SubstitutionRule dogAnimal = new SubstitutionRule(dog, animal);

        // Inference Rules
        InferenceRule andIntroduction = new InferenceRule(
            new IPattern[]{xt0, xt1},
            new IPattern[]{new ExpressionPattern(Expression.AND, xt0, xt1)},
            EntailmentContext.Downward);

        InferenceRule orIntroduction1 = new InferenceRule(
            new IPattern[]{xt0},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward);

        InferenceRule orIntroduction2 = new InferenceRule(
            new IPattern[]{xt1},
            new IPattern[]{new ExpressionPattern(Expression.OR, xt0, xt1)},
            EntailmentContext.Downward);

        // TODO "or" elmination

        im.Add(aImpliesB);
        im.Add(dni);
        im.Add(tRule);
        im.Add(tRule2);

        im.Add(huskyDog);
        im.Add(dogAnimal);

        im.Add(EvaluationRule.NOT);
        im.Add(EvaluationRule.EVERY);
        im.Add(EvaluationRule.DEFAULT_PREDICATE);

        im.Add(andIntroduction);
        im.Add(orIntroduction1);
        im.Add(orIntroduction2);

        // sentences
        im.Add(a);
        im.Add(mitkaIsAHusky);
        im.Add(rockyIsNotAnAnimal);
        im.Add(everyDogIsADog);

        Expression notB = new Phrase(not, new Word(SemanticType.TRUTH_VALUE, "B"));
        Expression notNotB = new Phrase(not, notB);
        Expression trueNotB = new Phrase(Expression.TRUE, notB);
        Expression notTrueNotB = new Phrase(not, trueNotB);

        PrintProves(im, notB, false);
        PrintProves(im, notNotB, true);
        PrintProves(im, trueNotB, false);
        PrintProves(im, notTrueNotB, true);

        PrintProves(im, mitkaIsAHusky, true);
        PrintProves(im, mitkaIsADog, true);
        PrintProves(im, mitkaIsAnAnimal, true);
        PrintProves(im, rockyIsNotAnAnimal, true);
        PrintProves(im, rockyIsNotADog, true);
        PrintProves(im, rockyIsNotAHusky, true);
        PrintProves(im, everyDogIsADog, true);
        PrintProves(im, everyHuskyIsADog, true);
        PrintProves(im, everyHuskyIsAnAnimal, true);

        PrintProves(im, new Phrase(Expression.AND, mitkaIsAHusky, rockyIsNotAnAnimal), true);
        PrintProves(im, new Phrase(Expression.AND, notB, rockyIsNotAnAnimal), false);
        PrintProves(im, new Phrase(Expression.AND, notB, trueNotB), false);
        PrintProves(im, new Phrase(Expression.OR, notB, rockyIsNotAnAnimal), true);
        PrintProves(im, new Phrase(Expression.OR, mitkaIsAHusky, notB), true);
        PrintProves(im, new Phrase(Expression.OR, notB, trueNotB), false);

        // testing Find()
        Model fm = new SimpleModel();
        fm.Add(new Phrase(Expression.BLUE, new Parameter(SemanticType.INDIVIDUAL, 0)));
        fm.Add(new Phrase(Expression.BLUE, new Parameter(SemanticType.INDIVIDUAL, 1)));
        fm.Add(new Phrase(Expression.BLUE, new Parameter(SemanticType.INDIVIDUAL, 2)));
        fm.Add(new Phrase(Expression.BLUE, new Parameter(SemanticType.INDIVIDUAL, 3)));

        fm.Add(new Phrase(Expression.RED, new Parameter(SemanticType.INDIVIDUAL, 2)));
        fm.Add(new Phrase(Expression.RED, new Parameter(SemanticType.INDIVIDUAL, 3)));
        fm.Add(new Phrase(Expression.RED, new Parameter(SemanticType.INDIVIDUAL, 5)));
        fm.Add(new Phrase(Expression.RED, new Parameter(SemanticType.INDIVIDUAL, 6)));

        fm.Add(new Phrase(Expression.GREEN, new Parameter(SemanticType.INDIVIDUAL, 7)));
        fm.Add(new Phrase(Expression.GREEN, new Parameter(SemanticType.INDIVIDUAL, 8)));

        fm.Add(new Phrase(Expression.YELLOW, new Parameter(SemanticType.INDIVIDUAL, 9)));
        fm.Add(new Phrase(Expression.YELLOW, new Parameter(SemanticType.INDIVIDUAL, 10)));

        ExpressionPattern xIsRed = new ExpressionPattern(Expression.RED, new MetaVariable(SemanticType.INDIVIDUAL, 0));
        ExpressionPattern xIsBlue = new ExpressionPattern(Expression.BLUE, new MetaVariable(SemanticType.INDIVIDUAL, 0));

        Debug.Log(ContentString(fm.Find(xIsRed)));
        Debug.Log(ContentString(fm.Find(xIsRed, xIsBlue)));

        Expression contains = new Word(SemanticType.RELATION_2, "contains");

        InferenceRule transitivityForContains = new InferenceRule(
            new IPattern[]{
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1)),
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 1),
                    new MetaVariable(SemanticType.INDIVIDUAL, 2))
            },
            new IPattern[]{
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new MetaVariable(SemanticType.INDIVIDUAL, 2))
            });

        InferenceRule exclusivityForContains = new InferenceRule(
            new IPattern[]{
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1)),
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 2),
                    new MetaVariable(SemanticType.INDIVIDUAL, 1))
            },
            new IPattern[]{
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 0),
                    new MetaVariable(SemanticType.INDIVIDUAL, 2)),
                new ExpressionPattern(contains,
                    new MetaVariable(SemanticType.INDIVIDUAL, 2),
                    new MetaVariable(SemanticType.INDIVIDUAL, 0))
            });

        fm.Add(transitivityForContains);
        fm.Add(exclusivityForContains);

        Expression elmira  = new Word(SemanticType.INDIVIDUAL, "Elmira");
        Expression newYork = new Word(SemanticType.INDIVIDUAL, "New York");
        Expression america = new Word(SemanticType.INDIVIDUAL, "America");

        fm.Add(elmira);
        fm.Add(newYork);
        fm.Add(america);

        Expression americaContainsNewYork = new Phrase(contains, america, newYork);
        Expression newYorkContainsElmira = new Phrase(contains, newYork, elmira);

        fm.Add(americaContainsNewYork);
        fm.Add(newYorkContainsElmira);

        Expression paris  = new Word(SemanticType.INDIVIDUAL, "Paris");
        Expression france = new Word(SemanticType.INDIVIDUAL, "France");
        Expression europe = new Word(SemanticType.INDIVIDUAL, "Europe");

        Expression europeContainsParis = new Phrase(contains, europe, paris);
        Expression franceContainsParis = new Phrase(contains, france, paris);
        Expression franceDoesntContainEurope = new Phrase(Expression.NOT, new Phrase(contains, france, europe));

        fm.Add(europeContainsParis);
        fm.Add(franceContainsParis);
        fm.Add(franceDoesntContainEurope);

        PrintProves(fm, new Phrase(contains, america, elmira), true);
        PrintProves(fm, new Phrase(contains, europe,  france), true);
    }

    private void PrintProves(Model m, Expression e, bool proves) {
        if (proves) {
            if (m.Proves(e)) {
                Debug.Log("SUCCESS: model proves " + e);
            } else {
                Debug.Log("FAILURE: model doesn't prove " + e);
            }
        } else {
            if (!m.Proves(e)) {
                Debug.Log("SUCCESS: model doesn't prove " + e);
            } else {
                Debug.Log("FAILURE: model proves " + e);
            }
        }

    }

    private String ContentString(Dictionary<MetaVariable, Expression> bindings) {
        StringBuilder s = new StringBuilder();
        s.Append("{");
        foreach (MetaVariable x in bindings.Keys) {
            s.Append(x);
            s.Append(" |-> ");
            s.Append(bindings[x]);
            s.Append(", ");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append("}");

        return s.ToString();
    }

    private String ContentString(HashSet<Dictionary<MetaVariable, Expression>> bindingSet) {
        StringBuilder s = new StringBuilder();
        s.Append("{\n");
        if (bindingSet == null) {
            return "NULL";
        }
        foreach (Dictionary<MetaVariable, Expression> bindings in bindingSet) {
            s.Append("\t");
            s.Append(ContentString(bindings));
            s.Append(",\n");
        }

        if (s.Length > 1) {
            s.Remove(s.Length - 2, 2);
        }

        s.Append("\n}");

        return s.ToString();
    }
}
