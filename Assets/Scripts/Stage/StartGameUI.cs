using UnityEngine;
using UnityEngine.UI;
using Managers;

namespace Stage
{
    /// <summary>
    /// 開始遊戲的選擇面板，包含角色與關卡選擇
    /// </summary>
    public class StartGameUI : MonoBehaviour
    {
        [SerializeField] private Button startButton; // 開始遊戲按鈕
        [SerializeField] private Button backButton; // 返回按鈕
        [SerializeField] private CanvasGroup panelCanvasGroup; // 面板的透明度與互動控制
        [SerializeField] private AllySelectedUI allySelectionUI; // 角色選擇面板
        [SerializeField] private StageSelectedUI stageSelectionUI; // 關卡選擇面板
        [SerializeField] private AllyAndStageSetting allyAndStageSetting; // 預設角色與關卡資料

        private void Awake()
        {
            // 設定按鈕點擊事件
            startButton.onClick.AddListener(StartGame);
            backButton.onClick.AddListener(HidePanel);

            HidePanel(); // 預設隱藏面板
        }

        private void Start()
        {
            var stageManager = StageSelectedManager.Instance;
            
            // 設定當角色被選擇時，要更新角色面板
            stageManager.OnAllyDataChanged.AddListener(allySelectionUI.OnAllySelected);
            // 當角色被選擇後，啟用「開始遊戲」按鈕
            stageManager.OnAllyDataChanged.AddListener((ally) => startButton.interactable = true);
        }

        /// <summary>
        /// 顯示開始遊戲面板
        /// </summary>
        public void ShowPanel()
        {
            panelCanvasGroup.alpha = 1;
            panelCanvasGroup.blocksRaycasts = true;
            panelCanvasGroup.interactable = true;

            stageSelectionUI.Init(allyAndStageSetting.StageNameList);
            allySelectionUI.Init(allyAndStageSetting.AllyNameList);
        }

        /// <summary>
        /// 隱藏開始遊戲面板
        /// </summary>
        public void HidePanel()
        {
            panelCanvasGroup.alpha = 0;
            panelCanvasGroup.blocksRaycasts = false;
            panelCanvasGroup.interactable = false;

            startButton.interactable = false; // 先禁用開始按鈕
            allySelectionUI.ClosePanel(); // 關閉角色選擇面板
        }

        /// <summary>
        /// 當玩家點擊「開始遊戲」時執行
        /// </summary>
        private void StartGame()
        {
            GameManager.Instance.NewGame();
        }
    }
}
