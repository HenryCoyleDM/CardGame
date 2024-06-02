using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscardPileUICounter : MonoBehaviour
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
        text.text = cardEffects.DiscardPile.Count.ToString();
    }
}
