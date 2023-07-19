using NueGames.Action;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Power;
using UnityEngine;

namespace Action
{
    /// <summary>
    /// 根據數學瑪娜數量，給予狀態
    /// </summary>
    public class ApplyPowerByMathManaValueAction : GameActionBase
    {
        public override ActionName ActionName => ActionName.ApplyPowerByMathManaValue;


        protected override void DoMainAction()
        {
            foreach (var target in TargetList)
            {
                Parameters.MultiplierAmount = CombatManager.CurrentMainAlly.GetPowerValue(PowerName.MathMana);
                target.CharacterStats.ApplyPower(ActionData.powerName, AdditionValue);
            }
        }
    }
}