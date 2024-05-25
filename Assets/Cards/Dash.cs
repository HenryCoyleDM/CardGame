using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Dash : Card
{
    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/Dash.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard() {
        Player player = FindObjectOfType<Player>();
        player.MovementSpeed *= 2;
        StartCoroutine(DelayUndash(player));
    }

    IEnumerator DelayUndash(Player player) {
        yield return new WaitForSeconds(15f);
        player.MovementSpeed /= 2;
    }
}
