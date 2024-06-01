using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Hesitation : Card
{
    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/Hesitation.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard()
    {
        throw new System.NotImplementedException("Hesitation is unplayable");
    }
}
