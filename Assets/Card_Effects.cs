using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Effects : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1")) {
            Dash_Card();
        }
        if (Input.GetKeyDown("2")) {
            Jump_Card();
        }
    }

    void Dash_Card() {
        Player player = FindObjectOfType<Player>();
        player.MovementSpeed *= 2;
        StartCoroutine(DelayUndash(player));
    }

    IEnumerator DelayUndash(Player player) {
        yield return new WaitForSeconds(15f);
        player.MovementSpeed /= 2;
    }

    void Jump_Card() {
        Debug.Log("Called jump card");
        Player player = FindObjectOfType<Player>();
        player.Jump(player.JumpForce * 2);
    }
}
