using System;
using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using GameListener;
using Kalkatos.DottedArrow;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using Sirenix.Utilities;
using UnityEngine;

namespace NueGames.Combat
{
    /// <summary>
    /// 計算戰鬥的數值(傷害、格檔)
    /// </summary>
    public static class CombatCalculator
    {
        private static CombatManager CombatManager => CombatManager.Instance;
        private static GameManager GameManager => GameManager.Instance;
        private static CharacterBase _targetEnemy;
   
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        public static int GetDamageValue(DamageInfo info)
        {
            if (info.FixDamage)
            {
                return  Mathf.RoundToInt(info.damageValue);
            }
            
            return GetDamageValue(info.damageValue, info.ActionSource.SourceCharacter, info.Target);
        }
        
        
        
        
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        private static int GetDamageValue(float rawValue, CharacterBase selfCharacter, CharacterBase targetCharacter)
        {
            List<CalculateOrderClip> orderClips = new List<CalculateOrderClip>();
            // 使用者能力加成
            foreach (var powerBase in selfCharacter.GetPowerDict().Values)
            {
                orderClips.Add(new CalculateOrderClip(powerBase.DamageCalculateOrder, powerBase.AtDamageGive));
            }
            
            // 目標能力加成
            if (targetCharacter != null)
            {
                foreach (var powerBase in targetCharacter.GetPowerDict().Values)
                {
                    orderClips.Add(new CalculateOrderClip(powerBase.DamageCalculateOrder, powerBase.AtDamageReceive));
                }
            }

            bool selfIsAlly = selfCharacter.IsCharacterType(CharacterType.Ally);
            // 計算遺物能力加成
            foreach (var relicClip in GameManager.PersistentGameplayData.CurrentRelicList)
            {
                if (selfIsAlly)
                {
                    orderClips.Add(new CalculateOrderClip(relicClip.Relic.DamageCalculateOrder, relicClip.Relic.AtDamageGive));
                }
                else
                {
                    orderClips.Add(new CalculateOrderClip(relicClip.Relic.DamageCalculateOrder, relicClip.Relic.AtDamageReceive));
                }
            }
            
            // 依據傷害計算順序，進行排序
            orderClips.Sort(new CalculateOrderComparer());
            
            
            float value = rawValue;
            foreach (var orderClip in orderClips)
            {
                value = orderClip.CalculateFunction(value);
            }
            
            return Mathf.RoundToInt(value);
        }

        /// <summary>
        /// 獲得格檔數值
        /// </summary>
        public static int GetBlockValue(float rawValue, CharacterBase selfCharacter)
        {
            List<CalculateOrderClip> orderClips = new List<CalculateOrderClip>();
            // 使用者能力加成
            foreach (var powerBase in selfCharacter.GetPowerDict().Values)
            {
                orderClips.Add(new CalculateOrderClip(powerBase.BlockCalculateOrder, powerBase.ModifyBlock));
            }
            

            bool selfIsAlly = selfCharacter.IsCharacterType(CharacterType.Ally);
            // 計算遺物能力加成
            foreach (var relicClip in GameManager.PersistentGameplayData.CurrentRelicList)
            {
                if (selfIsAlly) // 格檔發起者是玩家，遺物給予加成
                {
                    orderClips.Add(new CalculateOrderClip(relicClip.Relic.BlockCalculateOrder, relicClip.Relic.ModifyBlock));
                }
            }
            
            // 依據計算順序，進行排序
            orderClips.Sort(new CalculateOrderComparer());
            
            float value = rawValue;
            foreach (var orderClip in orderClips)
            {
                value = orderClip.CalculateFunction(value);
            }
            
            return Mathf.RoundToInt(value);
        }

        
    }

}
