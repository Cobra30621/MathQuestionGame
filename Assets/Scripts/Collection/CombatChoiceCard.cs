using System;
using Card.Data;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.Collection
{
    public class CombatChoiceCard : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler,
        IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        private BattleCard _battleCard;
        private Vector3 _initalScale;
        public System.Action<BattleCard> OnCardChose;
        public CollectionManager CollectionManager = CollectionManager.Instance;

        public void BuildCard(CardData cardData)
        {
            _battleCard = GetComponent<BattleCard>();
            _initalScale = transform.localScale;
            _battleCard.SetCard(cardData);
            _battleCard.UpdateCardText();
        }

        private void OnChoice()
        {
            // if (CollectionManager != null)
            //     CollectionManager.OnCardChangePile();

            OnCardChose?.Invoke(_battleCard);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = _initalScale * showScaleRate;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.localScale = _initalScale;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnChoice();

        }
    }
}