using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SurveyButton : MonoBehaviour
    {
        public string linkUrl = "https://forms.gle/XnkTpZjWz4SmEvAV7"; // 更改為你想要打開的連結

        void Start()
        {
            // 確保按鈕組件存在
            Button button = GetComponent<Button>();
            if (button != null)
            {
                // 設置按鈕的點擊事件
                button.onClick.AddListener(OpenLink);
            }
            else
            {
                Debug.LogError("Button component not found on the GameObject.");
            }
        }

        void OpenLink()
        {
            // 打開連結
            Application.OpenURL(linkUrl);
        }
    }
}