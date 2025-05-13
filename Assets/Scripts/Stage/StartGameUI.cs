using System.Collections;
using System.Collections.Generic;
using Managers;
using Question;
using Save;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utils;

namespace Stage
{
    /// <summary>
    /// 控制遊戲開始畫面的 UI 行為與流程
    /// </summary>
    public class StartGameUI  : MonoBehaviour
    {
        [Required] public Button StartButton;
        [Required] public Button BackButton;

        [Required] [SerializeField]
        private CanvasGroup PanelCanvasGroup;

        [Required] [InlineEditor]
        [LabelText("角色與關卡設定")] 
        [SerializeField]
        private AllyAndStageSetting AllyAndStageSetting;

        [SerializeField] 
        private StageDataOverview StageDataOverview;

        private StageSelectedManager _stageManager;

        [Required] public StageSelectedUI StageSelectionUI;
        [Required] public AllySelectedUI AllySelectionUI;

        [Required] [SerializeField]
        private SceneChanger SceneChanger;

        /// <summary>
        /// 顯示是否放棄原本存檔，開啟新遊戲確認視窗
        /// </summary>
        [Required] public GameObject ConfirmNewGamePanel;

        [Required] public Button ConfirmStartButton;
        [Required] public Button CancelConfirmButton;

        private void Awake()
        {
            StartButton.onClick.AddListener(HandleStartButtonClicked);
            BackButton.onClick.AddListener(HidePanel);

            ConfirmStartButton.onClick.AddListener(StartGame);
            CancelConfirmButton.onClick.AddListener(() => ConfirmNewGamePanel.SetActive(false));

            HidePanel();
        }

        private void Start()
        {
            _stageManager = StageSelectedManager.Instance;

            // 設定角色選擇變更時的監聽
            _stageManager.OnAllyDataChanged.AddListener(AllySelectionUI.OnAllySelected);

            // 當選擇角色後才可按下開始遊戲
            _stageManager.OnAllyDataChanged.AddListener((ally) => StartButton.interactable = true);
        }

        /// <summary>
        /// 顯示開始遊戲的 UI 面板
        /// </summary>
        public void ShowPanel()
        {
            PanelCanvasGroup.alpha = 1;
            PanelCanvasGroup.blocksRaycasts = true;
            PanelCanvasGroup.interactable = true;

            StageSelectionUI.Init(AllyAndStageSetting.StageNameList);
            AllySelectionUI.Init(AllyAndStageSetting.AllyNameList);
        }

        /// <summary>
        /// 隱藏開始遊戲的 UI 面板
        /// </summary>
        public void HidePanel()
        {
            PanelCanvasGroup.alpha = 0;
            PanelCanvasGroup.blocksRaycasts = false;
            PanelCanvasGroup.interactable = false;

            StartButton.interactable = false;
            AllySelectionUI.ClosePanel();
        }

        /// <summary>
        /// 點擊開始按鈕時檢查是否有未完成的遊戲
        /// </summary>
        public void HandleStartButtonClicked()
        {
            if (SaveManager.Instance.HasOngoingGame())
            {
                ConfirmNewGamePanel.SetActive(true);
            }
            else
            {
                StartGame();
            }
        }

        /// <summary>
        /// 正式啟動新遊戲流程
        /// </summary>
        public void StartGame()
        {
            StartCoroutine(StartGameCoroutine());
        }

        /// <summary>
        /// 啟動新遊戲並切換到地圖場景的協程
        /// </summary>
        private IEnumerator StartGameCoroutine()
        {
            GameManager.Instance.NewGame();
            yield return SceneChanger.OpenMapScene();
        }
    }
}
