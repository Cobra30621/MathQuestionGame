using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;

namespace NueGames.Action
{
    /// <summary>
    /// 根據玩家的能力層數，給予傷害
    /// </summary>
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageByAllyPowerValue;


        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            MultiplierAmount = CombatManager.CurrentMainAlly.GetPowerValue(PowerType);
            
            DoDamageAction();
        }
    }
}