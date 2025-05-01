using rStarTools.Scripts.StringList;

namespace Card.Data
{
    public class CardDataOverview : DataOverviewBase<CardDataOverview, CardData>
    {



        public CardName GetCardName(CardData cardData)
        {
            var uniqueId = cardData.DataId;
            var cardName = new CardName();
            cardName.SetId(uniqueId);

            return cardName;
        }
    }
}