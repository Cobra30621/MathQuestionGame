using Kalkatos.DottedArrow;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace NueGames.Power
{
    /// <summary>
    /// 回合結束時，降低能力
    /// </summary>
    public abstract class ReducePowerAtEndOfTurnPower : PowerBase
    {
        protected abstract PowerName TargetPowerName { get; }

        public Button Button;

        public override void SubscribeAllEvent()
        {
            CombatManager.OnRoundEnd += OnRoundEnd;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnRoundEnd -= OnRoundEnd;
        }

        protected override void OnRoundEnd(RoundInfo info)
        {
            // 回合結束時，降低使用者的能力
            Owner.CharacterStats.ApplyPower(TargetPowerName, - Amount);
            Owner.CharacterStats.ClearPower(PowerName);
            
            base.OnRoundEnd(info);
        }
    }
}