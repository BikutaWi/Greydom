using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterControl : MonoBehaviour
{
    private float timeToChangeDirection;
    private RaycastHit Hit;
    
    void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            Vector3 newPosition = transform.position;
            newPosition.z += animator.GetFloat("Runspeed") * Time.deltaTime;
            Vector3 deltaPos = new Vector3(0,0,1) * animator.GetFloat("Runspeed") * Time.deltaTime;
            transform.Translate(deltaPos);
        }
    }

    private void Start()
    {
        ChangeDirection();
    }

    private void Update()
    {
        timeToChangeDirection -= Time.deltaTime;

        if (timeToChangeDirection <= 0)
        {
            ChangeDirection();
        }
    }
    
    private void ChangeDirection()
    {
        timeToChangeDirection = 1.5f;
    }
}
