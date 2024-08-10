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
    /// 答對一題，獲得數學瑪娜
    /// </summary>
    public class BurningRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Burning;

        public override void SubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect += OnAnswerCorrect;
        }

        public override void UnSubscribeAllEvent()
        {
            QuestionManager.OnAnswerCorrect -= OnAnswerCorrect;
        }
        
        // TODO 實作這個遺物
        protected override void OnAnswerCorrect()
        {
            CharacterBase enemy = CombatManager.Instance.currentSelectedEnemyBase;
            enemy.ApplyPower(PowerName.Fire,1);

            // TODO 實作遺物
        }
        
    }
}