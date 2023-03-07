using System.Collections;
using System.Collections.Generic;
using Kalkatos.DottedArrow;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Power;
using UnityEngine;
using CombatManager = NueGames.NueDeck.Scripts.Managers.CombatManager;

namespace NueGames.NueDeck.Scripts.Combat
{
    public static class CombatCalculator
    {
        private static CombatManager CombatManager => CombatManager.Instance;
        private static readonly float vulnerableValue = 1.5f;
        private static readonly float weakValue = 0.75f;
        private static CharacterBase _targetEnemy;
        
        public static int GetDamageValue(float rawValue, CharacterBase selfCharacter)
        {
            _targetEnemy = CombatManager.CurrentSelectedEnemy;
            return GetDamageValue(rawValue, selfCharacter, _targetEnemy);
        }
        
        public static int GetDamageValue(float rawValue, CharacterBase selfCharacter, CharacterBase targetCharacter)
        {
            float value = rawValue;
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                value = powerBase.AtDamageGive(value);
            }

            if (targetCharacter != null)
            {
                foreach (PowerBase powerBase in targetCharacter.GetPowerDict().Values)
                {
                    value = powerBase.AtDamageReceive(value);
                }
            }

            return Mathf.RoundToInt(value);
        }

        public static int GetBlockValue(float rawValue, CharacterBase selfCharacter)
        {
            float value = rawValue;
            Debug.Log("selfCharacter.GetPowerDict()" + selfCharacter);
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                value = powerBase.ModifyBlock(value);
            }
            
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                value = powerBase.ModifyBlockLast(value);
            }

            return Mathf.RoundToInt(value);
        }

        
    }

}
