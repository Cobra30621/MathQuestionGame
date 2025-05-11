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
            // 傷害目標對象是敵人
            if (target.IsCharacterType(CharacterType.Enemy))
            {
                // 只有傷害來自卡牌才會觸發效果
                if (info.EffectSource.SourceType == SourceType.Card)
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
}