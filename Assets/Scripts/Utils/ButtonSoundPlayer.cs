using Aduio;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundPlayer : MonoBehaviour
    {
        private Button _btn;
        private AudioManager AudioManager => GameManager.Instance.AudioManager;
        private void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(PlayButton);
        }

        public void PlayButton()
        {
            AudioManager.PlaySound("Button");
        }
    }
}
