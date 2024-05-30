using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plasma : MonoBehaviour
{
    // private Collider collider;
    public float Damage;
    public float Velocity;
    private new Rigidbody rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position += transform.forward * Velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision) {
        Player hitPlayer = collision.collider.GetComponent<Player>();
        if (hitPlayer != null) {
            hitPlayer.RecieveDamage(Damage);
        }
        Destroy(gameObject);
    }
}
