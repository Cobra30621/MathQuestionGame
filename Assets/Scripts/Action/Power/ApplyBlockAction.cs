using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Characters;
using NueGames.Power;

namespace NueGames.Action
{
    public class ApplyBlockAction : GameActionBase
    {
        /// <summary>
        /// 狀態層數
        /// </summary>
        private int _applyValue;
        
        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        public ApplyBlockAction(int applyValue, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            _applyValue = applyValue;
            TargetList = targetList;
            ActionSource = actionSource;
        }

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public ApplyBlockAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
            _applyValue = skillInfo.int1;
        }
        
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.ApplyPower(PowerName.Block, _applyValue);
            }
        }
    }
}