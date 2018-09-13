using System.Linq;
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

        Debug.Log("Syntactic Equality:");
        Debug.Log(bill.Equals(new Word(SemanticType.INDIVIDUAL, "Bill")));
        Debug.Log(!bill.Equals(heidi));
        Debug.Log(billIsRed.Equals(billIsRed));
        Debug.Log(!billIsRed.Equals(billHelpsHeidi));

        Debug.Log("Model:");
        Model model = new PrefixModel();
        Debug.Log(model.Add(billIsRed));
        Debug.Log(!model.Add(billIsRed));
        Debug.Log(model.Add(billGivesHeidiTheApple));
        Debug.Log(model.Add(theRoundThingIsRed));

        Debug.Log(model.Contains(billIsRed));
        Debug.Log(!model.Contains(billHelpsHeidi));

        Debug.Log(!model.Remove(billHelpsHeidi));
        Debug.Log(model.Remove(billIsRed));

        Debug.Log(!model.Contains(billIsRed));

        Debug.Log(model.Add(verum));
        Debug.Log(model.Contains(verum));
        Debug.Log(model.Remove(verum));
        Debug.Log(!model.Contains(verum));

        Debug.Log(model.Add(helps));
        Debug.Log(model.Contains(helps));
        Debug.Log(model.Remove(helps));
        Debug.Log(!model.Contains(helps));

        Debug.Log(model.Add(red));
        Debug.Log(model.Contains(red));
        Debug.Log(model.Remove(red));
        Debug.Log(!model.Contains(red));

        Debug.Log("Pattern Matching:");
        Debug.Log(verum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        Debug.Log(!falsum.Matches(new Word(SemanticType.TRUTH_VALUE, "T")));
        IPattern sentenceVariable = new MetaVariable(SemanticType.TRUTH_VALUE, 0);
        Debug.Log(sentenceVariable.Matches(verum));
        Debug.Log(sentenceVariable.Matches(falsum));
        Debug.Log(sentenceVariable.Matches(billHelpsHeidi));
        Debug.Log(!sentenceVariable.Matches(billGives));
        Debug.Log(!sentenceVariable.Matches(bill));

        IPattern reflexivePattern = new ExpressionPattern(helps,
            new IPattern[]{
                new MetaVariable(SemanticType.INDIVIDUAL, 0),
                new MetaVariable(SemanticType.INDIVIDUAL, 0)
            });

        Debug.Log(helps.Equals(helps));
        Debug.Log(reflexivePattern.Matches(new Phrase(billHelps, bill)));
        Debug.Log(!reflexivePattern.Matches(billHelpsHeidi));

        Debug.Log("Inference:");
        SubsententialRule appleEntailsFruit = new SubsententialRule(apple, fruit);
        Debug.Log(appleEntailsFruit.InferUpward(red) == null);
        Debug.Log(appleEntailsFruit.InferUpward(fruit) == null);
        Debug.Log(appleEntailsFruit.InferUpward(apple).Equals(fruit));

        Debug.Log(appleEntailsFruit.InferDownward(red) == null);
        Debug.Log(appleEntailsFruit.InferDownward(fruit).Equals(apple));
        Debug.Log(appleEntailsFruit.InferDownward(apple) == null);

        Debug.Log("Model Proves");
        Model im = new PrefixModel();
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

        // rules
        SubsententialRule aImpliesB = new SubsententialRule(a, b);
        SubsententialRule dni = new SubsententialRule(xt0,
            new ExpressionPattern(not, new IPattern[]{new ExpressionPattern(not, new IPattern[]{xt0})}));
        SubsententialRule tRule = new SubsententialRule(xt0, new ExpressionPattern(Expression.TRUE, xt0));
        // SubsententialRule tRule2 = new SubsententialRule(new ExpressionPattern(Expression.TRUE, xt0), xt0);
        
        SubsententialRule ntRule = new SubsententialRule(new ExpressionPattern(Expression.NOT, new IPattern[]{xt0}),
           new ExpressionPattern(Expression.NOT, new IPattern[]{new ExpressionPattern(Expression.TRUE, new IPattern[]{xt0})}));
        
        SubsententialRule huskyDog = new SubsententialRule(husky, dog);
        SubsententialRule dogAnimal = new SubsententialRule(dog, animal);

        // im.Add(aImpliesB);
        // im.Add(dni);
        // im.Add(tRule);
        // im.Add(tRule2);
        im.Add(ntRule);
        im.Add(huskyDog);
        im.Add(dogAnimal);

        im.Add(EvaluationRule.NOT);
        im.Add(EvaluationRule.EVERY);
        im.Add(EvaluationRule.DEFAULT_PREDICATE);

        // sentences
        im.Add(a);
        im.Add(mitkaIsAHusky);
        im.Add(rockyIsNotAnAnimal);
        im.Add(everyDogIsADog);

        Expression notB = new Phrase(not, new Word(SemanticType.TRUTH_VALUE, "B"));
        Expression notNotB = new Phrase(not, notB);
        Expression trueNotB = new Phrase(Expression.TRUE, notB);
        Expression notTrueNotB = new Phrase(not, trueNotB);

        // Debug.Log(!im.Proves(notB));
        // Debug.Log(im.Proves(notNotB));
        // Debug.Log(!im.Proves(trueNotB));
        // Debug.Log(im.Proves(notTrueNotB));

        Debug.Log(im.Proves(mitkaIsAHusky));
        Debug.Log(im.Proves(mitkaIsADog));
        Debug.Log(im.Proves(mitkaIsAnAnimal));
        Debug.Log(im.Proves(rockyIsNotAnAnimal));
        Debug.Log(im.Proves(rockyIsNotADog));
        Debug.Log(im.Proves(rockyIsNotAHusky));
        Debug.Log(im.Proves(everyDogIsADog));
        Debug.Log(im.Proves(everyHuskyIsADog));
        Debug.Log(im.Proves(everyHuskyIsAnAnimal));
    }
}
