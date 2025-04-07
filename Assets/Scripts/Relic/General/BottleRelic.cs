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
        private int block = 5;
        private int drawCount = 1;

        public override void OnTurnStart(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                block = IsMaxLevel()? 12 : 5;
                drawCount = IsMaxLevel()? 2 : 1;
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    block, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetEffectSource()));
                var drawCardEffect = new DrawCardEffect(drawCount, GetEffectSource());
                drawCardEffect.Play();
            }
            
        }
       
    }
}