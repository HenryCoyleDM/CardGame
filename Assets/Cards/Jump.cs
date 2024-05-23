using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : Card
{

    // Start is called before the first frame update
    void Start()
    {
        CardName = "Jump";
        CardText = "Jump with twice normal jump height";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PlayCard()
    {
        Debug.Log("Called jump card");
        Player player = FindObjectOfType<Player>();
        player.Jump(player.JumpForce * 2);
    }
}
