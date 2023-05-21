using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得數學瑪娜
    /// </summary>
    public class ManaGeneratorRelic : RelicBase
    {
        public override RelicType RelicType => RelicType.ManaGenerator;


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
            CharacterBase ally = CombatManager.Instance.CurrentMainAlly;
            ally.CharacterStats.ApplyPower(PowerType.MathMana, 1);
        }
        
    }
}