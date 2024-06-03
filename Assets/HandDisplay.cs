using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;

[System.Serializable]
public class HandDisplay : MonoBehaviour
{
    // private CardEffects DeckHandler;
    // maps the cards from DeckHandler to GameObjects used to display cards
    public readonly Dictionary<Card, CardboardDrawer> Cardboards = new();
    public CardboardDrawer CardboardPrefab;
    public float CardSpacing;
    
    // Start is called before the first frame update
    void Start()
    {
        // DeckHandler = FindObjectOfType<CardEffects>();
        // if (DeckHandler == null) {
        //     Debug.Log("Can't find deck handler");
        //     throw new System.Exception("Can't find deck handler");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHand(CardEffects deckHandler) {
        // Debug.Log("Updating hand");
        foreach (KeyValuePair<Card, CardboardDrawer> entry in Cardboards) {
            Card card = entry.Key;
            CardboardDrawer cardboard = entry.Value;
            int hand_index = deckHandler.Hand.IndexOf(card);
            if (hand_index >= 0) {
                cardboard.gameObject.SetActive(true);
                cardboard.transform.localPosition = GetCoordinatesOfCardIndex(hand_index, deckHandler.Hand.Count);
                if (!cardboard.WasInHandLastFrame) {
                    cardboard.AnimateDraw();
                }
                cardboard.SetSelectionKey(hand_index + 1);
                cardboard.WasInHandLastFrame = true;
            } else {
                if (cardboard.WasInHandLastFrame) {
                    cardboard.gameObject.SetActive(true);
                    cardboard.AnimatePlayAndDiscard();
                }
                cardboard.WasInHandLastFrame = false;
            }
        }
    }

    private Vector3 GetCoordinatesOfCardIndex(int i, int num_cards) {
        float x_coord = (i - num_cards * 0.5f + 0.5f) * CardSpacing;
        return new Vector3(x_coord, 0.0f, 0.0f);
    }

    public void UpdateDeck(CardEffects deckHandler) {
        // Debug.Log("Updating hand display deck");
        Card[] deck = deckHandler.GetAllCards();
        Dictionary<Card, bool> CardsArePresentInNewDeck = new();
        foreach (Card card in Cardboards.Keys) {
            CardsArePresentInNewDeck[card] = false;
        }
        // add new cards to the dictionary if they don't already exist
        foreach (Card card in deck) {
            if (!Cardboards.ContainsKey(card)) {
                CardboardDrawer cardboard = CreateCardboard(card);
                Cardboards[card] = cardboard;
            } else {
                CardsArePresentInNewDeck[card] = true;
            }
        }
        // removes and destroys gameobjects if those cards are no longer in the deck
        foreach (KeyValuePair<Card, bool> is_card_present in CardsArePresentInNewDeck.AsEnumerable()) {
            if (!is_card_present.Value) {
                CardboardDrawer old_cardboard = Cardboards[is_card_present.Key];
                Destroy(old_cardboard.gameObject);
                Cardboards.Remove(is_card_present.Key);
            }
        }
    }

    private CardboardDrawer CreateCardboard(Card card) {
        CardboardDrawer result = Instantiate(CardboardPrefab);
        result.SetImage(card);
        result.transform.SetParent(transform);
        result.gameObject.SetActive(false);
        return result;
    }
}
