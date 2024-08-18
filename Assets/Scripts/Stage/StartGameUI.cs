using System.Collections;
using System.Collections.Generic;
using NueGames.Data.Characters;
using NueGames.Managers;
using NueGames.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Stage
{
    public class StartGameUI : MonoBehaviour
    {
        [Required] public Button startButton;
        [Required] public Button backButton;
        [Required] [SerializeField]  private CanvasGroup _canvasGroup;

        [SerializeField] private StageDataOverview _stageDataOverview;
        
        private StageSelectedHandler stageSelectedHandler;
        [Required] public StageSelectedUI stageSelectedUI;
        [Required] public AllySelectedUI allySelectedUI;

        [Required]
        [SerializeField] private SceneChanger sceneChanger;

        private void Awake()
        {

            startButton.onClick.AddListener(StartGame);
            backButton.onClick.AddListener(ClosePanel);
            
            
            

            ClosePanel();
        }

        private void Start()
        {
            stageSelectedHandler = GameManager.Instance.StageSelectedHandler;
            // Check if null
            if (stageSelectedHandler == null)
            {
                Debug.LogError("StageSelectedHandler is null");
                return;
            }
            
            stageSelectedHandler.OnAllyDataChanged.AddListener(
                allySelectedUI.OnAllySelected);
        }

        /// <summary>
        /// Shows the panel by enabling the canvas group and setting its alpha to 1.
        /// </summary>
        public void ShowPanel()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;

            stageSelectedUI.Init(stageSelectedHandler.GetStageDataList());
            allySelectedUI.Init(stageSelectedHandler.GetAllyDataList());
        }

        /// <summary>
        /// Closes the panel by disabling the canvas group and setting its alpha to 0.
        /// </summary>
        public void ClosePanel()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            
            allySelectedUI.ClosePanel();
        }

        /// <summary>
        /// Handles the start game button click event.
        /// </summary>
        public void StartGame()
        {
            // Add your game logic to start the game
            Debug.Log("Starting the game...");
            
            GameManager.Instance.NewGame();
            StartCoroutine(StartGameCoroutine());
        }


        private IEnumerator StartGameCoroutine()
        {
            // onlineQuestionDownloader.GetQuestion();
            // yield return new WaitForSeconds(5);
            yield return null;
            sceneChanger.OpenMapScene();
        }
    }
}