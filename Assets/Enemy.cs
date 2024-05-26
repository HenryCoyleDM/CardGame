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
    public bool AttackIsTriggered = false;
    private EnemyAnimation enemyAnimation;
    
    // Start is called before the first frame update
    void Start()
    {
        TargetPlayer = FindObjectOfType<Player>();
        characterController = GetComponent<CharacterController>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController.isGrounded) {
            VerticalVelocity = 0.0f;
        } else {
            VerticalVelocity -= Gravity * Time.deltaTime;
        }
        Vector3 Velocity = Vector3.up * VerticalVelocity;
        UpdateAttackingState();
        if (!enemyAnimation.IsInAttackingAnimation()) {
            transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(TargetPlayer.transform.position - transform.position).eulerAngles.y, 0.0f);
            Velocity += transform.forward * MovementSpeed;
        }
        characterController.Move(Velocity * Time.deltaTime);
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

    // private void AnimateAttack(float progress) {
        // transform.rotation = Quaternion.Euler(AttackAnimation.Evaluate(AttackProgress) * 60.0f, transform.rotation.eulerAngles.y, 0.0f);
    // }

    public float GetDistanceToTarget() {
        return (transform.position - TargetPlayer.transform.position).magnitude;
    }

    public bool UpdateAttackingState() {
        bool isCloseEnough = GetDistanceToTarget() < AttackTriggerDistance;
        AttackIsTriggered = isCloseEnough;
        return isCloseEnough;
    }

    public void ActivateAttack() {
        DealDamage(10.0f, TargetPlayer);
    }
}
