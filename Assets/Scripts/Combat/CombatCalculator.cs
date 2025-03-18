using System.Collections.Generic;
using Characters;
using Effect.Parameters;
using GameListener;
using Managers;
using UnityEngine;

namespace Combat
{
    /// <summary>
    /// 計算戰鬥的數值(傷害、格檔)
    /// </summary>
    public static class CombatCalculator
    {
        private static CharacterBase _targetEnemy;
   
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        public static int GetDamageValue(DamageInfo info)
        {
            if (info.FixDamage)
            {
                return  Mathf.RoundToInt(info.DamageValue);
            }
            
            return GetDamageValue(info.DamageValue, info.EffectSource.SourceCharacter, info.Target);
        }
        
        
        
        
        /// <summary>
        /// 獲得戰鬥傷害數值
        /// </summary>
        private static int GetDamageValue(float rawValue, CharacterBase selfCharacter, CharacterBase targetCharacter)
        {
            List<CalculateOrderClip> orderClips = new List<CalculateOrderClip>();
            // 使用者能力加成
            if (selfCharacter != null)
            {
                foreach (var powerBase in selfCharacter.GetPowerDict().Values)
                {
                    orderClips.Add(new CalculateOrderClip(powerBase.DamageCalculateOrder, powerBase.AtDamageGive));
                }
            }
            
            // 目標能力加成
            if (targetCharacter != null)
            {
                foreach (var powerBase in targetCharacter.GetPowerDict().Values)
                {
                    orderClips.Add(new CalculateOrderClip(powerBase.DamageCalculateOrder, powerBase.AtDamageReceive));
                }
            }

            if (selfCharacter != null)
            {
                bool selfIsAlly = selfCharacter.IsCharacterType(CharacterType.Ally);
                // 計算遺物能力加成
                foreach (var relicBase in GameManager.Instance.RelicManager.CurrentRelicDict.Values)
                {
                    if (selfIsAlly)
                    {
                        orderClips.Add(new CalculateOrderClip(relicBase.DamageCalculateOrder, relicBase.AtDamageGive));
                    }
                    else
                    {
                        orderClips.Add(new CalculateOrderClip(relicBase.DamageCalculateOrder, relicBase.AtDamageReceive));
                    }
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


            if (selfCharacter != null)
            {
                bool selfIsAlly = selfCharacter.IsCharacterType(CharacterType.Ally);
                // 計算遺物能力加成
                foreach (var relicBase in GameManager.Instance.RelicManager.CurrentRelicDict.Values)
                {
                    if (selfIsAlly) // 格檔發起者是玩家，遺物給予加成
                    {
                        orderClips.Add(new CalculateOrderClip(relicBase.BlockCalculateOrder, relicBase.ModifyBlock));
                    }        
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

        /// <summary>
        /// 獲得加成後的瑪娜
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static int GetManaValue(int rawValue)
        {
            int gainValue = rawValue;
            var allyPowers = CombatManager.Instance.MainAlly.GetPowerDict();
            
            // 能力系統瑪娜加成
            foreach (var (key, value) in allyPowers)
            {
                gainValue = value.AtGainTurnStartMana(gainValue);
            }

            // 遺物系統瑪娜加成
            var relics = GameManager.Instance.RelicManager.CurrentRelicDict.Values;
            foreach (var relicBase in relics)
            {
                gainValue = relicBase.AtGainTurnStartMana(gainValue);
            }

            return gainValue;
        }
        
        /// <summary>
        /// 取得回合開始時，抽卡數量的加成
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static int GetDrawCountValue(int rawValue)
        {
            int gainValue = rawValue;
            var allyPowers = CombatManager.Instance.MainAlly.GetPowerDict();
            
            // 能力系統瑪娜加成
            foreach (var (key, value) in allyPowers)
            {
                gainValue = value.AtGainTurnStartDraw(gainValue);
            }

            // 遺物系統瑪娜加成
            var relics = GameManager.Instance.RelicManager.CurrentRelicDict.Values;
            foreach (var relicBase in relics)
            {
                gainValue = relicBase.AtGainTurnStartDraw(gainValue);
            }

            return gainValue;
        }

        
    }

}
