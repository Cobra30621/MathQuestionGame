using Card.Data;
using UnityEngine;

namespace Card.Display
{
    public abstract class CardBase : MonoBehaviour
    {
        protected CardInfo _cardInfo;

        [SerializeField] protected CardDisplay _cardDisplay;
        
        
        public void Init(CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
            _cardDisplay.SetCard(cardInfo);
        }
    }
}