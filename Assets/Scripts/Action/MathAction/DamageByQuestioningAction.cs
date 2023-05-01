using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using Question;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 根據答對數，造成傷害
    /// </summary>
    public class DamageByQuestioningAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByQuestioning;
        
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            
            DoDamageAction();
        }

    }
}