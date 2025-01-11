using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using UnityEngine.UI;

namespace Power.ReducePowerAtTurnEnd
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
            EffectExecutor.AddAction(new ApplyPowerEffect(
                -Amount, TargetPowerName, new List<CharacterBase>(){Owner}, GetActionSource()));
            
            EffectExecutor.AddAction(new ClearPowerAction(
                PowerName, new List<CharacterBase>(){Owner}, GetActionSource()));
            
            base.OnRoundEnd(info);
        }
    }
}