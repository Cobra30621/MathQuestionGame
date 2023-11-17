using UnityEngine;
using UnityEngine.UI;
public class TutorialButton : MonoBehaviour
{
    public TutorialManager tutorialManager;

    private void Start()
    {
        // 確保tutorialManager被設置
        if (tutorialManager == null)
        {
            Debug.LogError("TutorialManager未設置!");
            return;
        }

        // 設置按鈕的點擊事件
        GetComponent<Button>().onClick.AddListener(OnClickTutorialButton);
    }

    private void OnClickTutorialButton()
    {
        // 調用TutorialManager中的ShowTutorial方法
        tutorialManager.ShowTutorial();
    }
}