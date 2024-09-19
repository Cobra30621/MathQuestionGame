using Action.Parameters;
using Card;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Combat;

namespace NueGames.Action

{
    public class FleeAction : GameActionBase
    {
        /// <summary>
        /// 內部系統用
        /// </summary>
        /// <param name="applyValue"></param>
        /// <param name="powerName"></param>
        /// <param name="targetList"></param>
        /// <param name="actionSource"></param>
        public FleeAction()
        {
            
        }

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
            // 無敵
            if (CombatManager.CurrentCombatStateType == CombatStateType.AllyTurn)
                CombatManager.EndTurn();
        }
    }
}