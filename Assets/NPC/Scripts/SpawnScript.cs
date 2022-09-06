using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnScript : MonoBehaviour
{
    public GameObject princess;
    public int nbPrincess = 10;

    public GameObject man;
    public int nbMan = 5;

    public GameObject fairy;
    public int nbFairy = 3;

    public GameObject blacksmith;
    public int nbBlacksmith = 3;

    private List<GameObject> allPrincess;
    private List<GameObject> allMan;
    private List<GameObject> allFairy;
    private List<GameObject> allBlacksmith;

    public float spawnInterval = 2f;
    public float minDistance = 0;
    public float maxDistance = 1000f;
    private Vector3 m_position;
    private float m_distance;
    private RaycastHit Hit;
    private float timeToChangeDirection;

    public GameObject spawnPosition;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        allPrincess = new List<GameObject>();
        allMan = new List<GameObject>();
        allFairy = new List<GameObject>();
        allBlacksmith = new List<GameObject>();

        for (int i = 0; i < nbPrincess; i++)
        {
            InitNp(princess);
            yield return new WaitForSeconds(spawnInterval);
        }


        for (int i = 0; i < nbMan; i++)
        {
            InitNp(man);
            yield return new WaitForSeconds(spawnInterval);
        }

        for (int i = 0; i < nbFairy; i++)
        {
            InitNp(fairy);
            yield return new WaitForSeconds(spawnInterval);
        }


        for (int i = 0; i < nbBlacksmith; i++)
        {
            InitNp(blacksmith);
            yield return new WaitForSeconds(spawnInterval);
        }

        yield return new WaitForSeconds(spawnInterval);

    }

    private void InitNp(GameObject np)
    {
        GameObject go = null;

        if (m_position == null) m_position = new Vector3();

        m_distance = UnityEngine.Random.Range(minDistance, maxDistance);

        //Left
        if (UnityEngine.Random.Range(0, 2) == 0)
            m_position.Set(-m_distance, 0f, m_distance);
        //Right
        else
            m_position.Set(m_distance, 0f, m_distance); 


        switch (np.tag)
        {
            case "princess":
                go = Instantiate(princess, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                go.SetActive(true);
                allPrincess.Add(go);
                break;

            case "fairy":
                go = Instantiate(fairy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                go.SetActive(true);
                allFairy.Add(go);
                break;

            case "man":
                go = Instantiate(man, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                go.SetActive(true);
                allMan.Add(go);
                break;

            case "blacksmith":
                go = Instantiate(blacksmith, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                go.SetActive(true);
                allBlacksmith.Add(go);
                break;

            case null:
                go = Instantiate(man, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                go.SetActive(true);
                allMan.Add(go);
                break;
        }

        go.transform.position = spawnPosition.transform.position + transform.TransformVector(m_position);
        go.transform.RotateAround(transform.position, transform.up, Time.deltaTime * 360f);

        go.transform.rotation = Quaternion.Slerp(go.transform.rotation, Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(-150, 150)), Time.deltaTime * 360f); 

    }



    // Update is called once per frame
    void Update()
    {
        timeToChangeDirection -= Time.deltaTime;

        if (timeToChangeDirection <= 0)
        {
            ChangeDirection();
        } 
    }


    private void ChangeDirection()
    {

        foreach (GameObject go in allPrincess)
        {
            RandomDirection(go);
        }

        foreach (GameObject go in allMan)
        {
            RandomDirection(go);
        }

        foreach (GameObject go in allFairy)
        {
            RandomDirection(go);
        }

        foreach (GameObject go in allBlacksmith)
        {
            RandomDirection(go);
        }

        timeToChangeDirection = 1f;
    }

    private void RandomDirection(GameObject go)
    {
        if (Physics.Raycast(go.transform.position, go.transform.TransformDirection(Vector3.forward), out Hit, 2))
        {
            go.transform.rotation = Quaternion.Slerp(go.transform.rotation, Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(-150, 150)), Time.deltaTime * 360f);
        }
    }
}
