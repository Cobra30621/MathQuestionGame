using System.Collections.Generic;
using NueGames.Enums;
using NueGames.Parameters;
using UnityEngine;

namespace NueGames.Action
{
    /// <summary>
    /// 攻擊並且當攻擊成功時，觸發某個效果
    /// </summary>
    public class DamageAndTriggerActionIfDamageSuccessAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.DamageAndTriggerActionIfDamageSuccess;

        protected override void DoMainAction()
        {
            // TODO : 將功能依據新架構實作
            // if (damageInfo.HaveDamage())
            // {
            //     Debug.Log($"攻擊成功，觸發{ActionData.TriggerActionList}");
            //     DoTriggerAction();
            // }
            //
            //
            var damageAction = new DamageAction();
            damageAction.SetValue(Parameters);
            
            GameActionExecutor.AddToBottom(damageAction);
        }

        /// <summary>
        /// 執行 TriggerActionList
        /// </summary>
        private void DoTriggerAction()
        {
            
            List<GameActionBase> gameActions =  GameActionGenerator.GetGameActions(null, 
                Parameters.ActionSource, ActionData.TriggerActionList, TargetList);
            GameActionExecutor.AddToBottom(gameActions);
        }
    }
}