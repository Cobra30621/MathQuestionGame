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
    /// 賦予能力
    /// </summary>
    public class ApplyPowerAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyPower;


        public ApplyPowerAction(int applyValue, PowerName powerName, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            SetPowerActionValue(applyValue, powerName, targetList, actionSource);
        }
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.ApplyPower(ActionData.powerName, AdditionValue);
            }
            
        }
    }
}