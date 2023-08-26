using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Data.Characters
{
    public abstract class CharacterDataBase : SerializedScriptableObject
    {
        [SerializeField] protected string characterName;
        [SerializeField] [TextArea] protected string characterDescription;
        [SerializeField] protected int maxHealth;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public int MaxHealth => maxHealth;
    }
}