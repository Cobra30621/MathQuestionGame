using NueGames.Data.Collection;
using NueGames.Managers;

namespace NueGames.Card
{
    public class ThrowBattleCardUI : BattleCard
    {
        public System.Action OnCardChose;
        
        public void ThrowCard()
        {
            GameManager.Instance.ThrowCard(CardData);
            OnCardChose.Invoke();
        }

    }
}