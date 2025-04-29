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
        public string character;
        public int characterID;
        public CardType cardType;

        // make energy NOT displayed to set values back to original on start
        public int Energy;
        public int EnergyDecreasedBy = 0;
        public float Damage = 0f;
        public float Heal = 0f;
        public float Team_dmgMultiplier = 1f;
        public float DecEnemyDmg = 1f;
        public int Single_atkAdder = 0;
        public int DiceManipulationAmount = 0;
        public bool Shield = false;
        public bool Reshuffle = false;
        public bool ReshuffleElio = false;
        public bool CostManipulation = false;
        public bool Thorns = false;
        public float thornsPercent = 0f;
        public int diceRollManipulation = 0;
        public Sprite cardSprite;
        public string CharacterPosition;
        public GameObject vfxProjectile;
        public GameObject vfxHeal;
        public GameObject vfxBuff;
        public GameObject vfxRainFall;
        public GameObject vfxUnderEnemy;
        public GameObject vfxImpact;
        

    // Update later
        

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
            Freeze,
            KingFireBlastSpecial,
            BellaSpecial,
            SviurMaiden,
            SviurGeneral,
            ReshuffleElio

        }

    }
}
