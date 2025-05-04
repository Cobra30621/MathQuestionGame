using System;
using Economy;
using Reward;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Question.UI
{
    /// <summary>
    /// 答題結果
    /// </summary>
    public class QuestionOutcomeUI : MonoBehaviour
    {
        /// <summary>
        /// 主介面
        /// </summary>
        [Required]
        [SerializeField] private GameObject mainPanel;

        [Required] [SerializeField] private TextMeshProUGUI correctCount;
        [Required] [SerializeField] private TextMeshProUGUI wrongCount;
        [Required] [SerializeField] private TextMeshProUGUI rewardCount;

        [Required] [SerializeField] private Button closeButton;
        [Required] [SerializeField] private Button againButton;

        private void Start()
        {
            closeButton.onClick.AddListener(ClosePanel);
            againButton.onClick.AddListener(Again);
        }

        public void ShowOutcome(AnswerRecord record)
        {
            mainPanel.SetActive(true);
            
            correctCount.text = $"答對數量：{record.CorrectCount}";
            wrongCount.text = $"答錯數量：{record.WrongCount}";

            int stone = RewardManager.Instance.GetQuestionReward(record);
            CoinManager.Instance.AddCoin(stone, CoinType.Stone);
            
            rewardCount.text = $"獎勵寶石：{stone}";
        }

        private void Again()
        {
            ClosePanel();
            var action = QuestionManager.Instance.QuestionAction;
            var needAnswerCount = QuestionManager.Instance.QuestionSetting.needAnswerCount;
            QuestionManager.Instance.EnterQuestionMode(action, needAnswerCount);
        }
        
        
        public void ClosePanel()
        {
            mainPanel.SetActive(false);
        }
    }
}