using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

[System.Serializable]
public class CardEffects : MonoBehaviour
{

    public LinkedList<Card> DrawPile = new();
    public LinkedList<Card> DiscardPile = new();
    public List<Card> Hand = new();
    
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
        DiscardPile.AddFirst(Card.CreateCardGameObject<Dash>());
        ShuffleDiscardAndMakeNewDrawPile();
        DrawNewHand(5);
    }

    // Update is called once per frame
    void Update()
    {
        int input = GetCardSelectionKeyDown();
        if (input >= 0) {
            Hand[input].PlayCard();
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
        Debug.Log("Reshuffling");
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
            DrawPile.AddFirst(next_card);
            n--;
        }
        Debug.Log("Deck contains " + DrawPile.Count + " cards and Discard contains " + DiscardPile.Count + " cards");
    }

    public void DrawNewHand(int num_cards) {
        DrawCards(num_cards);
    }

    public bool DrawCards(int num_cards) {
        for (int i=0; i<num_cards; i++) {
            if (DrawPile.Count == 0) {
                if (DiscardPile.Count == 0) {
                    return false;
                } else {
                    ShuffleDiscardAndMakeNewDrawPile();
                }
            }
            Hand.Add(DrawPile.First.Value);
            DrawPile.RemoveFirst();
        }
        return true;
    }
}
