using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Doc : https://www.youtube.com/channel/UCzgvT3r-o8-Qqt9O3K_PHuA
public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    PlayerAttack playerAttack;

    [SerializeField] AttackEnemy attackenemy;

    //HideCursor hidecursor;
    Menu menu;


    public Vector2 movementInput;
    public Vector2 cameraInput;


    // input
    public float verticalInput;
    public float horizontalInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;

    public bool buttonSprinting;
    public bool buttonJumping;
    public bool buttonDodging;

    public bool buttonUnarmed;


    public float moveAmount;

    private bool buttonPause = false;

    public bool buttonAttack;
    public bool canCombo = false;

    public bool buttonGuard;

    
    

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        //hidecursor = GetComponent<HideCursor>();
        menu = GetComponent<Menu>();

        playerAttack = GetComponent<PlayerAttack>();
    }


    /// <summary>
    /// When the gameobject is enable
    /// </summary>
    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            // "record" keyboard (or gamepad) in variable
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            // "record" sprint
            playerControls.PlayerActions.Sprint.performed += i => buttonSprinting = true;
            playerControls.PlayerActions.Sprint.canceled += i => buttonSprinting = false;

            // "record" jump
            playerControls.PlayerActions.Jump.performed += i => buttonJumping = true;

            // "record" dodge
            playerControls.PlayerActions.Dodge.performed += i => buttonDodging = true;

            // "record" unarmed / armed
            playerControls.PlayerActions.Unarmed.performed += i => buttonUnarmed = true;

            // "record" pause (escape and start)
            playerControls.PlayerActions.Pause.performed += i => buttonPause = true;

            // "recorc" melee attack
            playerControls.PlayerActions.Melee.performed += i => buttonAttack = true;

            // "reccord" guard attack
            playerControls.PlayerActions.Guard.performed += i => buttonGuard = true;
            playerControls.PlayerActions.Guard.canceled += i => buttonGuard = false;

        }

        // enable control
        playerControls.Enable();
    }

    /// <summary>
    /// When gameobject is diable, gameobject can't be controled by player (cinematic for example)
    /// </summary>
    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInput()
    {
        HandleMovementInput();
        HandleSprintInput();
        HandleJumping();
        HandleDodgeInput();

        HandleUnarmed();

        HandleGamePause();

        HandleAttack();

        HandleGuard();
    }

    private void HandleMovementInput()
    {
        // WASD / LeftStick Movement
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        
        // Mouse / RightStick Camera rotation
        cameraHorizontalInput = cameraInput.x;
        cameraVerticalInput = cameraInput.y;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimator(0, moveAmount, playerLocomotion.isPlayerSprinting);
    }

    private void HandleSprintInput()
    {
        if(buttonSprinting && moveAmount > 0.5f)
        {
            playerLocomotion.isPlayerSprinting = true;
        }
        else
        {
            playerLocomotion.isPlayerSprinting = false;
        }
    }

    private void HandleJumping()
    {
        if(buttonJumping)
        {
            buttonJumping = false;
            playerLocomotion.HandleJumping();

        }
    }

    private void HandleDodgeInput()
    {
        if(buttonDodging == true)
        {
            buttonDodging = false;
            playerLocomotion.HandleDodge();
        }
    }

    private void HandleUnarmed()
    {
        if(buttonUnarmed == true)
        {
            buttonUnarmed = false;
            playerLocomotion.HandleUnarmed();
        }
    }

    private void HandleGamePause()
    {
        if(buttonPause)
        {
            buttonPause = false;
            //hidecursor.CursorVisible();
            menu.MakeAnAction();
        }      
    }

    private void HandleAttack()
    {
        if(buttonAttack)
        {
            //buttonAttack = false;

            attackenemy.EnableWeaponCollider();
            playerAttack.Attack();   
            Debug.Log("start attack : " + buttonAttack);
        }
    }

    private void HandleGuard()
    {
            playerAttack.Guard();
                  
    } 
}
