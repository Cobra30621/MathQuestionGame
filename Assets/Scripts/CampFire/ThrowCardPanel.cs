using System.Collections.Generic;
using Card;
using Managers;
using NueGames.Card;
using UnityEngine;

namespace CampFire
{
    public class ThrowCardPanel: MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;

        [SerializeField] private ThrowBattleCardUI throwBattleCardUI;
        private List<ThrowBattleCardUI> _spawnedCardList = new List<ThrowBattleCardUI>();

        [SerializeField] private Transform spawnPos;
        
        
        public void Open()
        {
            mainPanel.SetActive(true);
            SetCardUI();
        }

        public void Close()
        {
            mainPanel.SetActive(false);
        }

        private void SetCardUI()
        {
            DestroyPreviousUI();
            
            foreach (var cardData in CardManager.Instance.CurrentCardsList)
            {
                var cardBase = Instantiate(throwBattleCardUI, spawnPos);
                _spawnedCardList.Add(cardBase);
                cardBase.Init(cardData);
                cardBase.uiCard.OnCardChose += OnThrowCard;
            }
        }

        /// <summary>
        /// Destroys the previously created UI elements.
        /// </summary>
        private void DestroyPreviousUI()
        {
            foreach (var cardBase in _spawnedCardList)
            {
                Destroy(cardBase.gameObject);
            }

            _spawnedCardList.Clear();
        }
        
        
        private void OnThrowCard()
        {
            UIManager.Instance.CampFireCanvas.OnSelectOption();
            Close();
        }
    }
}