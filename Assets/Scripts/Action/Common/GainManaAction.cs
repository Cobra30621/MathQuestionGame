using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 獲得瑪娜
    /// </summary>
    public class GainManaAction : GameActionBase
    {
        private int _manaCount;

        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        public GainManaAction(int manaCount, ActionSource source)
        {
            _manaCount = manaCount;
            ActionSource = source;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public GainManaAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _manaCount = skillInfo.EffectParameterList[0];
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            if (CombatManager != null)
                CombatManager.AddMana(_manaCount);
            else
                Debug.LogError("There is no CombatManager");
        }
    }
}