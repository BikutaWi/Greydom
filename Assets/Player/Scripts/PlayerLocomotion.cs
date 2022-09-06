using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 // Doc : https://www.youtube.com/channel/UCzgvT3r-o8-Qqt9O3K_PHuA

public class PlayerLocomotion : MonoBehaviour
{
    // PlayerLife script
    PlayerLife playerLife;

    // InputManager script
    InputManager inputManager;

    // PlayerManager script
    PlayerManager playerManager;

    // Animator Manager script
    AnimatorManager animatorManager;

    // direction where player move
    Vector3 playerMovement;

    // camera behind player
    Transform cameraTransform;

    // when player die
    Menu menu;

    // Rigidbody of player
    public Rigidbody playerRigidBody;

    // Player Speed
    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float sprintSpeed = 8;
    public float rotationSpeed = 10;

    // Player jumping & falling
    public bool isPlayerOnGround;
    private float inAirTimer;
    public float fallSpeed = 30;
    public float fallVelocity = 3;
    public LayerMask groundLayer;
    private float heightOffsetCast = 0.5f;
    public bool isPlayerJumping;

    public float gravity = -15;
    public float jumpHeight = 2;


    //Unarmed Gameobject
    public GameObject weaponBack;
    public GameObject weaponHand;
    public bool isWeaponEquiped = false;


    // is player sprinting (shift or joystick)
    public bool isPlayerSprinting = false;

    //colliders
    private CapsuleCollider capsuleCollider;
    [SerializeField] BoxCollider deathCollider;
    

    

    /// <summary>
    /// When game start
    /// </summary>
    private void Awake()
    {
        playerLife = GetComponent<PlayerLife>();

        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();

        playerRigidBody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;

        capsuleCollider = GetComponent<CapsuleCollider>();

        menu = GetComponent<Menu>();
    }


    /// <summary>
    /// Call every movement function
    /// </summary>
    public void HandleAllMovement()
    {
        if(playerLife.hearts > 0)
        {
            HandleFalling();

            // if interaction, player can't move or do anything else
            if (!playerManager.isInteraction)
            {
                HandleMovement();
                HandleRotation();
            }
        }

        else
        {
            HandleDeath();
        } 
    }


    /// <summary>
    /// Movement X / Y of the player
    /// </summary>
    private void HandleMovement()
    {
        if(!isPlayerJumping)
        {
            playerMovement = cameraTransform.forward * inputManager.verticalInput;
            playerMovement = playerMovement + cameraTransform.right * inputManager.horizontalInput;
            playerMovement.Normalize();
            playerMovement.y = 0; // can't fly

            // change speed of player (joystick position and sprint)
            if (isPlayerSprinting)
            {
                playerMovement = playerMovement * sprintSpeed;
            }
            else
            {
                if (inputManager.moveAmount >= 0.55f)
                {
                    playerMovement = playerMovement * runSpeed;
                }
                else
                {
                    playerMovement = playerMovement * walkSpeed;
                }
            }


            Vector3 movementVelocity = playerMovement;
            playerRigidBody.velocity = movementVelocity;
        }
    }


    /// <summary>
    /// Rotation X / Z of the player
    /// </summary>
    private void HandleRotation()
    {
        if(!isPlayerJumping)
        {
            Vector3 targetDirection = Vector3.zero;

            targetDirection = cameraTransform.forward * inputManager.verticalInput;
            targetDirection = targetDirection + cameraTransform.right * inputManager.horizontalInput;
            targetDirection.Normalize();
            targetDirection.y = 0;

            // keep the rotation when player stop moving
            if (targetDirection == Vector3.zero)
            {
                targetDirection = transform.forward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            transform.rotation = playerRotation;
        }
       
    }

    private void HandleFalling()
    {
        RaycastHit hit;

        Vector3 feetPosition = transform.position;
        
        // feet of player
        Vector3 origin = transform.position;
        origin.y = origin.y + heightOffsetCast;

        // if player is not on the ground and in interaction, start fall animation
        if(!isPlayerOnGround && !isPlayerJumping)
        {
            if(!playerManager.isInteraction)
            {
                animatorManager.PlayAnimation("Falling", true);
            }


            animatorManager.animator.SetBool("isRootMotion", false);

            // more time in air = quicker fall (acceleration)
            inAirTimer = inAirTimer + Time.deltaTime;

            playerRigidBody.AddForce(transform.forward * fallVelocity);
            playerRigidBody.AddForce(-Vector3.up * fallSpeed * inAirTimer);
        }

        // draw invisible sphere around the player to detect the ground
        if(Physics.SphereCast(origin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            // if we were in the air, start landing animation
            if(!isPlayerOnGround && !playerManager.isInteraction)
            {
                animatorManager.PlayAnimation("Landing", true);
            }

            // feet
            Vector3 rayCastHitPoint = hit.point;
            feetPosition.y = rayCastHitPoint.y;

            inAirTimer = 0;
            isPlayerOnGround = true;
        }
        else
        {
            isPlayerOnGround = false;
        }


        if(isPlayerOnGround && !isPlayerJumping)
        {
            if(playerManager.isInteraction || inputManager.moveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, feetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = feetPosition;
            }
        }

    }

    public void HandleJumping()
    {
        // if player can jump
        if(isPlayerOnGround)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayAnimation("Jump", false);

            float jumpingVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            Vector3 velocityPlayer = playerMovement;
            velocityPlayer.y = jumpingVelocity;
            playerRigidBody.velocity = velocityPlayer;
        }           


    }

    public void HandleDodge()
    {
        if(!playerManager.isInteraction && isPlayerOnGround && !isPlayerJumping)
        {
            animatorManager.PlayAnimation("Dodge", true);
            playerRigidBody.AddForce(transform.forward * AdaptDodgeDistance() * Time.deltaTime, ForceMode.Impulse);
            //playerRigidBody.AddForce(transform.forward * 250, ForceMode.Impulse);  
        }
    }

    public void HandleUnarmed()
    {
        if(!playerManager.isInteraction && isPlayerOnGround && !isPlayerJumping)
        {
            animatorManager.PlayAnimation("Unarmed", true);
        }
    }

    private int AdaptDodgeDistance()
    {
        if(isPlayerSprinting)
        {
            return 750;
        }
        else
        {
            return 1500;
        }
    }

    public void HandleDeath()
    {
        animatorManager.animator.SetBool("death", true);
        menu.GameOver();
    }

    
    // call when character is on ground
    public void EnableDeathCollider()
    {
        //isTrigger capsule collider
        capsuleCollider.isTrigger = true;

        deathCollider.enabled = true;
    }


    /// <summary>
    /// Switch between weapon in hand and in back
    /// </summary>
    public void SwitchWeapon()
    {
        if(isWeaponEquiped == true)
        {
            weaponBack.SetActive(true);
            weaponHand.SetActive(false);
        }
        if(isWeaponEquiped == false)
        {
            weaponHand.SetActive(true);
            weaponBack.SetActive(false);
        }

        isWeaponEquiped = !isWeaponEquiped;
    }

    

    public void StartAttack()
    {
        walkSpeed = 0;
        runSpeed = 0;
        sprintSpeed = 0;

    }

    public void StopFight()
    {
        walkSpeed = 2;
        runSpeed = 6;
        sprintSpeed = 8;
    }





}
