using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Common;
using Relic.Data;

namespace Relic.Knight
{
    public class DrumstickRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Drumstick;

        public override void OnBattleWin(int roundNumber)
        {
            if (IsMaxLevel())
            {
                EffectExecutor.AddEffect(new HealEffect(
                    14, new List<CharacterBase>() {MainAlly},
                    GetEffectSource()));
            }
            else
            {
                EffectExecutor.AddEffect(new HealEffect(
                    7, new List<CharacterBase>() {MainAlly},
                    GetEffectSource()));
            }
            
        }
       
    }
}