using System;

namespace Combat.Card
{
    public static class CardEvents
    {
        /// <summary>一張卡被拖出手牌 (即將要飛行到場上或回到手牌) 時觸發。</summary>
        public static event Action<BattleCard> OnCardDragged;
        /// <summary>一張卡成功被打出 (Use()) 時觸發。</summary>
        public static event Action<BattleCard> OnCardPlayed;
        /// <summary>一張卡被放回手牌 (未成功打出) 時觸發。</summary>
        public static event Action<BattleCard> OnCardReturnedToHand;

        public static void CardDragged(BattleCard card)         => OnCardDragged?.Invoke(card);
        public static void CardPlayed(BattleCard card)          => OnCardPlayed?.Invoke(card);
        public static void CardReturnedToHand(BattleCard card)  => OnCardReturnedToHand?.Invoke(card);
    }
}