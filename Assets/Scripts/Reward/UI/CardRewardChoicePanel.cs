using System.Collections.Generic;
using Card;
using Card.Data;
using Managers;
using NueGames.Card;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Reward.UI
{
    public class CardRewardChoicePanel : MonoBehaviour
    {
        // 確認獲得獎勵的按鈕
        [Required]
        [SerializeField] private Button getRewardButton;
        
        // 2D卡牌生成的父物體位置
        [Required]
        [SerializeField] private Transform choice2DCardSpawnRoot;
        
        // 獎勵卡牌UI預製體
        [Required]
        [SerializeField] private RewardChoiceCard rewardChoiceCardUIPrefab;

        // 當前場景中的所有獎勵卡牌列表
        private List<RewardChoiceCard> _rewardChoiceCards = new List<RewardChoiceCard>();

        // 當前選中的獎勵卡牌
        private RewardChoiceCard selectedChoice;

        /// <summary>
        /// 初始化面板，設置按鈕監聽和初始狀態
        /// </summary>
        private void Awake()
        {
            _rewardChoiceCards = new List<RewardChoiceCard>();
            getRewardButton.onClick.AddListener(GetReward);

            getRewardButton.interactable = false;
        }

        /// <summary>
        /// 顯示獎勵選擇面板，並生成可選擇的卡牌
        /// </summary>
        /// <param name="cardData">可選擇的卡牌數據列表</param>
        public void Show(List<CardData> cardData)
        {
            gameObject.SetActive(true);
            getRewardButton.interactable = false;
            
            _rewardChoiceCards = new List<RewardChoiceCard>();
            for (int i = 0; i < cardData.Count; i++)
            {
                Transform spawnTransform = choice2DCardSpawnRoot;
              
                var choice = Instantiate(rewardChoiceCardUIPrefab, spawnTransform);

                var reward = cardData[i];
                choice.BuildReward(reward, this);
                _rewardChoiceCards.Add(choice);
            }
        }

        /// <summary>
        /// 選擇卡牌時的處理函數
        /// </summary>
        /// <param name="rewardChoiceCard">被選中的獎勵卡牌</param>
        public void ChoiceCard(RewardChoiceCard rewardChoiceCard)
        {
            selectedChoice = rewardChoiceCard;
            getRewardButton.interactable = true;
            
            foreach (var choiceCard in _rewardChoiceCards)
            {
                choiceCard.SetChoiceBackground(false);
            }
            selectedChoice.SetChoiceBackground(true);
        }

        /// <summary>
        /// 確認獲得獎勵時的處理函數
        /// </summary>
        public void GetReward()
        {
            CardManager.Instance.playerDeckHandler.GainCard(selectedChoice.uiCard.CardData);
            
            UIManager.Instance.RewardCanvas.LeaveRewardCanvas();
        }
        
        /// <summary>
        /// 重置面板狀態，清理所有生成的卡牌
        /// </summary>
        public void Reset()
        {
            // 銷毀所有子物體
            foreach (Transform child in choice2DCardSpawnRoot)
            {
                Destroy(child.gameObject);
            }
            
            _rewardChoiceCards.Clear();
            gameObject.SetActive(false);
        }
    }
}