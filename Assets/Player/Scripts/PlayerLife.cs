using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    // how many hearts have the player
    public int hearts = 4;
    public int maxHeats = 4;

    [SerializeField] HeartSystem heartSystem;

    private void Awake()
    {
        heartSystem.DrawHeart(hearts, maxHeats);
    }

    public void HealPlayer(int potion)
    {
        if(hearts < maxHeats)
        {
            hearts += potion;
            heartSystem.DrawHeart(hearts, maxHeats);
        }
           
        
    }

    public void DamagePlayer(int damages)
    {
        hearts -= damages;
        heartSystem.DrawHeart(hearts, maxHeats);
    }


}
