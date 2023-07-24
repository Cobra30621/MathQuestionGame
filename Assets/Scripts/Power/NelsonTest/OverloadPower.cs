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
            CombatManager.Instance.OnTurnStart += OnTurnStart;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.Instance.OnTurnStart -= OnTurnStart;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == GetOwnerCharacterType())
            {
                ClearPower();
            }
        }
        public override int AtGainTurnStartMana(int rawValue)
        {
            return rawValue - 1;
        }
    }
}