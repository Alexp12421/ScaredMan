using EthanTheHero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadManCombat : MonoBehaviour
{
    public Animator animator;
    public Transform tracePoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;


    //player attack variables
    public float attackPower = 150;
    private float attackPowerMultiplier = 1;
    public float attackRate = 3f;
    float nextAttackTime = 0;

    //player health variables

    public float maxHp = 100;
    private float currentHp;
    bool isDead = false;

    // Update is called once per frame

    private void Start()
    {
        currentHp = maxHp;
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + (1f / attackRate);
            }
        }

    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (isDead == false)
        {
            // hurt animation
            animator.SetTrigger("Hurt");

        }


        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        setIsDead(true);

        // die animation
        animator.SetBool("isDead", true);

        //Disable the collider

        GetComponent<Collider2D>().enabled = false;
        GetComponent<MovementController>().enabled = false;

        //Disable the enemy
        this.enabled = false;
    }

    void Attack()
    {


        animator.SetTrigger("Attack");

        Debug.Log(attackPower * attackPowerMultiplier);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(tracePoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
           
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(attackPower * attackPowerMultiplier); // infusing dmg to monsters
            DmgMultiplier();


            if (enemy.GetComponent<EnemyBehaviour>().getIsDead()) // death check
                resetAttackMultiplier();

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (tracePoint == null)
            return;

        Gizmos.DrawWireSphere(tracePoint.position, attackRange);
    }

    private void resetAttackMultiplier()
    {
        attackPowerMultiplier = 1;
    }

    private void DmgMultiplier()
    {
        attackPowerMultiplier *= 1.75f;
    }

    /* public float getAttackMultix(int i)
     { 
         return attackMultiplier[i];
     }
 */

    bool setIsDead(bool isTheEnemyDead)
    {
        isDead = isTheEnemyDead;
        return isDead;
    }

}
