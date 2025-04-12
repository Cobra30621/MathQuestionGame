using System;
using System.Collections.Generic;
using Card.Data;
using Economy;
using Encounter;
using Map;
using Relic.Data;
using Reward.Data;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using static UnityEngine.Random;
namespace Reward.UI
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
        /// 產生戰鬥勝利後的獎勵
        /// </summary>
        /// <param name="nodeType"></param>
        [Button]
        public void ShowCombatWinReward(NodeType nodeType)
        {
            var winRewards = GenerateCombatWinRewards(nodeType);
            ShowReward(winRewards, nodeType);
        }

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

            string tooltipTitle = "";
            string tooltipDescription = "";
            
            switch (rewardData.RewardType)
            {
                case RewardType.Money:
                    int money = RewardManager.Instance.GetMoney(rewardData, nodeType);
                    rewardText = $"+ {money}";
                    break;
                case RewardType.Card:
                    rewardText = "卡片獎勵";
                    break;
                case RewardType.Stone:
                    int stone = RewardManager.Instance.GetStone(rewardData, nodeType);
                    rewardText = $"+ {stone} ";
                    break;
                case RewardType.Relic:
                    var relicInfo = RewardManager.Instance.GetRelic(nodeType, rewardData);
                    rewardText = $"{relicInfo.data.Title}";
                    sprite = relicInfo.data.IconSprite;
                    // 將產生的遺物，暫存在 rewardData 中
                    rewardData.randomNameCache = relicInfo.relicName;
                    rewardClone.NeedShowToolTip($"{relicInfo.data.Title}", relicInfo.GetDescription());
                    break;
                case RewardType.Heal:
                    var healthAmount = rewardData.healthAmount;
                    rewardText = $"+{healthAmount}";
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
                        var cardRewardList = RewardManager.Instance.GetCombatWinCardList(3);
                        GetCardReward(cardRewardList);
                        haveChoiceReward = true;
                        break;
                    case RewardType.Relic:
                        // 使用前面產生的暫存遺物
                        var relicName = rewardData.randomNameCache;
                        GetRelicReward(relicName);
                        break;
                    case RewardType.Heal:
                        var healthAmount = rewardData.healthAmount;
                        GameManager.HealAlly(healthAmount);
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
        
        
        /// <summary>
        /// 產生戰鬥獲勝的獎勵
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        private List<RewardData> GenerateCombatWinRewards(NodeType nodeType)
        {
            var rewardList = new List<RewardData>()
            {
                new()
                {
                    RewardType = RewardType.Card,
                    ItemGainType =  ItemGainType.Character
                },
                new ()
                {
                    RewardType =  RewardType.Money,
                    CoinGainType =  CoinGainType.NodeType
                }
            };

            switch (nodeType)
            {
                // 精英怪有一定機率掉落遺物
                case NodeType.EliteEnemy:
                    var eliteDropRelicRate = 0.33f;
                    if (Range(0f, 1f) < eliteDropRelicRate)
                    {
                        rewardList.Add(new RewardData()
                        {
                            RewardType = RewardType.Relic,
                        });
                    }
                    
                    break;
                // Boss敵人多一個遺物
                case NodeType.Boss:
                    rewardList.Add(new RewardData()
                    {
                        RewardType = RewardType.Relic,
                    });
                    break;
            }

            return rewardList;
        }
        
        
        #endregion
        
    }
}