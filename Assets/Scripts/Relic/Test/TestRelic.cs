using System.Collections.Generic;
using Characters;
using Effect.Damage;
using Effect.Parameters;
using Effect.Power;
using NSubstitute;
using Power;
using Relic.Data;
using UnityEngine;

namespace Relic.Test
{
    public class TestRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Test;

        // 上一個攻擊的敵人們
        private List<CharacterBase> preTargets;

        public override void OnAttack(DamageInfo info, List<CharacterBase> targets)
        {
            
            // 玩家連續攻擊同一個敵人，給予易傷
            var sourceCharacter = info.EffectSource.SourceCharacter;
            if (sourceCharacter != null && sourceCharacter.IsCharacterType(CharacterType.Ally))
            {
                foreach (var target in targets)
                {
                    if (preTargets.Contains(target))
                    {
                        var applyPowerEffect = new ApplyPowerEffect(1, PowerName.Vulnerable,
                            new List<CharacterBase>() { target }, GetEffectSource());
                        applyPowerEffect.Play();
                    }
                }

                preTargets = targets;
            }
        }

        public override void OnDead(DamageInfo info)
        {
            // 是敵人，給予所有敵人當前血量的傷害
            var target = info.Target;

            Debug.Log("死亡造成全體 5 點傷害");
            if (target.IsCharacterType(CharacterType.Enemy))
            {
                var damageInfo = new DamageInfo(5, GetEffectSource(), fixDamage: true);
                var targets = CombatManager.EnemiesForTarget();
                targets.Remove(target);

                var damageEffect = new DamageEffect(damageInfo, targets);
                damageEffect.Play();
            }
        }
    }
}