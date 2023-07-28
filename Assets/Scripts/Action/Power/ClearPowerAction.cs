using System.Collections.Generic;
using Action.Parameters;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Power;
using UnityEngine;


namespace NueGames.Action
{
    /// <summary>
    /// 清除能力
    /// </summary>
    public class ClearPowerAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ClearPower;

        public void SetValue(PowerName powerName, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            SetPowerActionValue(0, powerName, targetList, actionSource);
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.ClearPower(ActionData.powerName);
            }
        }
    }
}