using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Doc : https://www.youtube.com/channel/UCzgvT3r-o8-Qqt9O3K_PHuA


public class CameraManager : MonoBehaviour
{
    InputManager inputManager;
    Menu menu;
    PlayerLife playerlife;

    // camera will follow this object
    private Transform followObjectTransform;

    //camera will use this object to turn
    public Transform cameraPivotObject;

    // Position of the camera
    private Transform cameraPos;

    public float defaultPos;

    private Vector3 cameraVelocity = Vector3.zero;

    public float followSpeed = 1f;

    public float leftrightAngle;
    public float updownAngle;
    public float sensibility = 1.5f;

    //angle min-max
    private float minAngle = -40;
    private float maxAngle = 30;



    private Vector3 cameraVectorPosition;

    // Layer camera can collide
    public LayerMask collisionLayers;

    // radius collision (when camera hit wall)
    private float collisionRadius = 0.3f;

    //When camera pass over object
    public float cameraCollisionOffset = 0.2f;

    public float minCollisionOffset = 0.2f;

    /// <summary>
    /// When the game start
    /// </summary>
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        menu = FindObjectOfType<Menu>();
        playerlife = FindObjectOfType<PlayerLife>();

        // Find the object which has PlayerManager script attached (player)
        followObjectTransform = FindObjectOfType<PlayerManager>().transform;

        cameraPos = Camera.main.transform;

        defaultPos = cameraPos.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        CameraRotation();
        HandleCameraCollision();
    }


    private void FollowTarget()
    {
        Vector3 followPos = Vector3.SmoothDamp(transform.position, followObjectTransform.position, ref cameraVelocity, followSpeed);
        
        transform.position = followPos;
    }

    private void CameraRotation()
    {
        Vector3 rotation = Vector3.zero;


        if(!menu.gamePaused && playerlife.hearts > 0)
        {
            leftrightAngle = leftrightAngle + (inputManager.cameraHorizontalInput * sensibility);
            updownAngle = updownAngle - (inputManager.cameraVerticalInput * sensibility);

            updownAngle = Mathf.Clamp(updownAngle, minAngle, maxAngle);
        }
        

        rotation = Vector3.zero;
        rotation.y = leftrightAngle;

        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;


        rotation = Vector3.zero;
        rotation.x = updownAngle;
        targetRotation = Quaternion.Euler(rotation);

        cameraPivotObject.localRotation = targetRotation;

    }

    /// <summary>
    /// Collision with camera and walls
    /// </summary>
    private void HandleCameraCollision()
    {
        float followPos = defaultPos;
        RaycastHit hit;

        Vector3 direction = cameraPos.position - cameraPivotObject.position;
        direction.Normalize();

        // if we hit something (like a wall)
        if (Physics.SphereCast(cameraPivotObject.transform.position, collisionRadius, direction, out hit, Mathf.Abs(followPos), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivotObject.position, hit.point);
            followPos =- (distance - cameraCollisionOffset);
        }

        if(Mathf.Abs(followPos) < minCollisionOffset)
        {
            followPos = followPos - minCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraPos.localPosition.z, followPos, 0.2f);
        cameraPos.localPosition = cameraVectorPosition;
    }
}
