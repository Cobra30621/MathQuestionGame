using System.Collections.Generic;
using NueGames.Action;
using NueGames.Characters;
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
            GameActionExecutor.AddToBottom(new ApplyPowerAction(
                -Amount, TargetPowerName, new List<CharacterBase>(){Owner}, GetActionSource()));
            
            GameActionExecutor.AddToBottom(new ClearPowerAction(
                PowerName, new List<CharacterBase>(){Owner}, GetActionSource()));
            
            base.OnRoundEnd(info);
        }
    }
}