using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent wolfAgent;


    // distance between player and enemy
    private float distanceEnemyPlayer;

    // range
    public float chaseRange = 20f;
    public float attackRange = 1.5f;

    public int lifeEnemy = 100;

    //cooldown
    //public float attackRepeatTime = 1;


    private Animator wolfAnimator;

    public bool canAttack = true;

    private int attackCount = 0;

    private bool isItemDrop = false;


    public GameObject attackColliders;

    [SerializeField] PlayerLife playerlife;
    [SerializeField] PlayerAttack playerAttack;

    public GameObject ItemHealth;
    public GameObject ItemEnergy;

    private void Awake()
    {
        wolfAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        wolfAnimator = GetComponentInChildren<Animator>();

        playerlife = player.GetComponent<PlayerLife>();
        playerAttack = player.GetComponent<PlayerAttack>();
        
    }

    private void Update()
    {
        //distance between player and wolf
        distanceEnemyPlayer = Vector3.Distance(player.transform.position, transform.position);

        // turn to player
        if (distanceEnemyPlayer <= wolfAgent.stoppingDistance)
        {
            RotateToTarget();
        }

        // if enemy is alive
        if (lifeEnemy > 0)
        {
            if (wolfAgent.velocity.magnitude > 0)
            {
                wolfAnimator.SetBool("run", true);
            }
            else
            {
                wolfAnimator.SetBool("run", false);
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
        wolfAnimator.SetBool("run", false);
        wolfAgent.velocity = Vector3.zero;
        RotateToTarget();

    }

    private void ChasePlayer()
    {
        RotateToTarget();
        wolfAgent.destination = player.transform.position;
        attackColliders.SetActive(false);
    }

    private void AttackPlayer()
    {
            //StartCoroutine("Cooldown");
        RotateToTarget();
        if (canAttack)
        {
            int attack = AleatAnimation();
            Debug.Log(attack);
            wolfAgent.destination = transform.position;

            Vector3 posDuringAttack = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(posDuringAttack);

            if (attack == 0)
            {
                wolfAnimator.SetTrigger("attack1");
            }

            if (attack == 1)
            {
                wolfAnimator.SetTrigger("attack2");
            }

            canAttack = false;
            attackCount++;
            Debug.Log("attack player");

            if(attackCount % 2 == 0)
            {
                // reduce player heart
                //ReducePlayerLife();
                if(!playerAttack.playerIsGuarding)
                {
                    StartCoroutine("ReducePlayerLife");
                }
               
            }

            attackColliders.SetActive(false);
            StartCoroutine("Cooldown");
        }

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
        wolfAgent.isStopped = true;

        wolfAnimator.SetBool("death", true);

        //transform.position = new Vector3(this.transform.position.x, -1, this.transform.position.z);

        this.GetComponent<BoxCollider>().enabled = false;
       
        Destroy(this.gameObject, 5f);
       
        
         DropItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerWeapon")
        {
            lifeEnemy -= 30;
        }
    }
    

    // no time to finish this part
    // we wanted to drop item when enemy's die.. :( 
    private void DropItem()
    {
        if(!isItemDrop)
        {
            int number = AleatAnimation();

            switch (number)
            {
                case 0:
                    Instantiate(ItemHealth, new Vector3(this.transform.position.x, 0.1f, this.transform.position.z), Quaternion.identity);
                    break;

                case 1:
                    Instantiate(ItemEnergy, new Vector3(this.transform.position.x, 0.1f, this.transform.position.z), Quaternion.Euler(-90,0,0));
                    break;
            }

            isItemDrop = true;
        }
        
    } 
}
