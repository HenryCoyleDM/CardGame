using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Stab : Card
{

    // Start is called before the first frame update
    public override void Start()
    {
        Details = AssetDatabase.LoadAssetAtPath<CardDetails>("Assets/Cards/Stab.asset");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard()
    {
        
        Player player = FindObjectOfType<Player>();
        player.StartStabbing();
        //Collider[] colliders = Physics.OverlapSphere(player.transform.position, 5.0f);
        //foreach (Collider collider in colliders) {
        //    Enemy enemy = collider.gameObject.GetComponent<Enemy>();
        //    if (enemy != null) {
        //        enemy.RecieveDamage(10.0f);
        //        return;
        //    }
        //}
    }
}
