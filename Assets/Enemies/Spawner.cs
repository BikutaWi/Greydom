using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int enemiesInZone = 3;
    public GameObject wolf;
    public GameObject bear;

    public bool isContainingBears = false;

    private GameObject parent;

    public Transform spawnPoint;

    private int enemyLeft;
    private int enemies;


    private void Awake()
    {
        parent = transform.parent.gameObject;
        enemyLeft = this.transform.childCount;
        enemies = enemyLeft;
    }


    private void Update()
    {
        if(enemiesInZone != 0)
        {
            if(transform.childCount == 0)
            {
                SpawnEnemy();
                enemiesInZone--;
            }
        }
        else
        {
            transform.parent = null;
            Destroy(parent);
        }
        
    }

    private void SpawnEnemy()
    {
        if(isContainingBears)
        {
            int enemyID = GenerateEnemyID();

            switch(enemyID)
            {
                case 0: Instantiate(wolf, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity , this.transform);
                    break;

                case 1:
                    Instantiate(bear, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity, this.transform);
                    break;
            }
        }
        else
        {
            Instantiate(wolf, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity, this.transform);
        }
    }

    

    private int GenerateEnemyID()
    {
        return Random.Range(0, 2);
    }
}
