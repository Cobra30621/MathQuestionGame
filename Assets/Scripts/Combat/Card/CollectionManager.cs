using System;
using System.Collections.Generic;
using Card;
using Card.Data;
using Managers;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat.Card
{
    public class CollectionManager : SingletonDestroyOnLoad<CollectionManager>
    {
        public CollectionManager(){}
      

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
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;

        protected UIManager UIManager => UIManager.Instance;

        #endregion

       
        #region Setup
        protected override void DoAtAwake()
        {
            SetupPileDict();
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
                cardObject.UpdateCardDisplay();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.hand)
            {
                if (cardBase.CardInfo.ExhaustIfNotPlay)
                {
                    cardBase.Exhaust();
                }
                else
                {
                    cardBase.Discard();
                }
            }
            
            HandController.hand.Clear();
        }
        
        public void OnCardDiscarded(BattleCard targetBattleCard)
        {
            HandPile.Remove(targetBattleCard.CardData);
            DiscardPile.Add(targetBattleCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        
        public void OnCardExhausted(BattleCard targetBattleCard)
        {
            HandPile.Remove(targetBattleCard.CardData);
            ExhaustPile.Add(targetBattleCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }
        public void OnCardPlayed(BattleCard targetBattleCard)
        {
            if (targetBattleCard.CardInfo.ExhaustAfterPlay)
                targetBattleCard.Exhaust();
            else
                targetBattleCard.Discard();
          
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardDisplay();
        }
        public void SetGameDeck()
        {
            foreach (var i in CardManager.Instance.CurrentDeck) 
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
        /// 更新所有卡牌的花費，使其再次取得
        /// </summary>
        public void UpdateAllCardsManaCost()
        {
            foreach (var card in HandController.hand)
            {
                card.SetInitCardManaCost();
            }
        }
        
        #region 卡牌在卡組間移動

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
        /// <param name="targetBattleCard"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ChangeCardPile(ChoiceParameter parameter, BattleCard targetBattleCard)
        {
            Debug.Log($"ChangeCard{parameter}");
            // switch (parameter.TargetPile)
            // {
            //     case PileType.Discard:
            //         // targetCard.Discard();
            //         break;
            //     case PileType.Exhaust:
            //         // targetCard.Exhaust();
            //         break;
            //     case PileType.Draw:
            //         // throw new System.NotImplementedException();
            //         break;
            //     case PileType.Hand:
            //         AddCardOnHand(targetCard.CardData);
            //         break;
            // }
            
            OnCardChangePile(parameter, targetBattleCard);
        }

        
        
        
        public void OnCardChangePile(ChoiceParameter parameter, BattleCard targetBattleCard)
        {
            RemoveCardFromPile(parameter.SourcePile, targetBattleCard.CardData);
            AddCardToPile(parameter.TargetPile, targetBattleCard.CardData);
            UIManager.CombatCanvas.SetPileTexts();
        }


        public void AddCardToPile(PileType targetPile, string cardId)
        {
            CardData cardData;
            var find = CardManager.Instance.cardInfoGetter.GetCardDataWithId(cardId, out cardData);

            if (find)
            {
                AddCardToPile(targetPile, cardData);
            }
            
        }
        
        
        /// <summary>
        /// 新增卡排至牌組
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="targetPile"></param>
        public void AddCardToPile(PileType targetPile, CardData cardData)
        {
            PileDict[targetPile].Add(cardData);

            // 如果是移動到手牌，創造手牌物件
            if (targetPile == PileType.Hand)
            {
                AddCardOnHand(cardData);
            }
        }

        /// <summary>
        /// 從卡組移除卡牌
        /// </summary>
        /// <param name="cardData"></param>
        /// <param name="targetPile"></param>
        public void RemoveCardFromPile(PileType targetPile, CardData cardData)
        {
            PileDict[targetPile].Remove(cardData);
            if (targetPile == PileType.Hand)
            {
                HandController.RemoveCardFromHand(cardData);
            }
        }
        
        /// <summary>
        /// 加入卡牌到手牌
        /// </summary>
        /// <param name="cardData"></param>
        protected void AddCardOnHand(CardData cardData)
        {
            var clone = GameManager.BuildAndGetCard(cardData, HandController.drawTransform);
            HandController.AddCardToHand(clone);
        }

        /// <summary>
        /// 從手牌移除卡牌
        /// </summary>
        /// <param name="cardData"></param>
        public void RemoveCardFromHand(int index)
        {
            HandController.RemoveCardFromHand(index);
        }
        
        /// 增加手牌上限
        public void AddMaxHandCard(int amount)
        {
            GameManager.GameplayData.MaxCardOnHand += amount;
        }


        #endregion

        
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