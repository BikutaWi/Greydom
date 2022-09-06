using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomCharacterData", menuName = "My game/Random Character Data")]
public class RandomCharacterData : ScriptableObject
{
    public string characterName;
    public string description;
    public GameObject characterModel;
    public float speed = 1.5f;
}
