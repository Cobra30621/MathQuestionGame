using System;
using Effect.Parameters;

namespace Power
{
    public class PowerHelper
    {
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
        /// 效果跟能力有關
        /// </summary>
        /// <param name="effectName"></param>
        /// <returns></returns>
        public static bool IsPowerRelatedEffect(EffectName effectName)
        {
            return effectName == EffectName.ApplyPower;
        }

        public static bool IsBlockRelatedEffect(EffectName effectName)
        {
            return effectName == EffectName.ApplyBlock ||
                effectName == EffectName.BlockByCount;
        }

        
    }
}