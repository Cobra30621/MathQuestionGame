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
        private readonly PowerName _targetPower;

        
        
        public ClearPowerAction(PowerName powerName, 
            List<CharacterBase> targetList, ActionSource actionSource)
        {
            _targetPower = powerName;
            TargetList = targetList;
            ActionSource = actionSource;
        }
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.ClearPower(_targetPower);
            }
        }
    }
}