using System.Collections.Generic;
using Characters;
using Combat;
using Effect;
using Effect.Common;
using Relic.Data;

namespace Relic.Knight
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
            EffectExecutor.AddEffect(new HealEffect(
                amount, new List<CharacterBase>() {MainAlly},
                GetActionSource()));
        }
       
    }
}