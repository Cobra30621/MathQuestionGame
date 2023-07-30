using System;
using System.Collections.Generic;
using Action.Parameters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.Parameters;
using NueGames.Power;
using UnityEngine;

namespace NueGames.Characters
{
    /// <summary>
    /// 角色
    /// </summary>
    public abstract class CharacterBase : MonoBehaviour
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private Transform textSpawnRoot;

        #region Cache

        /// <summary>
        /// 角色數值
        /// </summary>
        protected CharacterStats CharacterStats;
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


        #region Event

        
        /// <summary>
        /// 被攻擊時。妳剛剛攻擊我的村莊 ? 我的 Coin Master 村莊 ?
        /// </summary>
        public Action<DamageInfo> OnAttacked => CharacterStats.OnAttacked;
        /// <summary>
        /// 攻擊時。應該是。妳大老遠跑來，就只因為我攻擊了妳的村莊?
        /// </summary>
        public Action<DamageInfo> OnAttack => CharacterStats.OnAttack;

        #endregion
        
        

        public CharacterStats GetCharacterStats()
        {
            return CharacterStats;
        }

        public virtual void BuildCharacter()
        {
            
        }
        
        
        
        public CharacterType GetCharacterType()
        {
            return CharacterType;
        }


        public Dictionary<PowerName, PowerBase> GetPowerDict()
        {
            return CharacterStats.PowerDict;
        }

        /// <summary>
        /// 持有能力
        /// </summary>
        /// <param name="powerName"></param>
        /// <returns></returns>
        public bool HasPower(PowerName powerName)
        {
            return CharacterStats.PowerDict.ContainsKey(powerName);
        }

        public int GetPowerValue(PowerName powerName)
        {
            if (CharacterStats.PowerDict.ContainsKey(powerName))
            {
                return CharacterStats.PowerDict[powerName].Amount;
            }

            return 0;
        }

        public void SetCharacterStatus(CharacterStats stats)
        {
            CharacterStats = stats;
        }

        #region Damage

        protected virtual void OnDeath(DamageInfo damageInfo)
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

        public void Heal(int value)
        {
            CharacterStats.Heal(value);
        }
        
        

        #endregion
        #region Power

        /// <summary>
        /// 賦予能力
        /// </summary>
        /// <param name="targetPower"></param>
        /// <param name="value"></param>
        public void ApplyPower(PowerName targetPower,int value)
        {
            CharacterStats.ApplyPower(targetPower, value);
        }
        
        /// <summary>
        /// 將能力 x 倍數
        /// </summary>
        public void MultiplyPower(PowerName targetPower,int value)
        {
            CharacterStats.MultiplyPower(targetPower, value);
        }

        public void ClearPower(PowerName targetPower)
        {
            CharacterStats.ClearPower(targetPower);
        }

        public void ClearAllPower()
        {
            CharacterStats.ClearAllPower();
        }
        
        #endregion


        public override string ToString()
        {
            return $"{nameof(characterType)}: {characterType}\n{nameof(CharacterStats)}: {CharacterStats}";
        }
    }
}