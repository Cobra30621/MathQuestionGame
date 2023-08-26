using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;

namespace NueGames.Power
{
    /// <summary>
    /// 回合開始時，獲得 Amount 層瑪娜
    /// </summary>
    public class GainManaAtRoundStartPower : PowerBase
    {
        public override PowerName PowerName => PowerName.GainManaAtRoundStart;

        public GainManaAtRoundStartPower()
        {
            
        }

        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                Owner.ClearPower(PowerName);
            }
        }


        public override int AtGainTurnStartMana(int rawValue)
        {
            return rawValue + Amount;
        }
    }
}