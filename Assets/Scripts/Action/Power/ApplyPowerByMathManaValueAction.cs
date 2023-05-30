using NueGames.Action;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace Action
{
    /// <summary>
    /// 根據數學瑪娜數量，給予狀態
    /// </summary>
    public class ApplyPowerByMathManaValueAction : GameActionBase
    {
        public override GameActionType ActionType => GameActionType.ApplyPowerByMathManaValue;


        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                MultiplierAmount = CombatManager.CurrentMainAlly.GetPowerValue(PowerType.MathMana);
                target.CharacterStats.ApplyPower(PowerType, AdditionValue);
            }
        }
    }
}