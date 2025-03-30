using Combat.Card;
using Effect.Card;
using Relic.Data;

namespace Relic.General
{
    public class PumpRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Pump;
        private int drawCount = 1;
        public override void OnUseCard(BattleCard card)
        {
            drawCount = IsMaxLevel() ? 2 : 1;
            var drawCardEffect = new DrawCardEffect(drawCount, GetEffectSource());
            drawCardEffect.Play();
        }
       
    }
}