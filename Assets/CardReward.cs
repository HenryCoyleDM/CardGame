using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

[System.Serializable]
public class CardReward : MonoBehaviour
{
    public Card card;
    private CardEffects cardEffects;
    private Material material;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        List<CardDetails> all_card_types = ScriptableObject.CreateInstance<AllCardTypes>().TypesList;
        CardDetails card_details = all_card_types[new System.Random().Next(0, all_card_types.Count)];
        cardEffects = FindObjectOfType<CardEffects>();
        gameObject.AddComponent(card_details.CardClass);
        card = (Card) GetComponent(card_details.CardClass);
        material = GetComponent<MeshRenderer>().material;
        material.mainTexture = card_details.Image;
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e") && (player.transform.position - transform.position).sqrMagnitude < 4.0f) {
            Pickup();
        }
    }

    public void Pickup() {
        cardEffects.GainCardToDiscard(card);
        Destroy(gameObject);
    }
}
