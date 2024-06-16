using Card.Data;
using UnityEngine;

namespace Card.Display
{
    public abstract class CardBase : MonoBehaviour
    {
        protected CardInfo _cardInfo;

        [SerializeField] protected CardDisplay _cardDisplay;


        public virtual void Init(CardData cardData)
        {
            var cardInfo = CardManager.Instance.CreateCardInfo(cardData);
            
            Init(cardInfo);
        }
        
        public virtual void Init(CardInfo cardInfo)
        {
            _cardInfo = cardInfo;
            
            UpdateCardDisplay();
        }
        
        public void UpdateCardDisplay()
        {
            _cardDisplay.SetCard(_cardInfo);
        }
    }
}