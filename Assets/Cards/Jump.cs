using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Jump : Card
{

    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/Jump.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard()
    {
        Player player = FindObjectOfType<Player>();
        player.Jump(player.JumpForce * 2);
    }
}
