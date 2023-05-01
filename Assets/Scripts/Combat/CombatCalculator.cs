using System.Collections;
using System.Collections.Generic;
using Kalkatos.DottedArrow;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
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
        private static GameManager GameManager => GameManager.Instance;
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
        public static int GetDamageValue(DamageInfo info)
        {
            if (info.FixDamage)
            {
                return info.Value;
            }
            
            return GetDamageValue(info.Value, info.Self, info.Target);
        }
        
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        private static int GetDamageValue(float rawValue, CharacterBase selfCharacter, CharacterBase targetCharacter)
        {
            float value = rawValue;
            // 計算使用者能力加成
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                // TODO 力量、虛弱計算要分開
                value = powerBase.AtDamageGive(value);
            }

            // 計算目標對象能力加成
            if (targetCharacter != null)
            {
                foreach (PowerBase powerBase in targetCharacter.GetPowerDict().Values)
                {
                    value = powerBase.AtDamageReceive(value);
                }
            }
            
            bool selfIsAlly = selfCharacter.CharacterType == CharacterType.Ally;
            // 計算遺物能力加成
            foreach (var relicClip in GameManager.PersistentGameplayData.CurrentRelicList)
            {
                if (selfIsAlly)// 傷害發起者是玩家，遺物給予傷害加成
                {
                    value = relicClip.Relic.AtDamageGive(value);
                }
                else // 攻擊對象是敵人，遺物給予受到傷害加成
                {
                    value = relicClip.Relic.AtDamageReceive(value);
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
            // 計算能力加成
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                value = powerBase.ModifyBlock(value);
            }
            
            // 計算遺物能力加成
            bool selfIsAlly = selfCharacter.CharacterType == CharacterType.Ally;
            foreach (var relicClip in GameManager.PersistentGameplayData.CurrentRelicList)
            {
                if (selfIsAlly)// 格檔發起者是玩家，遺物給予加成
                {
                    value = relicClip.Relic.ModifyBlock(value);
                }
            }
            
            // 計算能力加成
            foreach (PowerBase powerBase in selfCharacter.GetPowerDict().Values)
            {
                value = powerBase.ModifyBlockLast(value);
            }
            
            // 計算遺物能力加成
            foreach (var relicClip in GameManager.PersistentGameplayData.CurrentRelicList)
            {
                if (selfIsAlly)// 格檔發起者是玩家，遺物給予加成
                {
                    value = relicClip.Relic.ModifyBlockLast(value);
                }
            }

            return Mathf.RoundToInt(value);
        }

        
    }

}
