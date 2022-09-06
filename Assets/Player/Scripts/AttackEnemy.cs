using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public GameObject weaponCollider;


    public void EnableWeaponCollider()
    {
        weaponCollider.SetActive(true);
        Debug.Log("can attack");
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.SetActive(false);
        Debug.Log("Rip attack");
    }
}
