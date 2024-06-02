using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class Card : MonoBehaviour
{
    private CardDetails _Details;
    public CardDetails Details {
        get {
            if (_Details == null) {
                Start();
            }
            return _Details;
        }
        set {
            _Details = value;
        }
    }
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public abstract void PlayCard();

    public static T CreateCardGameObject<T>(CardEffects parent) where T : Card {
        GameObject card_holder = new();
        card_holder.AddComponent<T>();
        card_holder.transform.parent = parent.transform;
        return card_holder.GetComponent<T>();
    }

    public static Card CreateCardGameObject(CardEffects parent, Type cardType) {
        GameObject card_holder = new GameObject();
        card_holder.AddComponent(cardType);
        card_holder.transform.parent = parent.transform;
        return (Card) card_holder.GetComponent(cardType);
    }

    public static Card CreateCardGameObject(CardEffects parent, Card card) {
        GameObject card_holder = new GameObject();
        card_holder.AddComponent(card.GetType());
        card_holder.transform.parent = parent.transform;
        return (Card) card_holder.GetComponent(card.GetType());
    }
}
