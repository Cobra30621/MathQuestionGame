using System.Collections.Generic;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 攻擊並且當攻擊成功時，觸發某個效果
    /// </summary>
    public class DamageAndTriggerActionIfDamageSuccessAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.DamageAndTriggerActionIfDamageSuccess;

        protected override void DoMainAction()
        {
            if (IsTargetNull()) return;

            if (DamageInfo.HaveDamage())
            {
                Debug.Log($"攻擊成功，觸發{TriggerActionList}");
                DoTriggerAction();
            }
            
            
            var damageAction = new DamageAction();
            damageAction.SetValue(ActionParameters);
            
            GameActionExecutor.AddToBottom(damageAction);
        }

        /// <summary>
        /// 執行 TriggerActionList
        /// </summary>
        private void DoTriggerAction()
        {
            List<GameActionBase> gameActions =  GameActionGenerator.GetGameActions(null, 
                ActionSource.Enemy, TriggerActionList, Self, Target);
            GameActionExecutor.AddToBottom(gameActions);
        }
    }
}