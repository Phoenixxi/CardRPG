using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace CardNamespace 
{
    // Add data to editor
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
     public class Card : ScriptableObject
    {
        public string cardName;
        public CharacterType character;
        public CardType cardType;

        public int Energy;
        public float Damage = 0f;
        public float Heal = 0f;
        public float Team_dmgMultiplier = 1f;
        public float Incoming_dmgReducer = 1f;
        public float Single_atkAdder = 1f;
        public bool Shield = false;
        public bool Reshuffle = false;
        public int CostManipulation = 0;
        public float DOT = 0f;
        public float Thorns = 0f;
        public int diceRollManipulation = 0;
        public Sprite cardSprite;
        public GameObject vfxPrefab;

    // Update later
        public enum CharacterType
        {
            Mewa,
            Eou,
            BellaBora,
            KingFireBlast,
            W1_G_DPS,
            W2_P_Buffer,
            W2_P_Debuffer,
            W2_G_Healer,
            W3_P_DOT,
            W3_P_ScalableDPS,

        }

        public enum CardType
        {
            Damage,
            TeamHeal,
            TeamDmgMultiplier,
            DecEnemyDmg,
            SingleAtkAdder,
            Shield,
            Reshuffle,
            EnergyCostManipulation,
            DmgOverTime,
            Thorns,
            DiceManipulation

        }

    }
}
