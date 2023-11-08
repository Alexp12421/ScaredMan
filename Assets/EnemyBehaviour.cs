using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class EnemyBehaviour : MonoBehaviour
{

    public Animator animator;
    public Rigidbody2D rb;

    //monster view direction
    bool isFacingLeft = true;
    

    public float maxHp = 100;
    private float currentHp;
    private bool isDead = false;

    //attack variables needed below

    public float attackRange = 1f;
    public float attackInRange = 3;
    public float aggroRange = 6f;
    public Transform attackPoint;
    public Transform aggroPoint;
    public Transform attackRangeRadius;
    public LayerMask playerLayer;
    public float attackRate = 2f;
    float nextAttackTime = 0;
    int attackCounter = 0;
    float monsterDamageMultiplier = 1f;

    //Monster's speed
    public float speed = 0.5f;

    //Monster Damage
    public float monsterDamage = 35;

    
    void Start()
    {
        currentHp = maxHp;
    }
    private void Update()
    {
        //Making the monster to look in the player's direction
        //Debug.Log(transform.position);
        Collider2D playerAggroed = Physics2D.OverlapCircle(aggroPoint.position, aggroRange, playerLayer);

        if (playerAggroed && !(Physics2D.OverlapCircle(attackRangeRadius.position, attackInRange, playerLayer)) && !gameObject.name.Equals("Evil Wizard"))
        {
            Vector2 position = Vector2.MoveTowards(transform.position, playerAggroed.GetComponent<MovementController>().getPlayerTransform().position, speed * Time.deltaTime);
            Flip(position);
            transform.position = position;
            //Debug.Log(transform.position.x);


            animator.SetFloat("Speed", 1);

        }
        else if (!playerAggroed) 
        {
            animator.SetFloat("Speed", 0);
        }
        else if (Physics2D.OverlapCircle(attackRangeRadius.position, attackInRange, playerLayer))
        {
            animator.SetFloat("Speed", 0);

            if (Time.time >= nextAttackTime)
            {
                Attack();
                Debug.Log("Collision detected");
                nextAttackTime = Time.time + attackRate;
            }
        }
        


    }

    public void TakeDamage(float damage) 
    {
        currentHp -= damage;

        // hurt animation
        animator.SetTrigger("Hurt");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died!");
        setIsDead(true);

        // die animation
        animator.SetBool("isDead", true);

        //Disable the collider

        GetComponent<Collider2D>().enabled = false;
        //GetComponent<SpriteRenderer>().enabled = false;
        gameObject.transform.GetChild(3).gameObject.GetComponent<Collider2D>().enabled = false;
        //Disable the enemy script
        this.enabled = false;
    }

    void Attack() 
    {
        attackCounter++;

        if (gameObject.name.Equals("Evil Samurai") && attackCounter >= 2)
        {
            animator.SetTrigger("Attack2");
            attackCounter = 0;
            monsterDamageMultiplier = 2;
        }
        else 
        {
            animator.SetTrigger("Attack");
            monsterDamageMultiplier = 1;
        }
            

        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (hitPlayer)
        {
            if (hitPlayer.gameObject.name == "ScaredMan")
            {
                hitPlayer.GetComponent<PlayerCombat>().TakeDamage(monsterDamage * monsterDamageMultiplier);
            }
            else 
            {
                hitPlayer.GetComponent<MadManCombat>().TakeDamage(0.1f * monsterDamageMultiplier);
            }

            
        }


    }

    bool setIsDead(bool isTheEnemyDead) { 
        isDead = isTheEnemyDead;
        return isDead;
    }

    public bool getIsDead()
    {
        return isDead;
    }

    private void Flip(Vector2 position)
    {
        if (isFacingLeft && (transform.position.x - position.x) < 0 || !isFacingLeft && (transform.position.x - position.x) > 0)
        {
            isFacingLeft = !isFacingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void Flip()
    {
        
            isFacingLeft = !isFacingLeft;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if (aggroPoint == null)
            return;

        Gizmos.DrawWireSphere(aggroPoint.position, aggroRange);

        if (attackRangeRadius == null)
            return;

        Gizmos.DrawWireSphere(attackRangeRadius.position, attackInRange);
    }


    //method 1 of player detection
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Time.time >= nextAttackTime)
            {
                Attack();
                Debug.Log("Collision detected");
                nextAttackTime = Time.time + attackRate;
            }
            
        }
                       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Flip();
    }

}
