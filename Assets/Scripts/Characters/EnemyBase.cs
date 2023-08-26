using System;
using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using NueGames.Action;
using NueGames.Combat;
using NueGames.Data.Characters;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Parameters;
using UnityEngine;

namespace NueGames.Characters
{
    public class EnemyBase : CharacterBase
    {
        [Header("Enemy Base References")]
        protected EnemyCharacterData enemyCharacterData;
        [SerializeField] protected EnemyCanvas enemyCanvas;
        [SerializeField] protected SoundProfileData deathSoundProfileData;
        protected EnemyAbilityData NextAbility;
        
        public EnemyCharacterData EnemyCharacterData => enemyCharacterData;
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        public SoundProfileData DeathSoundProfileData => deathSoundProfileData;
        protected GameActionExecutor GameActionExecutor => GameActionExecutor.Instance;

        #region Setup
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(EnemyCharacterData.MaxHealth, this);
            CharacterStats.SetCharacterCanvasEvent(EnemyCanvas);
            OnDeath += OnDeathAction;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

            CombatManager.OnRoundStart += ShowNextAbility;
            CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
        }

        public void SetEnemyData(EnemyCharacterData data)
        {
            enemyCharacterData = data;
        }
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            CombatManager.OnRoundStart -= ShowNextAbility;
            CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
           
            CombatManager.OnEnemyDeath(this);
            AudioManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
        }
        #endregion
        
        #region Private Methods

        private int _usedAbilityCount;
        private void ShowNextAbility(RoundInfo info)
        {
            NextAbility = EnemyCharacterData.GetAbility(_usedAbilityCount);
            EnemyCanvas.IntentImage.sprite = NextAbility.Intention.IntentionSprite;
            EnemyCanvas.IntentionData = NextAbility.Intention;
            
            if (NextAbility.HideActionValue)
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                // EnemyCanvas.NextActionValueText.text = NextAbility.ActionDataClip.ActionList[0].ActionValue.ToString();
                int actionValue = NextAbility.ActionList[0].BaseValue;
                if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack)
                {
                    // TODO 串接根據狀態，顯示不同數值
                    // actionValue = 
                }
                
                EnemyCanvas.NextActionValueText.text = $"{actionValue}";
            }

            _usedAbilityCount++;
            EnemyCanvas.IntentImage.gameObject.SetActive(true);
        }
        #endregion
        
        /// <summary>
        /// 回合開始時的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator BattleStartActionRoutine()
        {
            return ActionRoutine(EnemyCharacterData.BattleStartAbility);
        }
        
        /// <summary>
        /// 每回合的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator ActionRoutine()
        {
            return ActionRoutine(NextAbility);
        }
        
        public virtual IEnumerator ActionRoutine(EnemyAbilityData ability)
        {
            if (CharacterStats.IsStunned)
                yield break;
            
            EnemyCanvas.IntentImage.gameObject.SetActive(false);
            
            var target = CombatManager.EnemyDetermineTargets(this, ability.ActionTargetType);
            DoGameAction(ability, target);
            
            if (ability.Intention.EnemyIntentionType == EnemyIntentionType.Attack || 
                    ability.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                defaultAttackFeedback.Play();
            }
        }

        private void DoGameAction(EnemyAbilityData targetAbility,  CharacterBase target)
        {
            // TODO 敵人取得 Target List
            List<CharacterBase> targetList = new List<CharacterBase>() { target };

            ActionSource actionSource = new ActionSource()
            {
                SourceType = SourceType.Enemy,
                SourceCharacter = this,
            };
            // List<GameActionBase> gameActions =  GameActionGenerator.GetGameActions(null, 
            //     actionSource, targetAbility.ActionList, targetList);
            // GameActionExecutor.AddToBottom(gameActions);
            // TODO Enemy Action
        }

    }
}