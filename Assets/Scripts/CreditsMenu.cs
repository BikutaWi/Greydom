using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public void Back()
    {
        mainMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
