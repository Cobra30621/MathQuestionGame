using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Power
{
    public class HardcorePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Hardcore;
        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong += OnAnswerWrong;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerWrong -= OnAnswerWrong;
        }

        protected override void OnTurnStart(TurnInfo info)
        {
            // if (info.CharacterType == GetOwnerCharacterType())
            // {
            //     ClearPower();
            // }
        }
        protected override void OnAnswerWrong()
        {
            var damageInfo = new DamageInfo(99999, GetActionSource(), fixDamage: true);

            GameActionExecutor.AddToBottom(new DamageAction(damageInfo, new List<CharacterBase>() {Owner}));
        }
    }
}