using NueGames.Enums;
using UnityEngine;

namespace NueGames.Data.Containers
{
    [CreateAssetMenu(fileName = "Enemy Intention", menuName = "NueDeck/Containers/EnemyIntention", order = 0)]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private EnemyIntentionType enemyIntentionType;
        [SerializeField] private Sprite intentionSprite;
        [SerializeField] private string header;
        [SerializeField] private string content;

        public EnemyIntentionType EnemyIntentionType => enemyIntentionType;

        public Sprite IntentionSprite => intentionSprite;
        public string Header => header;
        public string Content => content;
    }
}