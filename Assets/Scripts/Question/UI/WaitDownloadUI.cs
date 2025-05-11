using UnityEngine;

namespace Question.UI
{
    public class WaitDownloadUI : MonoBehaviour
    {
        [SerializeField] private GameObject mainPanel;
        public void Show()
        {
            mainPanel.SetActive(true);
        }

        public void Close()
        {
            mainPanel.SetActive(false);
        }
        
    }
}