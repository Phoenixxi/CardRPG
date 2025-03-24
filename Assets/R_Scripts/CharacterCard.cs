using System.Collections;
using System.Collections.Generic;
using CardNamespace;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Card", menuName = "Character Card")]
public class CharacterCard : ScriptableObject
{
    public Sprite sprite;
    public List<Card> cards;
    public bool unlocked;
    public int ID;
}
