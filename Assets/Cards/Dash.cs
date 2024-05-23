using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Dash : Card
{
    // Start is called before the first frame update
    void Start()
    {
        CardName = "Dash";
        CardText = "x2 Runspeed for 15 s";
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
