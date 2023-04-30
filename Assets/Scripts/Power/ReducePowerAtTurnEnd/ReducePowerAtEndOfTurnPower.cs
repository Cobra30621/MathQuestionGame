using Kalkatos.DottedArrow;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Power
{
    /// <summary>
    /// 回合結束時，降低能力
    /// </summary>
    public abstract class ReducePowerAtEndOfTurnPower : PowerBase
    {
        public abstract PowerType TargetPowerType { get; }
        protected Managers.CombatManager CombatManager => Managers.CombatManager.Instance;

        protected override void SubscribeAllEvent()
        {
            CombatManager.OnRoundEnd += OnRoundEnd;
        }

        protected override void UnSubscribeAllEvent()
        {
            CombatManager.OnRoundEnd -= OnRoundEnd;
        }

        protected override void OnRoundEnd(RoundInfo info)
        {
            // 回合結束時，降低使用者的能力
            Owner.CharacterStats.ApplyPower(TargetPowerType, - Value);
            Owner.CharacterStats.ClearPower(PowerType);
            
            base.OnRoundEnd(info);
        }
    }
}