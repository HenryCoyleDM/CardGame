using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[Serializable]
public class SwordEnemy : Enemy
{
    public float AttackTriggerDistance;
    private float VerticalVelocity = 0.0f;
    private Player TargetPlayer;
    private CharacterController characterController;
    private Sword sword;
    public bool AttackIsTriggered = false;
    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        TargetPlayer = FindObjectOfType<Player>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        sword = GetComponentInChildren<Sword>();
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
        if (!IsInAttackingAnimation() && GetHorizontalDistanceToTarget() > 1.0f) {
            Vector3 target_location = TargetPlayer.transform.position - 0.5f * transform.right;
            transform.rotation = Quaternion.Euler(0.0f, Quaternion.LookRotation(target_location - transform.position).eulerAngles.y, 0.0f);
            Velocity += transform.forward * MovementSpeed;
        }
        characterController.Move(Velocity * Time.deltaTime);
    }

    public float GetDistanceToTarget() {
        return (transform.position - TargetPlayer.transform.position).magnitude;
    }

    public float GetHorizontalDistanceToTarget() {
        Vector3 displacement = transform.position - TargetPlayer.transform.position;
        displacement.y = 0.0f;
        return displacement.magnitude;
    }

    public bool UpdateAttackingState() {
        bool isCloseEnough = GetDistanceToTarget() < AttackTriggerDistance;
        AttackIsTriggered = isCloseEnough;
        animator.SetBool("IsAttacking", AttackIsTriggered);
        return isCloseEnough;
    }

    public void ActivateAttack() {
        animator.SetBool("IsAttacking", false);
    }

    private bool IsInAttackingAnimation() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Attack");
    }
}
