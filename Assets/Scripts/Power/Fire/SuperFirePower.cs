using NueGames.Combat;
using NueGames.Enums;
using NueGames.Parameters;
using UnityEngine;

namespace NueGames.Power
{
    // 如果對方被消滅時是因為燃燒，則造成全體傷害
    public class SuperFirePower : PowerBase
    {
        public override PowerType PowerType => PowerType.SuperFire;


        public override void SubscribeAllEvent()
        {
            Owner.GetCharacterStats().OnDeath += OnDead;
        }

        protected override void UnSubscribeAllEvent()
        {
            Owner.GetCharacterStats().OnDeath -= OnDead;
        }


        protected override void OnDead(DamageInfo damageInfo)
        {
            if (damageInfo.ActionSource == ActionSource.Power && damageInfo.SourcePower == PowerType.Fire)
            {
                GameActionExecutor.DoDamageAllEnemyAction(damageInfo); // 給予所有敵人"燃燒"層數的傷害
                GameActionExecutor.DoApplyPowerToAllEnemyAction( 
                    new ApplyPowerParameters(damageInfo.Value, PowerType.Fire)); // 給予所有敵人"燃燒"層數的"燃燒"
            }
        }
    }
}