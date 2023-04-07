using NueGames.Characters;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Relic.Common
{
    public class ManaGeneratorRelic : RelicBase
    {
        public override RelicType RelicType => RelicType.ManaGenerator;

        public override void OnTurnStarted()
        {
            // 每回合開始時，給予一點數學瑪娜
            CharacterBase ally = CombatManager.Instance.CurrentMainAlly;
            ally.CharacterStats.ApplyPower(PowerType.MathMana, 1);
        }
    }
}