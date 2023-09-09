using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Action.Parameters;
using EnemyAbility;
using NueGames.Action;
using NueGames.Combat;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Parameters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace NueGames.Characters
{
    public class EnemyBase : CharacterBase
    {
        [SerializeField] protected EnemyCanvas enemyCanvas;
        [SerializeField] protected SoundProfileData deathSoundProfileData;
        
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        public SoundProfileData DeathSoundProfileData => deathSoundProfileData;
        protected GameActionExecutor GameActionExecutor => GameActionExecutor.Instance;

        [Sirenix.OdinInspector.ReadOnly]
        [SerializeField] private EnemyData _enemyData;
        [Sirenix.OdinInspector.ReadOnly]
        [SerializeField] private EnemyAbility.EnemyAbility _enemyAbility;
        [Sirenix.OdinInspector.ReadOnly]
        [SerializeField] private EnemySkill _currentSkill;

        #region Setup
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(_enemyData.MaxHealth, this);
            CharacterStats.SetCharacterCanvasEvent(EnemyCanvas);
            OnDeath += OnDeathAction;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

            CombatManager.OnRoundStart += SetThisRoundSkill;
            CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
        }

        public void SetEnemyData(EnemyData enemyData)
        {
            _enemyData = enemyData;
            _enemyAbility = new EnemyAbility.EnemyAbility(enemyData.EnemyAbilityData, this);
        }

        public EnemyAbility.EnemyAbility GetAbility()
        {
            return _enemyAbility;
        }

        public EnemySkill GetCurrentSkill()
        {
            return _currentSkill;
        }


        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            CombatManager.OnRoundStart -= SetThisRoundSkill;
            CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
           
            CombatManager.OnEnemyDeath(this);
            Destroy(gameObject);
        }
        #endregion
        
        #region Private Methods

        private void SetThisRoundSkill(RoundInfo info)
        {
            _currentSkill = _enemyAbility.GetNextSkill();
            _enemyAbility.UpdateSkillsCd();
            SetIntentionUI();
        }

        public void SetIntentionUI()
        {
            if (_currentSkill.GetIntentionValue(out int value))
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                EnemyCanvas.NextActionValueText.text = $"{value}";
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            
            EnemyCanvas.IntentImage.sprite = _currentSkill.Intention.IntentionSprite;
            EnemyCanvas.IntentionData = _currentSkill.Intention;
            EnemyCanvas.IntentImage.gameObject.SetActive(true);
        }
        
        
        #endregion
        
        /// <summary>
        /// 戰鬥開始時的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator BattleStartActionRoutine()
        {
            // PlayStartBattle Skill
            if (_enemyAbility.UseStartBattleSkill())
            {
                yield return ActionRoutine(_enemyAbility.GetStartBattleSkill());
                
            }
            else
            {
                yield return null;
            }

        }
        
        /// <summary>
        /// 每回合的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator ActionRoutine()
        {
            return ActionRoutine(_currentSkill);
        }
        
        public virtual IEnumerator ActionRoutine(EnemySkill skill)
        {
            if (CharacterStats.IsStunned)
                yield break;
            
            EnemyCanvas.IntentImage.gameObject.SetActive(false);
            
            var targetList = CombatManager.EnemyDetermineTargets(this, skill.ActionTargetType);
            skill.PlaySkill(targetList);
        }

    }
}