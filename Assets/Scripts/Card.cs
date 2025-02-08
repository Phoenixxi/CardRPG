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
        public int Damage = 0;
        public int Heal = 0;
        public int Team_dmgMultiplier = 1;
        public int Incoming_dmgReducer = 1;
        public int Single_atkMultiplier = 1;
        public bool Shield = false;
        public bool Reshuffle = false;
        public int CostManipulation = 0;
        public int DOT = 0;
        public int Throns = 0;
        public Sprite cardSprite;

    // Update later
        public enum CharacterType
        {
            MainCharacter,
            Cat,
            W1_P_CombatHealer,
            W1_P_BruiserTank,
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
            IncAllyDmg,
            DecEnemyDmg,
            Shield,
            Reshuffle,
            EnergyCostManipulation,
            DmgOverTime,
            Thorns

        }

    }
}
