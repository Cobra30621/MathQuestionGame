using System;
using Card.Data;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NueGames.Card
{
    public class RewardChoiceCard : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler
    {
        [SerializeField] private float showScaleRate = 1.15f;
        private BattleCard _battleCard;
        private Vector3 _initalScale;
        public System.Action OnCardChose;
        public GameManager GameManager => GameManager.Instance;
        public UIManager UIManager => UIManager.Instance;
        
        public void BuildReward(CardData cardData)
        {
            _battleCard = GetComponent<BattleCard>();
            _initalScale = transform.localScale;
            _battleCard.SetCard(cardData);
            _battleCard.UpdateCardText();
        }


        private void OnChoice()
        {
            if (GameManager != null)
                GameManager.CurrentCardsList.Add(_battleCard.CardData);

            if (UIManager != null)
                UIManager.RewardCanvas.ChoicePanel.DisablePanel();
            OnCardChose?.Invoke();
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
