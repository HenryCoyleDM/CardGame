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
    private bool is_initialized;

    // Start is called before the first frame update
    void Start()
    {
        if (!is_initialized) {
            SetCardDetails(GetRandomCardType());
        }
        cardEffects = FindObjectOfType<CardEffects>();
        material = GetComponent<MeshRenderer>().material;
        player = FindObjectOfType<Player>();
        material.mainTexture = card.Details.Image;
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

    public CardDetails GetRandomCardType() {
        List<CardDetails> common_card_types = ScriptableObject.CreateInstance<AllCardTypes>().CommonCards;
        return common_card_types[new System.Random().Next(0, common_card_types.Count)];
    }

    public void SetCard(Card new_card) {
        SetCardDetails(new_card.Details);
    }

    public void SetCardDetails(CardDetails new_details) {
        gameObject.AddComponent(new_details.CardClass);
        card = (Card) GetComponent(new_details.CardClass);
        if (material != null) {
            material.mainTexture = card.Details.Image;
        }
        is_initialized = true;
    }
}
