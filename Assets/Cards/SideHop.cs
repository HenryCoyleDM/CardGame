using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SideHop : Card
{
    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/SideHop.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard()
    {
        Player player = FindObjectOfType<Player>();
        player.StartSideHopping();
    }
}
