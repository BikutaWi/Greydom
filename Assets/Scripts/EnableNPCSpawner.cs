using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableNPCSpawner : MonoBehaviour
{
    public GameObject spawner;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            spawner.SetActive(true);
        }
    }
}
