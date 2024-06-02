using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawPileUICounter : MonoBehaviour
{
    private TextMeshProUGUI text;
    private CardEffects cardEffects;
    
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        cardEffects = FindObjectOfType<CardEffects>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = cardEffects.Deck.Count.ToString();
    }
}
