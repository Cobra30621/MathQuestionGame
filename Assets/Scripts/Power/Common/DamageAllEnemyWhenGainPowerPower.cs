using NueGames.Action;
using NueGames.Card;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Power
{
    /// <summary>
    /// 每獲得1點力量，對全體造成5點傷害
    /// </summary>
    public class DamageAllEnemyWhenGainPowerPower : PowerBase
    {
        public override PowerType PowerType => PowerType.DamageAllEnemyWhenGainPower;
        public PowerType gainPowerType = PowerType.Strength;
        private int damageValue = 5;

        public override void SetOwner(CharacterBase owner)
        {
            base.SetOwner(owner);
            Owner.CharacterStats.OnPowerIncrease += OnPowerIncrease;
        }

        protected override void OnPowerIncrease(PowerType powerType, int value)
        {
            if (powerType == gainPowerType)
            {
                DamageInfo damageInfo = GetDamageInfo(damageValue * Amount, 
                    true);

                DamageAllEnemyAction damageAllEnemyAction = new DamageAllEnemyAction();
                damageAllEnemyAction.SetValue(damageInfo);
            
                GameActionExecutor.Instance.AddToBottom(damageAllEnemyAction);
            }
        }
    }
}