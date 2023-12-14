using UnityEngine;
using UnityEngine.UI;

public class StoryManager : MonoBehaviour
{
    public GameObject storyPanel;
    public Image storyImage;
    public Button prevButton;
    public Button nextButton;

    public Sprite[] storyImages;
    private int currentIndex = 0;

    void Start()
    {
        // 設置按鈕點擊事件
        prevButton.onClick.AddListener(ShowPrevImage);
        nextButton.onClick.AddListener(ShowNextImage);

        // 初始化顯示第一張圖片
        ShowImageAtIndex(currentIndex);
    }

    void ShowImageAtIndex(int index)
    {
        if (index < storyImages.Length)
        {
            storyImage.sprite = storyImages[index];
            nextButton.gameObject.SetActive(true);
            prevButton.gameObject.SetActive(true);
        }
        else
        {
            storyPanel.SetActive(false);
        }
    }

    void ShowNextImage()
    {
        currentIndex++;
        ShowImageAtIndex(currentIndex);
    }

    void ShowPrevImage()
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
}