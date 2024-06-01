using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    new private Rigidbody rigidbody;
    new private Collider collider;
    public float Damage = 20.0f;
    public float KnockbackForce = 10.0f;
    private Vector3 equilibriumPosition;
    private Quaternion equilibriumRotation;
    private Transform enemyTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        equilibriumPosition = transform.localPosition;
        equilibriumRotation = transform.localRotation;
        enemyTransform = GetComponentInParent<SwordEnemy>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.localPosition = equilibriumPosition;
        transform.localRotation = equilibriumRotation;
    }

    void OnCollisionEnter(Collision collision) {
        Player collided_player = collision.collider.GetComponent<Player>();
        if (collided_player != null) {
            collided_player.RecieveDamage(Damage);
            collided_player.RecieveKnockback((enemyTransform.forward + enemyTransform.up) * KnockbackForce, 0.0f);
            Debug.Log("Sword landed blow on player");
        }
    }
}
