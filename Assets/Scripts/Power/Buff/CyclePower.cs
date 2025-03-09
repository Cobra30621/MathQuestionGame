using Characters;
using Combat;
using Combat.Card;
using UnityEngine;

namespace Power.Buff
{
    /// <summary>
    /// 本回合，每使用一張牌則抽一張牌
    /// </summary>
    public class CyclePower : PowerBase
    {
        public override PowerName PowerName => PowerName.Cycle;


        public override void OnUseCard(BattleCard card)
        {
            Debug.Log("Use Card");
            CollectionManager.Instance.DrawCards(1);
        }

        public override void OnTurnStart(TurnInfo info)
        {
            if (info.CharacterType == CharacterType.Ally)
            {
                ClearPower();
            }
        }
    }
}