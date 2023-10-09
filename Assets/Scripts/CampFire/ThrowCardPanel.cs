using System.Collections.Generic;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Managers;
using UnityEngine;

namespace CampFire
{
    public class ThrowCardPanel: MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;

        [SerializeField] private ThrowCardUI throwCardUI;
        private List<CardBase> _spawnedCardList = new List<CardBase>();

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
            
            foreach (var cardData in GameManager.Instance.CurrentCardsList)
            {
                var cardBase = Instantiate(throwCardUI, spawnPos);
                _spawnedCardList.Add(cardBase);
                cardBase.SetCard(cardData, false);
                cardBase.OnCardChose += OnThrowCard;
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