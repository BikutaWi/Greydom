using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public GameObject player;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;


    // distance between player and enemy
    private float distanceEnemyPlayer;

    // range
    public float chaseRange = 5f;
    public float attackRange = 2f;

    public float lifeEnemy = 100f;

    //cooldown
    public float attackRepeatTime = 1;
    private float attackTime;

    private Animator enemyAnimator;

    // damage to player
    public float damagePlayer;

    public bool canAttack = true;

    // Start is called before the first frame update
    void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //distance calculation
        distanceEnemyPlayer = Vector3.Distance(player.transform.position, transform.position);
        
        if(distanceEnemyPlayer <= navMeshAgent.stoppingDistance)
        {
            RotateToTarget();
        }
       
        // if enemy is alive
        if(lifeEnemy > 0)
        {
            if(navMeshAgent.velocity.magnitude > 0)
            {
                enemyAnimator.SetBool("run", true);
            }
            else
            {
                enemyAnimator.SetBool("run", false);
            }

            // movement possibles
            if(distanceEnemyPlayer > chaseRange)
            {
                Patrol();
            }

            if(distanceEnemyPlayer < chaseRange && distanceEnemyPlayer > attackRange)
            {
                ChasePlayer();
            }

            if(distanceEnemyPlayer < attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            Death();
        }



       // navMeshAgent.destination = player.transform.position;
        //actualiser le script
    }


    private void Patrol()
    {
        enemyAnimator.SetBool("run", false);
        navMeshAgent.velocity = Vector3.zero;
        RotateToTarget();
    }

    private void ChasePlayer()
    {
        RotateToTarget();
        navMeshAgent.destination = player.transform.position;
    }

    private void AttackPlayer()
    {
        RotateToTarget();
        //Debug.Log(canAttack);
        if(canAttack)
        {
            int attack = AleatAnimation();
            Debug.Log(attack);
            navMeshAgent.destination = transform.position;

            Vector3 posDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(posDuringAttack);

            if(attack == 0)
            {
                enemyAnimator.SetTrigger("attack1");
            }

            if(attack == 1)
            {
                enemyAnimator.SetTrigger("attack2");
            }
            
            canAttack = false;
            StartCoroutine("Cooldown");
        }
        
    }
    
    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        canAttack = true;
    }

    private int AleatAnimation()
    {
        return Random.Range(0, 2);
    }

    private void RotateToTarget()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // 0.5f to have a fluent rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f);
    }

    private void Death()
    {
        navMeshAgent.isStopped = true;

        int animNumber = AleatAnimation();

        enemyAnimator.SetBool("death2", true);

       /* if (animNumber == 0)
        {
            enemyAnimator.SetBool("death1", true);
        }

        if(animNumber == 1)
        {
            enemyAnimator.SetBool("death2", true);
        } */

        transform.position = new Vector3(this.transform.position.x, -1, this.transform.position.z);

        this.GetComponentInChildren<BoxCollider>().enabled = false;

        Destroy(this.gameObject, 5);
    }
}
