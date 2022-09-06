using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulleDialogScript : MonoBehaviour
{
    public GameObject Bulle;

    void OnTriggerEnter()
    {
        Bulle.SetActive(true);
    }

    void OnTriggerExit()
    {
        Bulle.SetActive(false);
    }
}
