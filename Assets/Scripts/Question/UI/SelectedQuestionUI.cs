using System;
using Economy;
using Question.Action;
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


        public int questionCount;
        
        private void Awake()
        {
            // 訂閱關閉介面方法
            closeButton.onClick.AddListener(
                ()=>mainPanel.SetActive(false));
            // 訂閱開啟商店方法
            shopButton.onClick.AddListener(
                ()=> UIManager.Instance.ShopCanvas.OpenCanvas());
            // 訂閱開始答題方法
            startButton.onClick.AddListener(StartQuestion);
        }

         /// <summary>
         /// 開啟介面
         /// </summary>
        [Button]
        public void OpenPanel()
        {
            mainPanel.SetActive(true);
            UpdateUI();
        }

        /// <summary>
        /// 設定答題數量
        /// </summary>
        /// <param name="count"></param>
        public void SetQuestionCount(int count)
        {
            questionCount = count;
            
            UpdateUI();
        }
        
        private void UpdateUI()
        {
            int stone = CoinManager.Instance.Stone;
            currentStone.text = $"現在寶石: {stone}";
            int dropStone = ItemDropData.questionDropStone;
            unitDropStone.text = $"答對一題獲得的寶石: {dropStone}";
            int totalDropStone = questionCount * dropStone;
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
                QuestionMode = QuestionMode.Easy
            };
            
            QuestionManager.Instance.EnterQuestionMode(normalQuestionAction);
        }

        public void ClosePanel()
        {
            mainPanel.SetActive(false);
        }
    }
}