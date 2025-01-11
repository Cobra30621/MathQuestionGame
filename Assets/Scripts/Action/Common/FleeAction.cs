using Combat;
using NueGames.Enums;
using Power;

namespace Action.Common

{
    public class FleeAction : GameActionBase
    {
        private int _applyValue;
        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>

        /// <summary>
        /// 讀表用的建構值
        /// </summary>
        /// <param name="skillInfo"></param>
        public FleeAction(SkillInfo skillInfo)
        {
            SkillInfo = skillInfo;
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            _applyValue = 1;
            foreach (var target in TargetList)
            {
                target.ApplyPower(PowerName.Invincible, _applyValue);
            }
            if (CombatManager.CurrentCombatStateType == CombatStateType.AllyTurn)
                CombatManager.EndTurn();
        }
    }
}