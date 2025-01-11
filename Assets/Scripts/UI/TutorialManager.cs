using UnityEngine;
using UnityEngine.UI;

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
        }

        private void ShowImageAtIndex(int index)
        {
            if (index >= 0 && index < tutorialImages.Length)
            {
                tutorialImage.sprite = tutorialImages[index];
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
                CloseTutorial();
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
                currentIndex = 0;
            }
        }

        private void CloseTutorial()
        {
            tutorialPanel.SetActive(false);
        }
    }
}