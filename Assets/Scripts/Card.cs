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

        // make energy NOT displayed to set values back to original on start
        public int Energy;
        public int EnergyDecreasedBy = 0;
        public float Damage = 0f;
        public float Heal = 0f;
        public float Team_dmgMultiplier = 1f;
        public float Incoming_dmgReducer = 1f;
        public int Single_atkAdder = 0;
        public int DiceManipulationAmount = 0;
        public bool Shield = false;
        public bool Reshuffle = false;
        public bool CostManipulation = false;
        public bool Thorns = false;
        public float DOT = 0f;
        public int diceRollManipulation = 0;
        public Sprite cardSprite;
        public string CharacterPosition;
        public GameObject vfxProjectile;
        public GameObject vfxHeal;
        public GameObject vfxBuff;
        public GameObject vfxRainFall;
        public GameObject vfxImpact;

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
            DiceManipulation,
            DmgOverTime,
            Thorns,
            CostManipulation,
            KingFireBlastSpecial,
            BellaSpecial

        }

    }
}
