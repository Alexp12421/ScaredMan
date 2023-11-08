using EthanTheHero;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform tracePoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    //healthbar

    public HealthIndicator healthIndicator;


    //player attack variables
    public int attackPower;
    float[] attackMultiplier = { 5, 5, 5 };
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
        if (healthIndicator)
        {
            healthIndicator.setHpbar(currentHp, maxHp);
        }
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

        if(isDead == false)
        {
        // hurt animation
        animator.SetTrigger("Hurt");

            if (healthIndicator)
            {
                healthIndicator.setHpbar(currentHp, maxHp);
            }

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
        string[] attacks= {"Attack", "Attack2", "Attack3"};
        int i;
        

        i = Random.Range(0, attacks.Length);


        
        Debug.Log(attacks[i] + ' ' + attackMultiplier[i]);
        

        animator.SetTrigger(attacks[i]);
        

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(tracePoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            
            DmgMultiplier(i);
            enemy.GetComponent<EnemyBehaviour>().TakeDamage(attackMultiplier[i]); // infusing dmg to monsters


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

    private void resetAttackMultiplier() {
        attackMultiplier[0] = attackMultiplier[1] = attackMultiplier[2] = 5 ;
    }

    private void DmgMultiplier(int i)
    {
        //Damage multiplier
        switch (i)
        {
            case 0:
                attackMultiplier[i] += 0.1f;
                break;
            case 1:
                attackMultiplier[i] += 1f;
                break;
            case 2:
                attackMultiplier[i] *= 2f;
                break;
        }
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

    public float getCurrentHp() 
    {
        return currentHp;
    }


}
