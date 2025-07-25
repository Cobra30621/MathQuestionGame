using Combat.Card;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace UI
{
    public class TutorialManager : MonoBehaviour
    {
        public GameObject tutorialPanel;
        public Image tutorialImage;
        public Button nextButton;
        public Button prevButton; // 新增的上一張按鈕
        public Button closeButton;
        public Sprite[] tutorialImages;
        
        private int currentIndex = 0;
        [SerializeField] private TMP_Text indexObject; // 用於顯示當前圖片的索引
        
        private void Start()
        {
            nextButton.onClick.AddListener(ShowNextImage);
            prevButton.onClick.AddListener(ShowPrevImage); // 新增的上一張按鈕的點擊事件
            closeButton.onClick.AddListener(CloseTutorial);
            tutorialPanel.SetActive(false);
        }

        public void ShowTutorial()
        {
            tutorialPanel.SetActive(true);
            ShowImageAtIndex(currentIndex);
            
            // 當在戰鬥場景時，要將手牌關閉
            CollectionManager.CloseHandController();
        }

        private void ShowImageAtIndex(int index)
        {
            if (index >= 0 && index < tutorialImages.Length)
            {
                tutorialImage.sprite = tutorialImages[index];
                UpdateIndexObject();
            }
        }

        private void ShowNextImage()
        {
            currentIndex++;
            if (currentIndex < tutorialImages.Length)
            {
                ShowImageAtIndex(currentIndex);
            }
            else
            {
                currentIndex = 0;
                ShowImageAtIndex(currentIndex);
            }
        }

        private void ShowPrevImage()
        {
            currentIndex--;
            if (currentIndex >= 0)
            {
                ShowImageAtIndex(currentIndex);
            }
            else
            {
                currentIndex = tutorialImages.Length - 1; // 如果已經是第一張，則跳到最後一張
                ShowImageAtIndex(currentIndex);
            }
        }

        private void CloseTutorial()
        {
            tutorialPanel.SetActive(false);
            // 當在戰鬥場景時，要將手牌開啟
            CollectionManager.OpenHandController();
        }
        
        private void UpdateIndexObject() 
        {
            if (indexObject)
            {
                indexObject.text =
                    $"{currentIndex + 1} / {tutorialImages.Length}";
            }
        }
    }
}