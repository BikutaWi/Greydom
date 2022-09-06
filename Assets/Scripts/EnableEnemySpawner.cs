using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemySpawner : MonoBehaviour
{
    public GameObject Aura;
    public GameObject Walls;
    public GameObject Spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Aura.SetActive(true);
            Walls.SetActive(true);
            Spawner.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
