using Action.Parameters;
using NueGames.Action;
using NueGames.Managers;


namespace NueGames.Power
{
    // 如果對方被消滅時是因為燃燒，則造成全體傷害
    public class SuperFirePower : PowerBase
    {
        public override PowerName PowerName => PowerName.SuperFire;


        public override void SubscribeAllEvent()
        {
            Owner.GetCharacterStats().OnDeath += OnDead;
        }

        public override void UnSubscribeAllEvent()
        {
            Owner.GetCharacterStats().OnDeath -= OnDead;
        }


        protected override void OnDead(DamageInfo damageInfo)
        {
            if (damageInfo.ActionSource.IsFromPower(PowerName.Fire))
            {
                DamageAllEnemyAction damageAction = new DamageAllEnemyAction();
                damageAction.SetDamageActionValue(damageInfo.BaseValue, 
                    null,
                    GetActionSource()
                );
                GameActionExecutor.Instance.AddToBottom(damageAction);
                
                // 給予所有敵人"燃燒"層數的"燃燒"
                ApplyPowerToAllEnemyAction action = new ApplyPowerToAllEnemyAction();
                action.SetPowerActionValue(1, PowerName.Fire, 
                    null, GetActionSource());
                GameActionExecutor.AddToBottom(action);
            }
        }
    }
}