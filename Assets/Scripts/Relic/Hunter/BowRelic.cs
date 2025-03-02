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
    public class BowRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Bow;
        public override void OnDead(DamageInfo info)
        {
            var target = info.Target;
            
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