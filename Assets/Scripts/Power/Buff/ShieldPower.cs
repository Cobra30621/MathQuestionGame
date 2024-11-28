using System.Collections.Generic;
using Action.Parameters;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{


    public class ShieldPower : PowerBase
    {
        public override PowerName PowerName => PowerName.Shield;

        public ShieldPower()
        {

        }

        public override void SubscribeAllEvent()
        {
            CombatManager.OnTurnEnd += OnTurnEnd;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnTurnEnd -= OnTurnEnd;
        }

        protected override void OnTurnEnd(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                
                GameActionExecutor.AddAction(new ApplyPowerAction(
                    3 * Amount, PowerName.Block, new List<CharacterBase>() {Owner},
                    GetActionSource()));
            }
        }
    }
}