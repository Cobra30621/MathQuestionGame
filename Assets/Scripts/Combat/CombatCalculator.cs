using System.Collections;
using System.Collections.Generic;
using Kalkatos.DottedArrow;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Power;
using UnityEngine;
using CombatManager = NueGames.Managers.CombatManager;

namespace NueGames.Combat
{
    /// <summary>
    /// 計算戰鬥的數值(傷害、格檔)
    /// </summary>
    public static class CombatCalculator
    {
        private static CombatManager CombatManager => CombatManager.Instance;
        /// <summary>
        /// 易傷加乘
        /// </summary>
        private static readonly float vulnerableValue = 1.5f;
        /// <summary>
        /// 虛弱加成
        /// </summary>
        private static readonly float weakValue = 0.75f;
        private static CharacterBase _targetEnemy;
        
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        /// <param name="rawValue"></param>
        /// <param name="selfCharacter"></param>
        /// <returns></returns>
        public static int GetDamageValue(float rawValue, CharacterBase selfCharacter)
        {
            _targetEnemy = CombatManager.CurrentSelectedEnemy;
            return GetDamageValue(rawValue, selfCharacter, _targetEnemy);
        }
        
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
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

        /// <summary>
        /// 獲得格檔數值
        /// </summary>
        public static int GetBlockValue(float rawValue, CharacterBase selfCharacter)
        {
            float value = rawValue;
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
