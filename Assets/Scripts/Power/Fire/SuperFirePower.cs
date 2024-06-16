using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Managers;


namespace NueGames.Power
{
    // 如果對方被消滅時是因為燃燒，則造成全體傷害
    public class SuperFirePower : PowerBase
    {
        public override PowerName PowerName => PowerName.SuperFire;


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
            if (damageInfo.ActionSource.IsFromPower(PowerName.Fire))
            {
                var targetList = CombatManager.Instance.Enemies;

                foreach (var target in targetList)
                {
                    var newDamageInfo = new DamageInfo(Amount, GetActionSource(), fixDamage: true);
                    GameActionExecutor.AddToBottom(new DamageAction(newDamageInfo, new List<CharacterBase>() {Owner}));

                    GameActionExecutor.AddToBottom(new ApplyPowerAction(
                        Amount, PowerName.Fire, new List<CharacterBase>(){target}, GetActionSource()));
                }
            }
        }
    }
}