using NueGames.Action;
using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Combat;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得數學瑪娜
    /// </summary>
    public class BurningRelic : RelicBase
    {
        public override RelicType RelicType => RelicType.Burning;

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
            CharacterBase enemy = CombatManager.Instance.CurrentSelectedEnemy;
            enemy.CharacterStats.ApplyPower(PowerType.Fire,1);

            ApplyPowerToAllEnemyAction action = new ApplyPowerToAllEnemyAction();
            // GameActionExecutor.Instance.DoApp

        }
        
    }
}