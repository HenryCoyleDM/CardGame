using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float HP;
    public float MaxHP;
    public float MovementSpeed;
    public float Gravity;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecieveDamage(float amount) {
        HP -= amount;
        if (HP <= 0.0f) {
            Die();
        }
    }

    public void Die() {
        Destroy(gameObject);
    }
}
