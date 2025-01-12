using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Parameters;
using Effect.Power;

namespace Power.Buff
{
    /// <summary>
    /// 強化
    /// </summary>
    public class StrengthAfterDeadPower : PowerBase
    {
        public override PowerName PowerName => PowerName.StrengthAfterDead;

        public StrengthAfterDeadPower()
        {
            
        }
        public override void SubscribeAllEvent()
        {
            Owner.OnDeath += OnDead;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.OnDeath -= OnDead;
        }
        
        protected override void OnDead(DamageInfo damageInfo)
        {
            List<CharacterBase> targets = new List<CharacterBase>();
            var allEnemy = CombatManager.Instance.Enemies;
            targets.AddRange(allEnemy);

            // 對全體敵方單位施加強化
            EffectExecutor.AddEffect(
                new ApplyPowerEffect(1, PowerName.Strength, 
                    targets, GetActionSource()));
        }
    }
}