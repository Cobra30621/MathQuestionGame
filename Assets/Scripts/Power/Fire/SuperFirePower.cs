using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
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
                var targetList = CombatManager.CurrentEnemiesList;

                foreach (var target in targetList)
                {
                    var damageAction = new DamageAction();
                    damageAction.SetValue(Amount, new List<CharacterBase>(){target}, GetActionSource());
                    GameActionExecutor.Instance.AddToBottom(damageAction);

                    var applyPowerAction = new ApplyPowerAction();
                    applyPowerAction.SetValue(Amount, PowerName.Fire, new List<CharacterBase>(){target}, GetActionSource());
                    GameActionExecutor.Instance.AddToBottom(applyPowerAction);
                }
            }
        }
    }
}