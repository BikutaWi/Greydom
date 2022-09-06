using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Doc : https://www.youtube.com/channel/UCzgvT3r-o8-Qqt9O3K_PHuA

/// <summary>
/// This class call all functionality of the player
/// </summary>
public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    CameraManager cameraManager;

    //animator
    Animator animator;

    // interaction animation
    public bool isInteraction;
    //public bool isJumping;

    public bool isRootMotion; 



    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        cameraManager = FindObjectOfType<CameraManager>();

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // call all input functions
        inputManager.HandleAllInput();
    }

    // in FixUpdate, because of the RigidBody
    private void FixedUpdate()
    {
        // call all movement functions
        playerLocomotion.HandleAllMovement();
    }

    // end of each frame
    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        isRootMotion = animator.GetBool("isRootMotion");

        isInteraction = animator.GetBool("isInteraction");
        playerLocomotion.isPlayerJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isPlayerOnGround);
    }


}
