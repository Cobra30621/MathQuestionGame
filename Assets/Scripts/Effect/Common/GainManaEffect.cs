using Effect.Parameters;
using UnityEngine;

namespace Effect.Common
{
    /// <summary>
    /// 獲得瑪娜
    /// </summary>
    public class GainManaEffect : EffectBase
    {
        private int _manaCount;

        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        public GainManaEffect(int manaCount, EffectSource source)
        {
            _manaCount = manaCount;
            EffectSource = source;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public GainManaEffect(SkillInfo skillInfo)
        {
            _manaCount = skillInfo.EffectParameterList[0];
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void Play()
        {
            if (CombatManager != null)
                CombatManager.AddMana(_manaCount);
            else
                Debug.LogError("There is no CombatManager");
        }
    }
}