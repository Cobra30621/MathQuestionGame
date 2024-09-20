using System.Collections;
using System.Collections.Generic;
using Card;
using Card.Data;
using Combat;
using Managers;
using NueGames.Card;
using NueGames.Collection;
using NueGames.Data.Collection;
using NueGames.Enums;
using UnityEngine;
using NueGames.Combat;
using Sirenix.OdinInspector;

namespace NueGames.Managers
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
        [ShowInInspector]
        [LabelText("已使用卡牌數量")]
        public int UsedCardCount { get; private set; }
        #endregion
       
        #region Setup
        protected override void DoAtAwake()
        {
            CombatManager.OnRoundStart += (roundInfo)=>
            {
                ResetUsedCardCount();
            };
            
            UsedCardCount = 0;
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
                cardObject.UpdateCardDisplay();
        }
        public void DiscardHand()
        {
            foreach (var cardBase in HandController.hand)
            {
                if (cardBase.CardData.ExhaustIfNotPlay)
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
            UsedCardCount++;
            if (targetBattleCard.CardData.ExhaustAfterPlay)
                targetBattleCard.Exhaust();
            else
                targetBattleCard.Discard();
          
            foreach (var cardObject in HandController.hand)
                cardObject.UpdateCardDisplay();
        }
        public void SetGameDeck()
        {
            foreach (var i in CardManager.Instance.CurrentCardsList) 
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

        #endregion
        
        #region Card Cost(卡牌花費改變)

        /// <summary>
        /// 根據卡牌的關鍵字，改變卡牌花費
        /// </summary>
        public void ChangeHandCardManaCost(SpecialKeywords targetKeyword, int cost)
        {
            foreach (var card in HandController.hand)
            {
                List<SpecialKeywords> keywordsList = card.CardData.KeywordsList;
                if (keywordsList.Contains(targetKeyword))
                {
                    card.SetManaCost(cost);
           
                }
            }
        }
        
        #endregion
        
        
        #endregion

        #region Private Methods
        private void ResetUsedCardCount()
        {
            
            UsedCardCount = 0;
        }
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