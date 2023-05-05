using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Relic.Common
{
    /// <summary>
    /// 答對一題，獲得數學瑪娜
    /// </summary>
    public class StrengthGeneratorRelic : RelicBase
    {
        public override RelicType RelicType => RelicType.StrengthGenerator;

        
        
        // // 每回合開始時，給予一點數學瑪娜
        // public override void OnTurnStarted()
        // {
        //     CharacterBase ally = CombatManager.Instance.CurrentMainAlly;
        //     ally.CharacterStats.ApplyPower(PowerType.MathMana, 1);
        // }
        
        protected override void OnAnswerCorrect()
        {
            CharacterBase ally = CombatManager.Instance.CurrentMainAlly;
            ally.CharacterStats.ApplyPower(PowerType.Strength, 1);
        }
        
    }
}