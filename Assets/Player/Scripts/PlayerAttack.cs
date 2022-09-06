using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public class PlayerAttack : MonoBehaviour
{

    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;
    PlayerLife playerlife;
    private Animator playerAnimator;

    InputManager inputManager;

    public bool canCombo;
    int comboStep = 0;

    [SerializeField]  GameObject guardGameObject;

    [SerializeField] BoxCollider weaponCollider;

    private bool playerIsAttacking = false;
    public bool playerIsGuarding = false;


    [SerializeField] Slider sliderGuard;

    [SerializeField] AttackEnemy attackEnemy;


  
   

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAnimator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        playerlife = GetComponent<PlayerLife>();
        sliderGuard.value = 1;

        
    }

    private void Update()
    {
        if(playerIsGuarding)
        {
            sliderGuard.value -= 0.005f;
        }

        if(!playerIsGuarding && sliderGuard.value < 1)
        {
            sliderGuard.value += 0.002f;
        }
    }

    public void Attack()
    {

        if (!playerManager.isInteraction && playerLocomotion.isPlayerOnGround && !playerLocomotion.isPlayerJumping && !playerIsGuarding && playerlife.hearts > 0)
        {
            

            playerIsAttacking = true;
            playerLocomotion.StartAttack();

            // if weapon is unequiped
            if (!playerLocomotion.isWeaponEquiped)
            {
                playerLocomotion.SwitchWeapon();
            }

            if (comboStep == 0)
            {
                attackEnemy.EnableWeaponCollider();
                playerAnimator.Play("combo1");
                comboStep = 1;
                SwitchAttackStatus(false);
                
                return;
            }

            //if it's not the first attack
            if (comboStep != 0)
            {
                if (canCombo)
                {
                    canCombo = !canCombo;
                    comboStep += 1;
                }
            }
        }
    }



    public void Combo()
    {
        if(inputManager.buttonAttack)
        {
            attackEnemy.EnableWeaponCollider();
            if (comboStep == 2)
            {
                playerAnimator.Play("combo2");
            }
            if (comboStep == 3)
            {
                playerAnimator.Play("combo3");
            }
            if (comboStep == 4)
            {
                playerAnimator.Play("combo4");
            }
            if (comboStep == 5)
            {
                playerAnimator.Play("combo5");
            }

            // after switch mode
            //attackEnemy.DisableWeaponCollider();
            SwitchAttackStatus(false);
        }        
    }

    public void ResetAttack()
    {
        canCombo = false;
        comboStep = 0;
        playerLocomotion.StopFight();
        playerIsAttacking = false;
        attackEnemy.DisableWeaponCollider();
    }

    private void SwitchAttackStatus(bool status)
    {
        inputManager.buttonAttack = status;
    }
    public void ComboPossibility()
    {
        canCombo = true;
    }

    public void EnableWeaponCollider()
    {
        weaponCollider.enabled = true;
        Debug.Log("WEAPON");
    }

    public void DisableWeaponCollider()
    {
        weaponCollider.enabled = false;
        Debug.Log("NO WEAPON");
    }

    // Guard effect appears
    public void Guard()
    {
        if(inputManager.buttonGuard && sliderGuard.value > 0 && !playerIsAttacking)
        {
            guardGameObject.SetActive(true);
            playerAnimator.SetBool("blocking", true);
            playerIsGuarding = true;
        }
        else
        {
            guardGameObject.SetActive(false);
            playerAnimator.SetBool("blocking", false);
            playerIsGuarding = false;
        }
    }


    public void ReloadGuard(float amount)
    {
        if(sliderGuard.value < 1)
        {
            sliderGuard.value += 1 - amount;
        }
    }




}
