using System.Collections.Generic;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Interfaces;
using NueGames.Managers;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Characters
{
    /// <summary>
    /// 角色
    /// </summary>
    public abstract class CharacterBase : MonoBehaviour, ICharacter
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private Transform textSpawnRoot;

        #region Cache
        /// <summary>
        /// 角色數值
        /// </summary>
        public CharacterStats CharacterStats { get; protected set; }
        /// <summary>
        /// 玩家類型
        /// </summary>
        public CharacterType CharacterType => characterType;
        /// <summary>
        /// 文字特效生成處
        /// </summary>
        public Transform TextSpawnRoot => textSpawnRoot;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        #endregion


        public CharacterStats GetCharacterStats()
        {
            return CharacterStats;
        }

        public virtual void BuildCharacter()
        {
            
        }
        
        protected virtual void OnDeath()
        {
            
        }
        
        /// <summary>
        /// 被攻擊
        /// </summary>
        /// <param name="damageInfo"></param>
        public virtual void BeAttacked(DamageInfo damageInfo)
        {
            CharacterStats.BeAttacked(damageInfo);
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
                return CharacterStats.PowerDict[powerType].Amount;
            }

            return 0;
        }

        public void SetCharacterStatus(CharacterStats stats)
        {
            CharacterStats = stats;
        }
    }
}