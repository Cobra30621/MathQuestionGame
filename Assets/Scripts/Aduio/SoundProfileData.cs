



using System.Collections.Generic;
using Map;
using UnityEngine;

namespace Aduio
{
    [CreateAssetMenu(fileName = "Sound Profile", menuName = "NueDeck/Containers/SoundProfile", order = 1)]
    public class SoundProfileData : ScriptableObject
    {
        [SerializeField] private AudioActionType audioType;
        [SerializeField] private List<AudioClip> randomClipList;

        public AudioActionType AudioType => audioType;

        public List<AudioClip> RandomClipList => randomClipList;

        public AudioClip GetRandomClip() => RandomClipList.Count>0 ? RandomClipList.Random():null;
    }
}