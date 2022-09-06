using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursor : MonoBehaviour
{
    bool isCursorVisible = true;

    private void Start()
    {
        CursorVisible();
    }

    public void CursorVisible()
    {
        isCursorVisible = !isCursorVisible;

        if (isCursorVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
