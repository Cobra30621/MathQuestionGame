using UnityEngine;
using UnityEngine.UI;

namespace Stage
{
    /// <summary>
    /// 顯示「是否確認開始新遊戲」的 UI 面板
    /// </summary>
    public class ConfirmNewGameUI : MonoBehaviour
    {
        [SerializeField] private Button confirmStartButton; // 確認開始新遊戲按鈕
        [SerializeField] private Button cancelConfirmButton; // 取消按鈕
        [SerializeField] private GameObject mainPanel; // 面板物件

        private StartGameUI startGameUI; // 目標面板的參考

        private void Awake()
        {
            // 設定按鈕點擊事件
            confirmStartButton.onClick.AddListener(OnConfirmStart);
            cancelConfirmButton.onClick.AddListener(HidePanel);
        }

        /// <summary>
        /// 指定要打開的 StartGameUI 面板
        /// </summary>
        public void SetStartGameUI(StartGameUI startGameUI)
        {
            this.startGameUI = startGameUI;
        }

        /// <summary>
        /// 顯示確認面板
        /// </summary>
        public void ShowPanel()
        {
            mainPanel.SetActive(true);
        }

        /// <summary>
        /// 隱藏確認面板
        /// </summary>
        private void HidePanel()
        {
            mainPanel.SetActive(false);
        }

        /// <summary>
        /// 當玩家點擊「確認開始新遊戲」時執行
        /// </summary>
        private void OnConfirmStart()
        {
            HidePanel(); // 關閉自己
            startGameUI?.ShowPanel(); // 顯示 StartGameUI，如果有設定的話
        }
    }
}