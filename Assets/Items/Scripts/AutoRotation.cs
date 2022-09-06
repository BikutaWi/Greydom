using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public float speedRotation = 100f;
    void Update()
    {
        transform.Rotate(0, speedRotation * Time.deltaTime, 0);
    }
}
