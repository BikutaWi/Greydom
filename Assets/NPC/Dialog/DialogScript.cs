using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    public GameObject LookTarget;
    public string[] Text;
    public bool AutoScroll = true;
    public float ScrollSpeed = 0.1f;

    private TextMesh txtMesh;

    // Start is called before the first frame update
    void TextDialog()
    {
        LookTarget = GameObject.FindGameObjectWithTag("Player");
        txtMesh = transform.GetChild(0).GetComponent<TextMesh>();

        if (AutoScroll)
        {
            StartCoroutine(ShowText());
        }
        else
        {
            for (int i = 0; i < Text.Length; i++)
            {
                txtMesh.text += Text[i].Replace("|", System.Environment.NewLine);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targerPosition = new Vector3(LookTarget.transform.position.x, transform.position.y, LookTarget.transform.position.z);
        transform.LookAt(targerPosition);
    }

    IEnumerator ShowText()
    {
        for (int i = 0; i < Text.Length; i++)
        {
            string tmp = Text[i].Replace("|", System.Environment.NewLine);

            for (int j = 1; j <= tmp.Length; j++)
            {
                yield return new WaitForSeconds(ScrollSpeed);
                txtMesh.text = tmp.Substring(0, j);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    void OnEnable()
    {
        TextDialog();
    } 

}
