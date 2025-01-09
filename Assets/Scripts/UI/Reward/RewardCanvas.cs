using System;
using System.Collections.Generic;
using Card.Data;
using Map;
using Money;
using NueGames.Encounter;
using NueGames.Relic;
using Relic;
using Reward;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.UI.Reward
{
    public class RewardCanvas : CanvasBase
    {
        /// <summary>
        /// 產生獎勵項目的節點
        /// </summary>
        [SerializeField] private Transform rewardRoot;
        
        /// <summary>
        /// 獎勵容器預製體
        /// </summary>
        [SerializeField] private RewardContainer rewardContainerPrefab;
        
        /// <summary>
        /// 房間完成處理器
        /// </summary>
        [SerializeField] private RoomFinishHandler roomFinishHandler;

        /// <summary>
        /// 卡片獎勵選擇面板
        /// </summary>
        [SerializeField] private CardRewardChoicePanel cardRewardChoicePanel;
        
        /// <summary>
        /// 回到地圖在選擇後
        /// </summary>
        private bool backToMap;

        /// <summary>
        /// 離開獎勵界面後，執行的事件
        /// </summary>
        private System.Action onLeave;
        
        /// <summary>
        /// 當前獎勵數據列表
        /// </summary>
        private List<RewardData> _rewardDatas;
        
        #region Public Methods
        

        /// <summary>
        /// 顯示獎勵介面
        /// </summary>
        /// <param name="rewardDatas">獎勵數據列表</param>
        /// <param name="nodeType">節點類型</param>
        /// <param name="backToMap">是否在完成後返回地圖</param>
        /// <param name="onLeave">離開時的回調函數</param>
        public void ShowReward(List<RewardData> rewardDatas, NodeType nodeType, bool backToMap = true, System.Action onLeave = null)
        {
            this.backToMap = backToMap;
            this.onLeave = onLeave;
            
            OpenCanvas();
            PrepareCanvas();
            _rewardDatas = rewardDatas;
            
            foreach (var rewardData in rewardDatas)
            {
                BuildReward(rewardData, nodeType);
            }
        }

        /// <summary>
        /// 準備獎勵畫布，清理舊的獎勵項目
        /// </summary>
        private void PrepareCanvas()
        {
            foreach (Transform child in rewardRoot)
            {
                Destroy(child.gameObject);
            }
            
            cardRewardChoicePanel.Reset();
        }


   
        /// <summary>
        /// 建立單個獎勵項目
        /// </summary>
        /// <param name="rewardData">獎勵數據</param>
        /// <param name="nodeType">節點類型</param>
        public void BuildReward(RewardData rewardData, NodeType nodeType)
        {
            var rewardClone = Instantiate(rewardContainerPrefab, rewardRoot);
            string rewardText = "";
            var sprite = RewardManager.Instance.GetRewardSprite(rewardData.RewardType);
            
            switch (rewardData.RewardType)
            {
                case RewardType.Money:
                    int money = RewardManager.Instance.GetMoney(rewardData, nodeType);
                    rewardText = $"+ {money}";
                    break;
                case RewardType.Card:
                    var cardRewardList = RewardManager.Instance.GetCardList(rewardData, 3);
                    rewardText = "卡片獎勵";
                    break;
                case RewardType.Stone:
                    int stone = RewardManager.Instance.GetStone(rewardData, nodeType);
                    rewardText = $"+ {stone} ";
                    break;
                case RewardType.Relic:
                    var (relicName, relicData) = RewardManager.Instance.GetRelic(nodeType);
                    rewardText = $"{relicData.Title}";
                    sprite = relicData.IconSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(RewardType), rewardData.RewardType, null);
            }
            
            rewardClone.BuildReward(sprite, rewardText);
        }

        /// <summary>
        /// 領取所有獎勵
        /// </summary>
        [Button("領取所有獎勵")]
        public void ReceiveRewards()
        {
            
            // 是否有要選擇的獎勵
            bool haveChoiceReward = false;
            
            var nodeType = MapManager.Instance.GetCurrentNodeType();
            
            Debug.Log($"_rewardDatas: {_rewardDatas.Count}");
            foreach (var rewardData in _rewardDatas)
            {
                switch (rewardData.RewardType)
                {
                    case RewardType.Money:
                        GetGoldReward(RewardManager.Instance.GetMoney(rewardData, nodeType));
                        break;
                    case RewardType.Stone:
                        GetStoneReward(RewardManager.Instance.GetStone(rewardData, nodeType));
                        break;
                    case RewardType.Card:
                        var cardRewardList = RewardManager.Instance.GetCardList(rewardData, 3);
                        GetCardReward(cardRewardList);
                        haveChoiceReward = true;
                        break;
                    case RewardType.Relic:
                        var (relicName, relicData) = RewardManager.Instance.GetRelic(nodeType);
                        GetRelicReward(relicName);
                        haveChoiceReward = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(RewardType), rewardData.RewardType, null);
                }
            }

            // 如果沒有要選擇的獎勵，直接離開獎勵介面
            if (!haveChoiceReward)
            {
                LeaveRewardCanvas();
            }
        }


  


        /// <summary>
        /// 離開獎勵畫布
        /// </summary>
        public void LeaveRewardCanvas()
        {
            if (backToMap)
            {
                // 回到地圖(戰鬥後
                roomFinishHandler.BackToMap();
            }
            else
            {
                // 關閉介面
                onLeave?.Invoke();
                CloseCanvas();
            }
        }
        
        #endregion
        
        #region Private Methods
        /// <summary>
        /// 獲得金幣獎勵
        /// </summary>
        /// <param name="amount">金幣數量</param>
        private void GetGoldReward(int amount)
        {
            CoinManager.Instance.AddCoin(amount, CoinType.Money);
        }
        
        /// <summary>
        /// 獲得寶石獎勵
        /// </summary>
        /// <param name="amount">寶石數量</param>
        private void GetStoneReward(int amount)
        {
            CoinManager.Instance.AddCoin(amount, CoinType.Stone);

        }

        /// <summary>
        /// 獲得卡片獎勵
        /// </summary>
        /// <param name="cardData">卡片數據列表</param>
        private void GetCardReward(List<CardData> cardData)
        {
            cardRewardChoicePanel.Show(cardData);
        }

        /// <summary>
        /// 獲得遺物獎勵
        /// </summary>
        /// <param name="relicName">遺物名稱</param>
        private void GetRelicReward(RelicName relicName)
        {
            GameManager.RelicManager.GainRelic(relicName);
        }
        
        
        #endregion
        
    }
}