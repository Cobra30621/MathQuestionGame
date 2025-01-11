using System.Collections.Generic;
using Action.Parameters;
using Card;
using System.Collections.Generic;
using Action;
using Action.Common;
using Characters;
using Combat;
using NueGames.Enums;
using NueGames.Managers;

namespace NueGames.Relic.Knight
{
    public class DrumstickRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Drumstick;
        private int amount = 3;
        public override void SubscribeAllEvent()
        {
            CombatManager.OnBattleWin += OnBattleWin;
        }

        public override void UnSubscribeAllEvent()
        {
            CombatManager.OnBattleWin -= OnBattleWin;
        }

        protected override void OnBattleWin(int roundNumber)
        {
            GameActionExecutor.AddAction(new HealAction(
                amount, new List<CharacterBase>() {MainAlly},
                GetActionSource()));
        }
       
    }
}