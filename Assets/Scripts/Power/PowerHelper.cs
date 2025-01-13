using System;
using Effect.Parameters;

namespace Power
{
    /// <summary>
    /// 提供一些能力常見的工具
    /// </summary>
    public static class PowerHelper
    {
        /// <summary>
        /// 取得 PowerName
        /// </summary>
        /// <param name="powerIndex"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static PowerName GetPowerName(int powerIndex)
        {
            var powerName = (PowerName)powerIndex;

            if (powerName == PowerName.None)
            {
                throw new Exception($"PowerName 沒有編號 {powerIndex}");
            }

            return powerName;
        }
        
        /// <summary>
        /// 效果是否跟能力有關
        /// </summary>
        /// <param name="effectName"></param>
        /// <returns></returns>
        public static bool IsPowerRelatedEffect(EffectName effectName)
        {
            return effectName == EffectName.ApplyPower;
        }

        /// <summary>
        /// 效果是否跟格擋有關
        /// </summary>
        /// <param name="effectName"></param>
        /// <returns></returns>
        public static bool IsBlockRelatedEffect(EffectName effectName)
        {
            return effectName == EffectName.ApplyBlock ||
                effectName == EffectName.BlockByCount;
        }

        
    }
}