using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* A class that holds the WordPiece objects that appear in the keyboard of the system
*/
public class Keyboard : MonoBehaviour {

    private WordPiece key;
    private WordPiece door;
    private WordPiece make;
    private WordPiece get;

	void Start () {
        key = new WordPiece("key", GetSpriteWithName("key"), GetSpriteWithName("key"), WordPiece.SemanticType.E); //when Bill's SemanticTypes are integrated, change 3rd parameter
        door = new WordPiece("door", GetSpriteWithName("door"), GetSpriteWithName("door"), WordPiece.SemanticType.E); 
        make = new WordPiece("make", GetSpriteWithName("make"), GetSpriteWithName("make"), WordPiece.SemanticType.Arrow);
        get = new WordPiece("get", GetSpriteWithName("get"), GetSpriteWithName("get"), WordPiece.SemanticType.Arrow);

    }

    public Sprite GetSpriteWithName(string name) {
        return Resources.Load<Sprite>("PlaceholderSprites/" + name);
    }
}
