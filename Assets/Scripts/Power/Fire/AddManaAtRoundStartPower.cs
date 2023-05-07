using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Power
{
    public class AddManaAtRoundStartPower : PowerBase
    {
        public override PowerType PowerType => PowerType.AddManaAtRoundStart;

        public AddManaAtRoundStartPower()
        {
            
        }
        
        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnStart += OnTurnStart;
        }

        protected override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnStart -= OnTurnStart;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                ClearPower();
            }
        }


        public override int AtGainTurnStartMana(int rawValue)
        {
            return rawValue + Amount;
        }
    }
}