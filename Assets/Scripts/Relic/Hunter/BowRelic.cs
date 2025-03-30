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
        private int damage = 10;
        public override void OnDead(DamageInfo info)
        {
            var target = info.Target;
            
            if (target.IsCharacterType(CharacterType.Enemy))
            {
                damage = IsMaxLevel() ? 35 : 10;
                var damageInfo = new DamageInfo(damage, GetEffectSource(), fixDamage: true);
                var targets = CombatManager.EnemiesForTarget();
                targets.Remove(target);
                var damageEffect = new DamageEffect(damageInfo, targets);
                damageEffect.Play();
            }
        }
       
    }
}