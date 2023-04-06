using System.Collections;
using System.Collections.Generic;
using NueGames.Card;
using NueGames.Collection;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;

namespace NueGames.Managers
{
    public class CollectionManager : MonoBehaviour
    {
        public CollectionManager(){}
      
        public static CollectionManager Instance { get; private set; }

        [Header("Controllers")] 
        [SerializeField] private HandController handController;

        [SerializeField] private CardChoicer cardChoicer;

        #region Cache

        public Dictionary<PileType, List<CardData>> PileDict;

        public List<CardData> DrawPile { get; private set; } = new List<CardData>();
        public List<CardData> HandPile { get; private set; } = new List<CardData>();
        public List<CardData> DiscardPile { get; private set; } = new List<CardData>();
        
        public List<CardData> ExhaustPile { get; private set; } = new List<CardData>();
        public HandController HandController => handController;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;

        protected UIManager UIManager => UIManager.Instance;

        #endregion
       
        #region Setup
        private void Awake()
        {
            SetupPileDict();
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// 初始化卡堆 Dictionary
        /// </summary>
        private void SetupPileDict()
        {
            PileDict = new Dictionary<PileType, List<CardData>>();
            PileDict.Add(PileType.Draw, DrawPile);
            PileDict.Add(PileType.Hand, HandPile);
            PileDict.Add(PileType.Discard, DiscardPile);
            PileDict.Add(PileType.Exhaust, ExhaustPile);
            
            Debug.Log("SetupPileDict");
        }

        #endregion

        #region Public Methods
        public void DrawCards(int targetDrawCount)
        {
            var currentDrawCount = 0;

            for (var i = 0; i < targetDrawCount; i++)
            {
                if (GameManager.GameplayData.MaxCardOnHand<=HandPile.Count)
                    return;
                
                if (DrawPile.Count <= 0)
                {
                    var nDrawCount = targetDrawCount - currentDrawCount;
                    
                    if (nDrawCount >= DiscardPile.Count) 
                        nDrawCount = DiscardPile.Count;
                    
                    ReshuffleDiscardPile();
                    DrawCards(nDrawCount);
                    break;
                }

                var randomCard = DrawPile[Random.Range(0, DrawPile.Count)];
                var clone = GameManager.BuildAndGetCard(randomCard, HandController.drawTransform);
                HandController.AddCardToHand(clone);
                HandPile.Add(randomCard);
                DrawPile.Remove(randomCard);
                currentDrawCount++;
                UIManager.CombatCanvas.SetPileTexts();
            }
            
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardText();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.hand) 
                cardBase.Discard();
            
            HandController.hand.Clear();
        }
        
        public void OnCardDiscarded(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            DiscardPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        
        public void OnCardExhausted(CardBase targetCard)
        {
            HandPile.Remove(targetCard.CardData);
            ExhaustPile.Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(CardBase targetCard)
        {
            if (targetCard.CardData.ExhaustAfterPlay)
                targetCard.Exhaust();
            else
                targetCard.Discard();
          
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardText();
        }
        public void SetGameDeck()
        {
            foreach (var i in GameManager.PersistentGameplayData.CurrentCardsList) 
                DrawPile.Add(i);
        }

        public void ClearPiles()
        {
            DiscardPile.Clear();
            DrawPile.Clear();
            HandPile.Clear();
            ExhaustPile.Clear();
            HandController.hand.Clear();
        }
        
        /// <summary>
        /// 開啟選牌介面
        /// </summary>
        /// <param name="choiceParameter"></param>
        public void ShowChoiceCardPanel(ChoiceParameter choiceParameter)
        {
            cardChoicer.ShowChoiceCardPanel(choiceParameter);
        }

        /// <summary>
        /// 對卡牌堆的卡片，進行移動
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="targetCard"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void ChangeCardPile(ChoiceParameter parameter, CardBase targetCard)
        {
            Debug.Log($"ChangeCard{parameter}");
            switch (parameter.TargetPile)
            {
                case PileType.Discard:
                    // targetCard.Discard();
                    break;
                case PileType.Exhaust:
                    // targetCard.Exhaust();
                    break;
                case PileType.Draw:
                    // throw new System.NotImplementedException();
                    break;
                case PileType.Hand:
                    AddCardOnHand(targetCard.CardData);
                    break;
            }
            
            OnCardChangePile(parameter, targetCard);
        }
        
        public void OnCardChangePile(ChoiceParameter parameter, CardBase targetCard)
        {
            PileDict[parameter.SourcePile].Remove(targetCard.CardData);
            PileDict[parameter.TargetPile].Add(targetCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }

        /// <summary>
        /// 加入卡牌到手牌
        /// </summary>
        /// <param name="cardData"></param>
        public void AddCardOnHand(CardData cardData)
        {
            var clone = GameManager.BuildAndGetCard(cardData, HandController.drawTransform);
            HandController.AddCardToHand(clone);
        }
        
        #endregion

        #region Private Methods

        private void ReshuffleDiscardPile()
        {
            foreach (var i in DiscardPile) 
                DrawPile.Add(i);
            
            DiscardPile.Clear();
        }
        private void ReshuffleDrawPile()
        {
            foreach (var i in DrawPile) 
                DiscardPile.Add(i);
            
            DrawPile.Clear();
        }
        #endregion

    }
}