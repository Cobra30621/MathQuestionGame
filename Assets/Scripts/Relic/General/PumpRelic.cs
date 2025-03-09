using Combat.Card;
using Effect.Card;
using Relic.Data;

namespace Relic.Hunter
{
    public class PumpRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Pump;
        public override void OnUseCard(BattleCard card)
        {
            var drawCardEffect = new DrawCardEffect(1, GetEffectSource());
            drawCardEffect.Play();
        }
       
    }
}