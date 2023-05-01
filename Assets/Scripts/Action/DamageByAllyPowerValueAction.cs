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
        private PowerType accordingPowerType;
        
        public DamageByAllyPowerValueAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            CheckHasSetValue();
            baseValue = CombatManager.CurrentMainAlly.GetPowerValue(accordingPowerType);
            
            DoDamageAction();
        }
    }
}