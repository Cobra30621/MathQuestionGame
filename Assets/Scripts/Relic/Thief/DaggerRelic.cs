using System.Collections.Generic;
using Characters;
using Effect.Parameters;
using Effect;
using Relic.Data;
using Effect.Power;
using Power;

namespace Relic.Thief
{
    public class DaggerRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Dagger;
        public override void OnAttack(DamageInfo info, List<CharacterBase> targets)
        {
            if (info.EffectSource.SourceCharacter == null)
            {
                return;
            }
            
            // 攻擊者為玩家
            if (info.EffectSource.SourceCharacter.IsCharacterType(CharacterType.Ally))
            {
                if (IsMaxLevel())
                {
                    EffectExecutor.AddEffect(new ApplyPowerEffect(
                        3, PowerName.Poison, targets,
                        GetEffectSource()));
                }
                else
                {
                    EffectExecutor.AddEffect(new ApplyPowerEffect(
                        1, PowerName.Poison, targets,
                        GetEffectSource()));
                }
            }
            
        }
    }
}