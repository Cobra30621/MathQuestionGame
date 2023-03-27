using NueGames.Data.Containers;
using NueGames.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.Utils
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundProfileData soundProfileData;

        private Button _btn;
        private SoundProfileData SoundProfileData => soundProfileData;
        private AudioManager AudioManager => AudioManager.Instance;
        private void Awake()
        {
            _btn = GetComponent<Button>();
            _btn.onClick.AddListener(PlayButton);
        }
        
        public void PlayButton() => AudioManager.PlayOneShotButton(SoundProfileData.GetRandomClip());
    }
}
