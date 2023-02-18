using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Combat
{
    public static class CombatCalculator
    {
        private static readonly float vulnerableValue = 1.5f;
        private static readonly float weakValue = 0.75f;
        public static int GetDamageValue(float rawValue, CharacterBase selfCharacter, CharacterBase targetCharacter)
        {
            // 力量 Strength
            float value = rawValue + selfCharacter.CharacterStats.StatusDict[StatusType.Strength].StatusValue;

            // 易傷(Vulnerable)
            if (targetCharacter.CharacterStats.StatusDict[StatusType.Vulnerable].IsActive)
            {
                value *= vulnerableValue;
            }
            
            // 虛弱(Weak)
            if (selfCharacter.CharacterStats.StatusDict[StatusType.Weak].IsActive)
            {
                value *= weakValue;
            }

            return Mathf.RoundToInt(value);
        }
    }

}
