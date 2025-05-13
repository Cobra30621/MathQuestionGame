using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Knight
{
    public class SwordRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Sword;
  
        public override int GainMaxMana(int rawValue)
        {
            return rawValue - 1;
        }

        public override void OnBattleStart()
        {
            if (IsMaxLevel())
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    5, PowerName.Strength, new List<CharacterBase>(){MainAlly}, GetEffectSource()));
            }
            else
            {
                EffectExecutor.AddEffect(new ApplyPowerEffect(
                    3, PowerName.Strength, new List<CharacterBase>(){MainAlly}, GetEffectSource()));
            }
        }
    }
}