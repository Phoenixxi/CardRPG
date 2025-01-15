using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace CardNamespace
{
    // Add data to editor
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
     public class Card : ScriptableObject
    {
        public string cardName;
        public CharacterType character;
        public int energy;
        public int damage;

        public GameObject cardSprite;

    // Update later
        public enum CharacterType
        {
            Red,
            Blue,
            Green
        }

    }
}
