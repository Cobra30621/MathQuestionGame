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
        public override int AtGainTurnStartMana(int rawValue)
        {
            return rawValue + 1;
        }
        public override int AtGainTurnStartDraw(int value)
        {
            if (IsMaxLevel())
            {
                return value + 1;
            }
            else
            {
                return value;
            }
        }
    }
}