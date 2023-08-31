using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Collection.RewardData
{
    public class RewardDataBase : SerializedScriptableObject
    {
        [SerializeField] private Sprite rewardSprite;
        [TextArea] [SerializeField] private string rewardDescription;
        public Sprite RewardSprite => rewardSprite;
        public string RewardDescription => rewardDescription;
    }
}