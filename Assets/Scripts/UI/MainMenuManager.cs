using Managers;
using Question;
using Save;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Required]
        [SerializeField] private SceneChanger sceneChanger;

        [Required]
        [SerializeField] private GameObject continueButton;
    
    
    
        private void Awake()
        {
            // 有單局遊戲存檔資料，才會讓繼續按鈕出現
            var hasOngoingGame = SaveManager.Instance.HasOngoingGame();
            continueButton.SetActive(hasOngoingGame);
        }


        public void ContinueGame()
        {
            GameManager.Instance.ContinueGame();
            StartCoroutine(sceneChanger.OpenMapScene());
        }

        public void ExitGame()
        {
            sceneChanger.ExitApp();
        }

        public void OpenShop()
        {
            UIManager.Instance.ShopCanvas.OpenCanvas();
        }

        public void EnterSelectedQuestionPanel()
        {
            QuestionManager.Instance.EnterSelectedQuestionUI();
        }
    
    }
}
