using NueGames.Enums;

namespace NueGames.Power
{
    /// <summary>
    /// 回合結束時，降低力量
    /// </summary>
    public class ReduceStrengthPowerAtEndOfTurnPower : ReducePowerAtEndOfTurnPower
    {
        public override PowerName PowerName => PowerName.ReduceStrengthPowerAtEndOfTurn;
        protected override PowerName TargetPowerName => PowerName.Strength;
    }
}