using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    PlayerLife playerlife;
    PlayerAttack playerAttack;
   
    private void Awake()
    {
        playerlife = GetComponent<PlayerLife>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.tag == "Heart")
            {
                Destroy(other.gameObject);
                playerlife.HealPlayer(1);

            }

        if (other.gameObject.tag == "GuardZone")
        {
            Destroy(other.gameObject);
            playerAttack.ReloadGuard(0.5f);

        }    
    }

}
