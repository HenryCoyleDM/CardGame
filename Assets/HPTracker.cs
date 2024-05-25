using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPTracker : MonoBehaviour
{
    private Player player;
    private TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "HP: " + player.HP;
    }
}
