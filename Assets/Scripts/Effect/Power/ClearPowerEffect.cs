using System.Collections.Generic;
using Characters;
using Effect.Parameters;
using Power;

namespace Effect.Power
{
    /// <summary>
    /// 清除能力
    /// </summary>
    public class ClearPowerEffect : EffectBase
    {
        private readonly PowerName _targetPower;

        
        /// <summary>
        /// 內部系統使用
        /// </summary>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="effectSource"></param>
        public ClearPowerEffect(PowerName powerName, 
            List<CharacterBase> targetList, EffectSource effectSource)
        {
            _targetPower = powerName;
            TargetList = targetList;
            EffectSource = effectSource;
        }
        
        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public ClearPowerEffect(SkillInfo skillInfo)
        {
            _targetPower =  PowerHelper.GetPowerName(skillInfo.EffectParameterList[0]);
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void Play()
        {
            foreach (var target in TargetList)
            {
                target.ClearPower(_targetPower);
            }
        }
    }
}