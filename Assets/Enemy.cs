using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class Enemy : MonoBehaviour
{
    public float AttackStrength;
    public float HP;
    public float MaxHP;
    public float MovementSpeed;
    public float Gravity;
    public float AttackReach;
    public float AttackTriggerDistance;
    private float VerticalVelocity = 0.0f;
    private Player TargetPlayer;
    private CharacterController characterController;
    private bool IsAttacking = false;
    private float AttackProgress = 0.0f;
    public AnimationCurve AttackAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        TargetPlayer = FindObjectOfType<Player>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded) {
            VerticalVelocity = 0.0f;
        } else {
            VerticalVelocity -= Gravity * Time.deltaTime;
        }
        if (IsAttacking) {
            bool has_hit = AttackProgress > 0.5f;
            AttackProgress += Time.deltaTime;
            if (AttackProgress >= 1.0f) {
                AttackProgress = 0.0f;
                IsAttacking = false;
            }
            if (!has_hit && AttackProgress > 0.5f) {
                DealDamage(AttackStrength, TargetPlayer);
            }
            AnimateAttack(AttackProgress);
        } else {
            transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(TargetPlayer.transform.position - transform.position).eulerAngles.y, 0.0f);
            characterController.Move((transform.forward * MovementSpeed + Vector3.up * VerticalVelocity) * Time.deltaTime);
            if (GetDistanceToTarget() < AttackTriggerDistance) {
                IsAttacking = true;
            }
        }
        
    }

    public void DealDamage(float amount, Player target) {
        if (GetDistanceToTarget() < AttackReach) {
            target.RecieveDamage(amount);
        }
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

    private void AnimateAttack(float progress) {
        transform.rotation = Quaternion.Euler(AttackAnimation.Evaluate(AttackProgress) * 60.0f, transform.rotation.eulerAngles.y, 0.0f);
    }

    private float GetDistanceToTarget() {
        return (transform.position - TargetPlayer.transform.position).magnitude;
    }
}
