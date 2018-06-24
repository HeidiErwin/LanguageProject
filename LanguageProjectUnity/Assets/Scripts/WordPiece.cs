using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * A script to be attached to any Word objects. I would call this class simply "Word" but that
 * is apparently already a thing in Unity!
 */
public class WordPiece : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    // wordName - the word in English (e.g. Key, Door, etc.)
    public string wordName;

    public Sprite currentSprite;
    public Sprite defaultSprite;
    public Sprite previousSprite;

    public bool isShowingPreview;
 
    //SUNDAY: get rid of the two lines below once Bill's SemanticType code is integrated
    public enum SemanticType { E , T , Arrow };
    public SemanticType semanticType;

    //the words on screen that could be combined with this Word
    List<WordPiece> wordsThatCouldCombineWithThisWord;

    // a List of the Semantic Types that this word can accept
    public List<SemanticType> canContain;

    public WordPiece(string wordName, Sprite defaultSprite, Sprite currentSprite, SemanticType semType) {
        this.wordName = wordName;
        this.defaultSprite = defaultSprite;
        this.currentSprite = currentSprite;
        this.semanticType = semType;
    }

    public void Update() {
        //there should only be one image in the images array; the loop below is just for safe measure
        Image[] images = gameObject.GetComponentsInChildren<Image>();
        foreach (Image image in images) {
            image.sprite = currentSprite;
        }
    }

    /**
    * Returns true if this Word can accept another Word, false otherwise
    */
    public bool CanAccept (WordPiece otherWord) {
        //TODO: set up words to use commented-out line below:
        // return (this.canContain.Contains(otherWord.semanticType));
        return true;
    }

    /**
     * Returns the new sprite that this Word should display when COMBINED WITH
     * the WordPiece wordToCombine
     */
    public Sprite DetermineUpdatedSprite (WordPiece wordToCombine) {
        Sprite fXSprite = Resources.Load<Sprite>("PlaceholderSprites/fXImage");

        //TODO: given wordToCombine, return appropriate Sprite
        return fXSprite;
    }

    /**
    * Returns the sprite that this Word should display when the user is HOLDING
    * the WordPiece wordToCombine
    */
    public Sprite DeterminePreviewSprite(WordPiece wordToCombine) {
        Sprite fOpenSprite = Resources.Load<Sprite>("PlaceholderSprites/fOpenSlot");

        //TODO: given wordToCombine, return appropriate Sprite
        return fOpenSprite;
    }

    /**
    * Triggered anytime an object is released on top of this Word. 
    * The image of this Word is updated appropriately.
    */
    public void OnDrop(PointerEventData eventData) {
        WordPiece droppedWord = eventData.pointerDrag.GetComponent<WordPiece>();
        Debug.Log(eventData.pointerDrag.name + " was dropped on " + gameObject.name + " :)");

        if (CanAccept(droppedWord)) {
            //TODO: the dropped word "returns" to the keyboard and the current word changes to the new sprite
            //TODO: make sure you can only combine words in the workspace---keyboard should stay constant

            this.currentSprite = DetermineUpdatedSprite(droppedWord);
            this.isShowingPreview = false;
        }
    }

    /**
     * When this Word is picked up, anything that it can be combined with will adopt
     * the appropriate preview sprite.
     * e.g. if this Word is an E, a Word of type E->T will turn from a solid shape into a 
     * shape that clearly shows that this E can be inserted into the shape.
     */
    public void OnBeginDrag(PointerEventData eventData) {
        WordPiece[] wordsOnScreen = FindObjectsOfType<WordPiece>();
        wordsThatCouldCombineWithThisWord = new List<WordPiece>();
        foreach (WordPiece wp in wordsOnScreen) {
            if (this.CanAccept(wp) && !this.Equals(wp)) {
                wordsThatCouldCombineWithThisWord.Add(wp);
            }
        }
        foreach (WordPiece wp in wordsThatCouldCombineWithThisWord) {
            Sprite previewSprite = DeterminePreviewSprite(wp);
            wp.previousSprite = wp.currentSprite;
            wp.currentSprite = previewSprite;
            wp.isShowingPreview = true;
        }
    }

    public void OnDrag(PointerEventData eventData) {
    }

    /**
    * When dragging this WordPiece ends, change the preview sprites on screen back to 
    * their original sprites.
    */
    public void OnEndDrag(PointerEventData eventData) {
        foreach (WordPiece wp in wordsThatCouldCombineWithThisWord) {
            if (wp.isShowingPreview) {
                wp.currentSprite = wp.previousSprite;
            }
        }
        wordsThatCouldCombineWithThisWord.Clear();
    }
}
