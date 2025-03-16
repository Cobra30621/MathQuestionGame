using System.Collections.Generic;
using Characters;
using Combat;
using Combat.Card;
using Effect;
using Effect.Parameters;
using GameListener;
using Power;
using Relic.Data;
using UnityEngine;

namespace Relic.Thief
{
    public class BootsRelic : RelicBase
    {
        public override RelicName RelicName => RelicName.Boots;
        public override float GetCardRawMana(float rawValue)
        {
            if (rawValue > 1)
            {
                return rawValue - 1;
            }
            return rawValue;
        }
    }
}