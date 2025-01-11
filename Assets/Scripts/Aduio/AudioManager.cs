using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Aduio
{
    public class AudioManager : MonoBehaviour
    {

        [SerializeField]private AudioSource musicSource;
        [SerializeField]private AudioSource sfxSource;
        [SerializeField]private AudioSource buttonSource;
        
        // 用於存儲音效片段的字典
        [SerializeField] private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

        [SerializeField] private AudioConfigSO sfxConfig;

        public static AudioManager Instance => GameManager.Instance.AudioManager;
        
        
        private void Awake()
        {
            // 初始化音訊字典
            InitializeAudioDictionary();
        }


        private void InitializeAudioDictionary()
        {
            soundEffects.Clear();
            foreach (var audioData in sfxConfig.audioList)
            {
                soundEffects[audioData.id] = audioData.clip;
            }
        }
        
        
        #region Public Methods

        public void PlayMusic(AudioClip clip)
        {
            if (!clip) return;
            
            musicSource.clip = clip;
            musicSource.Play();
        }


        public void PlaySound(string soundName)
        {
            AudioData audioData = sfxConfig.GetAudioData(soundName);
            if (audioData == null)
            {
                Debug.LogWarning($"音效 {soundName} 未找到！");
                return;
            }

            sfxSource.PlayOneShot(audioData.clip, audioData.volume);
        }
        
        
        public void PlayOneShotButton(AudioClip clip)
        {
            if (clip)
                buttonSource.PlayOneShot(clip);
        }

        #endregion
    }
}
