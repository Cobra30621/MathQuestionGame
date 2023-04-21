using System;
using System.Collections.Generic;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.Managers;
using UnityEngine;

namespace NueGames.Collection
{
    public class CardChoicer : MonoBehaviour
    {
        private List<CombatChoiceCard> _spawnedChoiceList = new List<CombatChoiceCard>();
        private List<CardData> _sourceCardPile = new List<CardData>();
        private ChoiceParameter _currentParameter;
        private CollectionManager CollectionManager => CollectionManager.Instance;

        [SerializeField] private Transform spawnTransform;
        [SerializeField] private CombatChoiceCard choiceCardUIPrefab;
        [SerializeField] private GameObject choicePanel;


        private void Start()
        {
            Close();
        }

        /// <summary>
        /// 開啟選牌介面
        /// </summary>
        /// <param name="choiceParameter"></param>
        public void ShowChoiceCardPanel(ChoiceParameter choiceParameter)
        {
            DestroyPreviousChoiceCard();
            
            _currentParameter = choiceParameter;
            Debug.Log($"CollectionManager.PileDict{CollectionManager}");

            if (choiceParameter.SourcePile == PileType.Hand)
            {
                Debug.Log("手牌的選擇介面要另外處理");
            }
            else
            {
                _sourceCardPile = CollectionManager.PileDict[choiceParameter.SourcePile];
                choicePanel.SetActive(true);
                foreach (var cardData in _sourceCardPile)
                {
                    var choice = Instantiate(choiceCardUIPrefab, spawnTransform);
                
                    choice.BuildCard(cardData);
                    choice.OnCardChose += OnCardChoose;
                
                    _spawnedChoiceList.Add(choice);
                }
            }
        }
        
        /// <summary>
        /// 刪除所有卡片選擇清單物件
        /// </summary>
        protected void DestroyPreviousChoiceCard()
        {
            foreach (var choiceCard in _spawnedChoiceList)
            {
                Destroy(choiceCard.gameObject);
            }

            _spawnedChoiceList = new List<CombatChoiceCard>();
        }

        /// <summary>
        /// 當卡牌被選擇時
        /// </summary>
        /// <param name="cardBase"></param>
        public void OnCardChoose(CardBase cardBase)
        {
            CollectionManager.ChangeCardPile(_currentParameter, cardBase);
            Close();
        }

        public void Close()
        {
            choicePanel.SetActive(false);
        }
    }

    [Serializable]
    public class ChoiceParameter
    {
        public PileType SourcePile;
        public PileType TargetPile;
        public int Amount;
        public bool ShuffleTargetPile;

        public ChoiceParameter(PileType sourcePile, PileType targetPile, int amount, bool shuffleTargetPile)
        {
            SourcePile = sourcePile;
            TargetPile = targetPile;
            Amount = amount;
            ShuffleTargetPile = shuffleTargetPile;
        }
    }
}