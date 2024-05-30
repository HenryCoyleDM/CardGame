using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardboardDrawer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetImage(Card card) {
        Image image_component = GetComponent<Image>();
        image_component.sprite = Sprite.Create(card.Details.Image, new Rect(0, 0, 620, 880), new Vector2());
    }

    public void SetSelectionKey(int number) {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = number.ToString();
    }
}
