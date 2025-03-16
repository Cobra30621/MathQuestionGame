using Combat.Card;
using Relic.Data;
using Effect.Condition;
using Combat;
using System.Collections.Generic;
using UnityEngine;
using Effect;
using Effect.Power;
using Power;
using Characters;

namespace Relic.General
{
    public class OldWarriorsBracerRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.OldWarriorsBracer;
        private bool isUsedThisTurn = false;
        
        public override void OnBattleStart()
        {
            isUsedThisTurn = false;
        }
        
        public override void OnTurnStart(TurnInfo info)
        {
            isUsedThisTurn = false;
        }
        
        public override void OnUseCard(BattleCard card)
        {
            // Check if the effect hasn't been used this turn and if 3 or more cards have been played
            if (!isUsedThisTurn && ConditionChecker.UseEnoughCard(2))
            {   
                // Apply 2 block to the player using EffectExecutor
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    2, PowerName.Block, new List<CharacterBase>() {MainAlly},
                    GetEffectSource()));
                
                // Mark as used for this turn
                isUsedThisTurn = true;
            }
        }
    }
}