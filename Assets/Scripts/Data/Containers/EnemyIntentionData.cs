using NueGames.Enums;
using UnityEngine;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Enemy Intention", menuName = "NueDeck/Containers/EnemyIntention", order = 0)]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private bool showIntentionValue;
        
        [SerializeField] private Sprite intentionSprite;
        [SerializeField] private string header;
        [SerializeField] private string content;


        public Sprite IntentionSprite => intentionSprite;
        public string Header => header;
        public string Content => content;
        public bool ShowIntentionValue => showIntentionValue;
    }
}