using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Power;
using Power;
using Relic.Data;

namespace Relic.Test
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
            EffectExecutor.AddEffect(new ApplyPowerEffect(
                1, PowerName.Strength, new List<CharacterBase>(){ally}, GetActionSource()));
        }
        
    }
}