using System.Collections.Generic;
using Combat;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;
using NueGames.Power;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得力量
    /// </summary>
    public class StrengthGeneratorRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.StrengthGenerator;


        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }
        protected override void OnAnswerCorrect()
        {
            CharacterBase ally = CombatManager.Instance.MainAlly;
            GameActionExecutor.AddAction(new ApplyPowerAction(
                1, PowerName.Strength, new List<CharacterBase>(){ally}, GetActionSource()));
        }
        
    }
}