using NueGames.Card;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;


namespace NueGames.Action
{
    public class MultiplyPowerAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.MultiplyPower;
        
        
        /// <summary>
        /// 執行遊戲行為的功能
        /// </summary>
        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                target.CharacterStats.MultiplyPower(ActionData.powerName, AdditionValue);
                
            }
        }
    }
}