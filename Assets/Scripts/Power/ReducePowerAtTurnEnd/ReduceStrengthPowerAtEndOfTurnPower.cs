using NueGames.Enums;

namespace NueGames.Power
{
    /// <summary>
    /// 回合結束時，降低力量
    /// </summary>
    public class ReduceStrengthPowerAtEndOfTurnPower : ReducePowerAtEndOfTurnPower
    {
        public override PowerType PowerType => PowerType.ReduceStrengthPowerAtEndOfTurn;
        protected override PowerType TargetPowerType => PowerType.Strength;
    }
}