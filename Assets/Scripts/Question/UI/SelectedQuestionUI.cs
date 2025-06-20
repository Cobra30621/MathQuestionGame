using System;
using System.Collections.Generic;
using Economy;
using Question.Action;
using Question.Data;
using Question.Enum;
using Reward.Data;
using Sirenix.OdinInspector;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Question.UI
{
    /// <summary>
    /// 選擇答題數據介面
    /// </summary>
    public class SelectedQuestionUI : MonoBehaviour
    {
        /// <summary>
        /// 主介面
        /// </summary>
        [Required] [SerializeField] private GameObject mainPanel;
        
        
        [Required] [SerializeField] private TextMeshProUGUI currentStone;
        
        [Required] [SerializeField] private TextMeshProUGUI unitDropStone;
        
        [Required] [SerializeField] private TextMeshProUGUI maxGainStone;

        
        [Required] [SerializeField] private Button closeButton;
        [Required] [SerializeField] private Button shopButton;
        [Required] [SerializeField] private Button startButton;

        [Required] [SerializeField] private ItemDropData ItemDropData;

        [Required] [SerializeField] private QuestionStoneDropTable _questionStoneDropTable;

        [Required] [SerializeField] private ToggleGroup toggleGroup;
        [Required] [SerializeField] private QuestionCountToggle _questionCountTogglePrefab;
        [Required] [SerializeField] private Transform toggleSpawnPos;

        public int questionCount;
        
        private void Awake()
        {
            // 訂閱關閉介面方法
            closeButton.onClick.AddListener(
                ClosePanel);
            // 訂閱開啟商店方法
            shopButton.onClick.AddListener(
                ()=> UIManager.Instance.ShopCanvas.OpenCanvas());
            // 訂閱開始答題方法
            startButton.onClick.AddListener(StartQuestion);

            InitQuestionCountToggles();
        }

        private void InitQuestionCountToggles()
        {
            var questionCounts = _questionStoneDropTable.GetAvailableQuestionCounts();
            var toggles = new List<QuestionCountToggle>();
            foreach (var count in questionCounts)
            {
                var questionCountToggle = Instantiate(_questionCountTogglePrefab, toggleSpawnPos);
                questionCountToggle.Init(this, toggleGroup, count);
                toggles.Add(questionCountToggle);
            }
        }

         /// <summary>
         /// 開啟介面
         /// </summary>
        [Button]
        public void OpenPanel()
        {
            mainPanel.SetActive(true);
            
        }

        private void Update()
        {
            UpdateUI();
        }

        /// <summary>
        /// 設定答題數量
        /// </summary>
        /// <param name="count"></param>
        public void SetQuestionCount(int count)
        {
            questionCount = count;
        }
        
        private void UpdateUI()
        {
            int stone = CoinManager.Instance.Stone;
            currentStone.text = $"現在寶石: {stone}";
            unitDropStone.text = $"待處理";
            var stoneDropAmountsForQuestionCount = _questionStoneDropTable.GetStoneDropAmountsForQuestionCount(questionCount);
            int totalDropStone = stoneDropAmountsForQuestionCount[stoneDropAmountsForQuestionCount.Count - 1];
            maxGainStone.text = $"最多獲得寶石: {totalDropStone}";
            
        }
        
        /// <summary>
        /// 開始答題
        /// </summary>
        [Button]
        public void StartQuestion()
        {
            // 設定答題資訊與答題結束後的行動
            var normalQuestionAction = new NormalQuestionAction()
            {
                QuestionCount = questionCount,
                NeedCorrectCount = 0,
                fallbackToLocalIfNoInternet = false
            };
            
            QuestionManager.Instance.EnterQuestionMode(normalQuestionAction, questionCount);
        }

        private void ClosePanel()
        {
            mainPanel.SetActive(false);
        }
    }
}