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

    public static T CreateCardGameObject<T>() where T : Card {
        GameObject card_holder = new();
        card_holder.AddComponent<T>();
        return card_holder.GetComponent<T>();
    }
}
