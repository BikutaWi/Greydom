using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Doc : https://www.youtube.com/channel/UCzgvT3r-o8-Qqt9O3K_PHuA

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;
    PlayerManager playerManager;
    PlayerLocomotion playerLocomotion;

    int horizontalID;
    int verticalID;

    public int dodgeDistance = 1000;
   


    /// <summary>
    /// When the game start
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        horizontalID = Animator.StringToHash("Horizontal");
        verticalID = Animator.StringToHash("Vertical");
    }

    /// <summary>
    /// Function which update movement of player with blend tree values
    /// </summary>
    /// <param name="verticalMovement">Vertical Movement of the player</param>
    /// <param name="horizontalMovement">Horizontal movement of the player</param>
    public void UpdateAnimator(float verticalMovement, float horizontalMovement, bool isPlayerSprinting)
    {
        //Don't block between animation (better transition)
        float snappedHorizontal = SnapValue(horizontalMovement);
        float snappedVertical = SnapValue(verticalMovement);

        // if player sprinting
        if(isPlayerSprinting)
        {
            snappedHorizontal = 2;
            snappedVertical = 2;
        }
        
       

        // Movement
        animator.SetFloat(horizontalID, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(verticalID, snappedVertical, 0.1f, Time.deltaTime);
    }

    private float SnapValue(float movementValue)
    {
        float snappedValueResult;

        if (movementValue > 0 && movementValue < 0.55f)
        {
            snappedValueResult = 0.5f;
        }
        else if (movementValue > 0.55f)
        {
            snappedValueResult = 1;
        }
        else if (movementValue < 0 && movementValue > -0.55f)
        {
            snappedValueResult = -0.5f;
        }
        else if (movementValue < -0.55f)
        {
            snappedValueResult = -1;
        }
        else
        {
            snappedValueResult = 0;
        }

        return snappedValueResult;
    }

    public void PlayAnimation(string animName, bool isInteraction)
    {
        animator.SetBool("isInteraction", isInteraction);
        //animator.SetBool("isRootMotion", useMotion);
        animator.CrossFade(animName, 0.2f);
    }

    /*private void OnAnimatorMove()
    {
        if(playerManager.isRootMotion)
        {
            playerLocomotion.playerRigidBody.drag = 0;
            Vector3 animationDeltaPosition = animator.deltaPosition;
            animationDeltaPosition.y = 0;
            //Vector3 velocity = animationDeltaPosition / Time.deltaTime;
            Vector3 velocity = animationDeltaPosition / Time.deltaTime;
            playerLocomotion.playerRigidBody.velocity = velocity / 2;
        } 
    } */
}
