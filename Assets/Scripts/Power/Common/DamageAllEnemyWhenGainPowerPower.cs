using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;

namespace NueGames.Power
{
    /// <summary>
    /// 每獲得1點力量，對全體造成5點傷害
    /// </summary>
    public class DamageAllEnemyWhenGainPowerPower : PowerBase
    {
        public override PowerName PowerName => PowerName.DamageAllEnemyWhenGainPower;
        public PowerName GainPowerName = PowerName.Strength;
        private int damageValue = 5;

        public override void SetOwner(CharacterBase owner)
        {
            base.SetOwner(owner);
            Owner.CharacterStats.OnPowerIncrease += OnPowerIncrease;
        }

        protected override void OnPowerIncrease(PowerName powerName, int value)
        {
            if (powerName == GainPowerName)
            {
                DamageAllEnemyAction damageAction = new DamageAllEnemyAction();
                damageAction.SetDamageActionValue(damageValue * Amount, 
                    null,
                    GetActionSource()
                );
                GameActionExecutor.Instance.AddToBottom(damageAction);
            }
        }
    }
}