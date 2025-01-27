using System.Collections.Generic;
using Characters;
using Effect.Damage;
using Effect.Parameters;
using Effect;
using Combat;
using Relic.Data;
using UnityEngine;

namespace Relic.Hunter
{
    public class ArmguardRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Armguard;
        private bool isTargetSame = false;

        // 上一個攻擊的敵人們
        private List<CharacterBase> preTargets = new List<CharacterBase>();

        public override void OnTurnEnd(TurnInfo info)
        {
            if (IsCharacterTurn(info))
            {
                // 重置
                preTargets = new List<CharacterBase>();
                isTargetSame = false;
            }
        }
        public override void OnAttack(DamageInfo info, List<CharacterBase> targets)
        {
            // Skip if it’s from this relic
            if (info.EffectSource?.SourceRelic == RelicName.Armguard) 
                return;

            var sourceCharacter = info.EffectSource.SourceCharacter;
            if (sourceCharacter != null && sourceCharacter.IsCharacterType(CharacterType.Ally))
            {
                foreach (var target in targets)
                {
                    if (preTargets.Contains(target))
                    {
                        isTargetSame = true;
                    }
                } 
                preTargets = targets;
            }
            
            if (isTargetSame)
            {
                // 造成傷害
                var damageInfo = new DamageInfo(5, GetEffectSource(), fixDamage: true);
                EffectExecutor.AddEffect(new DamageEffect(damageInfo, targets));
            }
        }
       
    }
}