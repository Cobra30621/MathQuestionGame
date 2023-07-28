using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;

namespace NueGames.Action
{
    /// <summary>
    /// 根據玩家的能力層數，給予傷害
    /// </summary>
    public class DamageByAllyPowerValueAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.DamageByAllyPowerValue;
        
        /// <summary>
        /// 設定數值
        /// </summary>
        /// <param name="multiplierAmount">給予能力 x 倍的傷害</param>
        /// <param name="targetList"></param>
        /// <param name="powerName"></param>
        /// <param name="actionSource"></param>
        /// <param name="fixDamage"></param>
        /// <param name="canPierceArmor"></param>
        public DamageByAllyPowerValueAction(int multiplierAmount, List<CharacterBase> targetList, PowerName powerName, 
            ActionSource actionSource, bool fixDamage  = false, bool canPierceArmor  = false)
        {
            SetDamageActionValue(0, targetList, actionSource, fixDamage, canPierceArmor);
            ActionData.powerName = powerName;
            ActionData.MultiplierValue = multiplierAmount;
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            Parameters.MultiplierAmount = CombatManager.CurrentMainAlly.GetPowerValue(ActionData.powerName);
            
            GameActionExecutor.AddToBottom(new DamageAction(
                AdditionValue, TargetList, Parameters.ActionSource));
        }
    }
}