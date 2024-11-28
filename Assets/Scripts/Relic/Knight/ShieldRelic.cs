using System.Collections.Generic;
using Action.Parameters;
using Card;
using NueGames.Characters;
using NueGames.Power;
using System.Collections.Generic;
using Combat;
using UnityEngine;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;
using NueGames.Power;

namespace NueGames.Relic.Knight
{
    public class ShieldRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Shield;
        private int amount = 3;
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
                    amount, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetActionSource()));
            }
        }
       
    }
}