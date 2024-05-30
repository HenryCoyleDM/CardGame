using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Scripting;

[System.Serializable]
public class CardEffects : MonoBehaviour
{

    public LinkedList<Card> Deck = new();
    public LinkedList<Card> DiscardPile = new();
    public List<Card> Hand = new();
    private HandDisplay hand_display;
    
    // Start is called before the first frame update
    void Start()
    {
        DiscardPile.AddFirst(Card.CreateCardGameObject<Jump>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Jump>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Jump>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Jump>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Jump>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Dash>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Dash>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Dash>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Dash>());
        DiscardPile.AddFirst(Card.CreateCardGameObject<Stab>());
        ShuffleDiscardAndMakeNewDrawPile();
        DrawNewHand(5);
        hand_display = FindObjectOfType<HandDisplay>();
        if (hand_display == null) {
            throw new Exception("Can't find hand display");
        }
        hand_display.UpdateDeck(this);
        hand_display.UpdateHand(this);
    }

    private bool draw_new_hand_button_pressed = false;
    
    // Update is called once per frame
    void Update()
    {
        int input = GetCardSelectionKeyDown();
        if (input >= 0 && input < Hand.Count) {
            Hand[input].PlayCard();
            DiscardPile.AddLast(Hand[input]);
            Hand.RemoveAt(input);
            hand_display.UpdateHand(this);
        }
        if (Input.GetAxis("Fire2") > 0) {
            if (!draw_new_hand_button_pressed) {
                DiscardHand();
                DrawNewHand(5);
                hand_display.UpdateHand(this);
            }
            draw_new_hand_button_pressed = true;
        } else {
            draw_new_hand_button_pressed = false;
        }
    }

    private int GetCardSelectionKeyDown() {
        if (Input.GetKeyDown("1")) {
            return 0;
        } else if (Input.GetKeyDown("2")) {
            return 1;
        } else if (Input.GetKeyDown("3")) {
            return 2;
        } else if (Input.GetKeyDown("4")) {
            return 3;
        } else if (Input.GetKeyDown("5")) {
            return 4;
        } else if (Input.GetKeyDown("6")) {
            return 5;
        } else if (Input.GetKeyDown("7")) {
            return 6;
        } else if (Input.GetKeyDown("8")) {
            return 7;
        } else if (Input.GetKeyDown("9")) {
            return 8;
        } else if (Input.GetKeyDown("0")) {
            return 9;
        } else {
            return -1;
        }
    }

    private readonly System.Random RNG = new();
    
    public void ShuffleDiscardAndMakeNewDrawPile() {
        // Debug.Log("Reshuffling");
        int n = DiscardPile.Count;
        while (n > 0) {
            int random = RNG.Next(n);
            LinkedListNode<Card> node;
            if (random < n / 2) {
                node = DiscardPile.First;
                while (random > 0) {
                    node = node.Next;
                    random--;
                }
            } else {
                random = n - 1 - random;
                node = DiscardPile.Last;
                while (random > 0) {
                    node = node.Previous;
                    random--;
                }
            }
            Card next_card = node.Value;
            DiscardPile.Remove(node);
            Deck.AddFirst(next_card);
            n--;
        }
    }

    public void DiscardHand() {
        foreach (Card card in Hand) {
            DiscardPile.AddLast(card);
        }
        Hand.Clear();
    }

    public void DrawNewHand(int num_cards) {
        DrawCards(num_cards);
    }

    public bool DrawCards(int num_cards) {
        for (int i=0; i<num_cards; i++) {
            if (Deck.Count == 0) {
                if (DiscardPile.Count == 0) {
                    return false;
                } else {
                    ShuffleDiscardAndMakeNewDrawPile();
                }
            }
            Hand.Add(Deck.First.Value);
            Deck.RemoveFirst();
        }
        return true;
    }

    public int CardCount() {
        return Deck.Count + Hand.Count + DiscardPile.Count;
    }

    public Card[] GetAllCards() {
        Card[] result = new Card[CardCount()];
        Deck.CopyTo(result, 0);
        DiscardPile.CopyTo(result, Deck.Count);
        Hand.CopyTo(result, Deck.Count + DiscardPile.Count);
        return result;
    }

    public void GainCardToDiscard(Card card) {
        DiscardPile.AddLast(card);
        hand_display.UpdateDeck(this);
    }
}
