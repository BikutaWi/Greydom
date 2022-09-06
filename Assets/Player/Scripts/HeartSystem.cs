using UnityEngine;

public class HeartSystem : MonoBehaviour
{
    [SerializeField] GameObject heartPrefab;
    [SerializeField] GameObject brokenHeartPrefab;

    public void DrawHeart(int hearts, int maxHeart)
    {
        // clean all childs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Draw heart in top of the game everytime player is hit
        for (int i = 0; i < maxHeart; i++)
        {
            if (i + 1 <= hearts)
            {
                GameObject heart = Instantiate(heartPrefab, transform.position, Quaternion.identity);
                heart.transform.parent = transform;
            }
            else
            {
                GameObject brokenHeart = Instantiate(brokenHeartPrefab, transform.position, Quaternion.identity);
                brokenHeart.transform.parent = transform;
            }
        }
    }
}
