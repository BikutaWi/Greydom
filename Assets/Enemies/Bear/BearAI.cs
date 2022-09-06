using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BearAI : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent bearAgent;


    // distance between player and enemy
    private float distanceEnemyPlayer;

    // range
    public float chaseRange = 20f;
    public float attackRange = 5f;

    public int lifeEnemy = 250;

    private Animator bearAnimator;

    public bool canAttack = true;

    private int attackCount = 0;

    public GameObject attackColliders;

    [SerializeField] PlayerLife playerlife;
    [SerializeField] PlayerAttack playerAttack;

    private void Awake()
    {
        bearAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        bearAnimator = GetComponentInChildren<Animator>();

        playerlife = player.GetComponent<PlayerLife>();
        playerAttack = player.GetComponent<PlayerAttack>();
    }

    private void Update()
    {
        //distance between player and wolf
        distanceEnemyPlayer = Vector3.Distance(player.transform.position, transform.position);

        // turn to player
        if (distanceEnemyPlayer <= bearAgent.stoppingDistance)
        {
            RotateToTarget();
        }

        // if enemy is alive
        if (lifeEnemy > 0)
        {
            if (bearAgent.velocity.magnitude > 0)
            {
                bearAnimator.SetBool("run", true);
            }
            else
            {
                bearAnimator.SetBool("run", false);
            }

            // movement possibles
            if (distanceEnemyPlayer > chaseRange)
            {
                Patrol();
            }

            if (distanceEnemyPlayer < chaseRange && distanceEnemyPlayer > attackRange)
            {
                ChasePlayer();
            }

            if (distanceEnemyPlayer < attackRange)
            {
                AttackPlayer();
            }
        }
        else
        {
            Death();
            attackColliders.SetActive(false);
        }


    }

    private void Patrol()
    {
        bearAnimator.SetBool("run", false);
        bearAgent.velocity = Vector3.zero;
        RotateToTarget();

    }

    private void AttackPlayer()
    { 
        RotateToTarget();
        if (canAttack)
        {
            int attack = AleatAnimation();
            Debug.Log(attack);
            bearAgent.destination = transform.position;

            Vector3 posDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(posDuringAttack);

            if (attack == 0)
            {
                bearAnimator.SetTrigger("attack1");
            }

            if (attack == 1)
            {
                bearAnimator.SetTrigger("attack2");
            }

            canAttack = false;
            attackCount++;
            Debug.Log("attack player");

            if (attackCount % 2 == 0)
            {
                // reduce player heart
                //ReducePlayerLife();
                if (!playerAttack.playerIsGuarding)
                {
                    StartCoroutine("ReducePlayerLife");
                }

            }

            attackColliders.SetActive(false);
            StartCoroutine("Cooldown");
        }

    }

    private void ChasePlayer()
    {
        RotateToTarget();
        bearAgent.destination = player.transform.position;
        attackColliders.SetActive(false);
    }



    private IEnumerator ReducePlayerLife()
    {
        yield return new WaitForSeconds(0.5f);
        playerlife.DamagePlayer(1);
        attackCount = 0;
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2f);
        attackColliders.SetActive(true);
        canAttack = true;

    }

    public void DamageEnemy(int damage)
    {
        lifeEnemy -= damage;
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
        bearAnimator.SetBool("run", false);
        bearAnimator.SetBool("death", true);
        
        bearAgent.isStopped = true;
        

        this.GetComponent<BoxCollider>().enabled = false;

        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon")
        {
            lifeEnemy -= 30;
        }
    }



}
