using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffGravity : MonoBehaviour
{
    private Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }
}
