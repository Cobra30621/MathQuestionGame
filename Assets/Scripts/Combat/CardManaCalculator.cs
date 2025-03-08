using System.Collections.Generic;
using GameListener;
using Managers;
using UnityEngine;

namespace Combat
{
    public class CardManaCalculator : MonoBehaviour
    {
        /// <summary>
        /// 卡片初始魔力加成的計算
        /// </summary>
        /// <param name="rawValue"></param>
        /// <returns></returns>
        public static int GetCardManaCost(int rawValue)
        {
            List<CalculateOrderClip> orderClips = new List<CalculateOrderClip>();
            var selfCharacter = CombatManager.Instance.MainAlly;
            // 使用者能力加成
            if (selfCharacter != null)
            {
                foreach (var powerBase in selfCharacter.GetPowerDict().Values)
                {
                    orderClips.Add(new CalculateOrderClip(powerBase.CardManaCalculateOrder, powerBase.GetCardRawMana));
                }
            }

            // 計算遺物能力加成
            foreach (var relicBase in GameManager.Instance.RelicManager.CurrentRelicDict.Values)
            {
                orderClips.Add(new CalculateOrderClip(relicBase.CardManaCalculateOrder, relicBase.GetCardRawMana));
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
        
        
        
        
    }
}