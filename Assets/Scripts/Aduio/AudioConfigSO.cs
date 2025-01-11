namespace Data.Audio
{
    using UnityEngine;
    using System.Collections.Generic;

    [CreateAssetMenu(fileName = "AudioConfigSO", menuName = "Audio/AudioConfigSO")]
    public class AudioConfigSO : ScriptableObject
    {
        public List<AudioData> audioList = new List<AudioData>();

        public AudioData GetAudioData(string id)
        {
            return audioList.Find(audio => audio.id == id);
        }
    }



    [System.Serializable]
    public class AudioData
    {
        public string id;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
    }
}