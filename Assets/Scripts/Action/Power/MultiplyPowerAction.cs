using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;


namespace NueGames.Action
{
    public class MultiplyPowerAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.MultiplyPower;
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        public override void DoAction()
        {
            if (IsTargetNull()) return;
            
            Target.CharacterStats.MultiplyPower(PowerType, AdditionValue);
        }
    }
}