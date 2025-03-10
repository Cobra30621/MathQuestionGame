using Combat;
using Relic.Data;
using UnityEngine;
using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Combat.Card;
using Effect.Card;
using Relic.Data;
using Effect.Power;
using Power;
using Relic.Data;
namespace Relic.General
{
    public class BottleRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bottle;

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    5, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetEffectSource()));
                var drawCardEffect = new DrawCardEffect(1, GetEffectSource());
                drawCardEffect.Play();
            }
            
        }
       
    }
}