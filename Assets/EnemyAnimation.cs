using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    public Enemy enemyBrain;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyBrain = GetComponentInParent<Enemy>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsAttacking", enemyBrain.AttackIsTriggered);
    }

    public bool IsInAttackingAnimation() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.EnemyAttack");
    }

    public void AttackHits() {
        enemyBrain.ActivateAttack();
    }
}
