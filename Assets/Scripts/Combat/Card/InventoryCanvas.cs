using System.Collections.Generic;
using Card;
using Card.Data;
using Sirenix.OdinInspector;
using TMPro;
using UI;
using UnityEngine;

namespace Combat.Card
{
    public class InventoryCanvas : CanvasBase
    {
        [SerializeField] private TextMeshProUGUI titleTextField;
  
        public TextMeshProUGUI TitleTextField => titleTextField;

        [Required]
        public CardListDisplay CardListDisplay;

        public void ChangeTitle(string newTitle) => TitleTextField.text = newTitle;


        [SerializeField] private DeckData testDisplayDeck;


        [Button]
        public void Test()
        {
            SetCards(testDisplayDeck.CardList);
        }
        
        
        public void SetCards(List<CardData> cardDataList)
        {
            var cardInfos = 
                CardManager.Instance.cardInfoGetter.CreateCardInfos(cardDataList);
            CardListDisplay.Open(cardInfos);
        }

        public override void OpenCanvas()
        {
            base.OpenCanvas();
            if (CollectionManager.HasInstance())
                CollectionManager.CloseHandController();
        }

        public override void CloseCanvas()
        {
            base.CloseCanvas();
            if (CollectionManager.HasInstance())
                CollectionManager.OpenHandController();
        }

        public override void ResetCanvas()
        {
            base.ResetCanvas();
        }
    }
}
