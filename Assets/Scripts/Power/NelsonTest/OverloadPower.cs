using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{

    public class OverloadPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Overload;


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
            return rawValue - 1;
        }
    }
}