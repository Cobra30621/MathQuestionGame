using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.Power;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Characters
{
    public abstract class CharacterBase : MonoBehaviour, ICharacter
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private Transform textSpawnRoot;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        public Transform TextSpawnRoot => textSpawnRoot;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        #endregion
       

        public virtual void Awake()
        {
        }
        
        public virtual void BuildCharacter()
        {
            
        }
        
        protected virtual void OnDeath()
        {
            
        }
        
        public  CharacterBase GetCharacterBase()
        {
            return this;
        }

        public CharacterType GetCharacterType()
        {
            return CharacterType;
        }

        public Dictionary<PowerType, PowerBase> GetPowerDict()
        {
            return CharacterStats.PowerDict;
        }

        public int GetPowerValue(PowerType powerType)
        {
            if (CharacterStats.PowerDict.ContainsKey(powerType))
            {
                return CharacterStats.PowerDict[powerType].Value;
            }

            return 0;
        }
    }
}