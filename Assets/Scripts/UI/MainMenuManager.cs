using Managers;
using Question;
using Save;
using Stage;
using UnityEngine;
using Utils;

namespace UI
{
    /// <summary>
    /// 負責主選單邏輯，例如開始遊戲、新遊戲、進入商店等功能
    /// </summary>
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject continueButton; // 繼續遊戲按鈕
        [SerializeField] private ConfirmNewGameUI confirmNewGameUI; // 確認開始新遊戲的 UI
        [SerializeField] private StartGameUI startGameUI; // 開始遊戲的 UI 面板
        [SerializeField] private SceneChanger sceneChanger; // 場景切換工具

        private void Awake()
        {
            // 檢查是否有存檔，如果有，顯示繼續遊戲按鈕
            bool hasSave = SaveManager.Instance.HasOngoingGame();
            continueButton.SetActive(hasSave);

            // 把 StartGameUI 的參考傳給 ConfirmNewGameUI
            confirmNewGameUI.SetStartGameUI(startGameUI);
        }

        /// <summary>
        /// 點擊「新遊戲」按鈕時的處理
        /// </summary>
        public void NewGame()
        {
            bool hasSave = SaveManager.Instance.HasOngoingGame();
            if (hasSave)
                confirmNewGameUI.ShowPanel(); // 如果有存檔，先詢問是否確認開新遊戲
            else
                startGameUI.ShowPanel(); // 沒有存檔，直接進入開始遊戲畫面
        }

        /// <summary>
        /// 點擊「繼續遊戲」按鈕時的處理
        /// </summary>
        public void ContinueGame()
        {
            GameManager.Instance.ContinueGame();
        }

        /// <summary>
        /// 點擊「離開遊戲」按鈕時的處理
        /// </summary>
        public void ExitGame()
        {
            sceneChanger.ExitApp();
        }

        /// <summary>
        /// 點擊「進入商店」按鈕時的處理
        /// </summary>
        public void OpenShop()
        {
            UIManager.Instance.ShopCanvas.OpenCanvas();
        }

        /// <summary>
        /// 點擊「進入題目選擇」按鈕時的處理
        /// </summary>
        public void EnterSelectedQuestionPanel()
        {
            QuestionManager.Instance.EnterSelectedQuestionUI();
        }
    }
}