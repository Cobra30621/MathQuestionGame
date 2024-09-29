using System;
using System.Collections.Generic;
using Card.Data;
using Map;
using Coin;
using Money;
using NueGames.Card;
using NueGames.Data.Collection;
using NueGames.Data.Collection.RewardData;
using NueGames.Data.Containers;
using NueGames.Encounter;
using NueGames.Enums;
using NueGames.NueExtentions;
using Reward;
using UnityEngine;

namespace NueGames.UI.Reward
{
    public class RewardCanvas : CanvasBase
    {
        [Header("References")]
        [SerializeField] private Transform rewardRoot;
        [SerializeField] private RewardContainer rewardContainerPrefab;
        [SerializeField] private Transform rewardPanelRoot;
        [Header("Choice")]
        [SerializeField] private Transform choice2DCardSpawnRoot;
        [SerializeField] private RewardChoiceCard rewardChoiceCardUIPrefab;
        [SerializeField] private ChoicePanel choicePanel;

        [SerializeField] private RoomFinishHandler roomFinishHandler;
        
        private readonly List<RewardContainer> _currentRewardsList = new List<RewardContainer>();
        private readonly List<RewardChoiceCard> _spawnedChoiceList = new List<RewardChoiceCard>();
        private readonly List<CardData> _cardRewardList = new List<CardData>();

        public ChoicePanel ChoicePanel => choicePanel;
        
        /// <summary>
        /// 回到地圖在選擇後
        /// </summary>
        private bool backToMap;
        
        #region Public Methods
        

        public void ShowReward(List<RewardData> rewardDatas, NodeType nodeType, bool backToMap = true)
        {
            this.backToMap = backToMap;
            UIManager.RewardCanvas.gameObject.SetActive(true);
            UIManager.RewardCanvas.PrepareCanvas();
            
            foreach (var rewardData in rewardDatas)
            {
                UIManager.RewardCanvas.BuildReward(rewardData, nodeType);
            }
        }

        public void PrepareCanvas()
        {
            rewardPanelRoot.gameObject.SetActive(true);
        }


   
        public void BuildReward(RewardData rewardData, NodeType nodeType)
        {
            var rewardClone = Instantiate(rewardContainerPrefab, rewardRoot);
            _currentRewardsList.Add(rewardClone);
            string rewardText = "";
            
            switch (rewardData.RewardType)
            {
                case RewardType.Gold:
                    int money = RewardManager.Instance.GetMoney(rewardData, nodeType);
                    rewardText = $"+ {money}";
                    rewardClone.RewardButton.onClick.AddListener(()=>GetGoldReward(rewardClone, money));
                    break;
                case RewardType.Card:
                    var cardRewardList = RewardManager.Instance.GetCardList(rewardData, 3);
                    rewardText = "卡片獎勵";
                    rewardClone.RewardButton.onClick.AddListener(()=>GetCardReward(rewardClone,cardRewardList));
                    break;
                case RewardType.Relic:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(RewardType), rewardData.RewardType, null);
            }

            var sprite = RewardManager.Instance.GetRewardSprite(rewardData.RewardType);
            rewardClone.BuildReward(sprite, rewardText);
        }
        
        public override void ResetCanvas()
        {
            ResetRewards();

            ResetChoice();
        }

        private void ResetRewards()
        {
            foreach (var rewardContainer in _currentRewardsList)
                Destroy(rewardContainer.gameObject);

            _currentRewardsList?.Clear();
        }

        private void ResetChoice()
        {
            foreach (var choice in _spawnedChoiceList)
            {
                Destroy(choice.gameObject);
            }

            _spawnedChoiceList?.Clear();
            ChoicePanel.DisablePanel();
        }

        public void OnClickNextButton()
        {
            if (backToMap)
            {
                roomFinishHandler.BackToMap();
            }
            else
            {
                CloseCanvas();
            }
        }
        
        #endregion
        
        #region Private Methods
        private void GetGoldReward(RewardContainer rewardContainer,int amount)
        {
            CoinManager.Instance.AddCoin(amount, CoinType.Money);
            _currentRewardsList.Remove(rewardContainer);
            UIManager.InformationCanvas.SetGoldText(CoinManager.Instance.Money);
            Destroy(rewardContainer.gameObject);
        }

        private void GetCardReward(RewardContainer rewardContainer, List<CardData> cardData)
        {
            ChoicePanel.gameObject.SetActive(true);
            
            for (int i = 0; i < cardData.Count; i++)
            {
                Transform spawnTransform = choice2DCardSpawnRoot;
              
                var choice = Instantiate(rewardChoiceCardUIPrefab, spawnTransform);

                var reward = cardData[i];
                choice.BuildReward(reward);
                choice.uiCard.OnCardChose += ResetChoice;
                
                _cardRewardList.Remove(reward);
                _spawnedChoiceList.Add(choice);
                _currentRewardsList.Remove(rewardContainer);
                
            }
            
            Destroy(rewardContainer.gameObject);
        }
        #endregion
        
    }
}