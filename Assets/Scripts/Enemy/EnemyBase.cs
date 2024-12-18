﻿using System;
using System.Collections;
using System.Collections.Generic;
using Action.Enemy;
using Action.Parameters;
using Combat;
using Enemy.Data;
using NueGames.Action;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Parameters;
using NueGames.Power;
using Sheets;
using Tool;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : CharacterBase
    {
        [SerializeField] protected EnemyCanvas enemyCanvas;
        public EnemyCanvas EnemyCanvas => enemyCanvas;

        private EnemyData data;

        public EnemyAbility enemyAbility;

        public EnemySkill currentSkill;

        private CharacterHandler _characterHandler;
        
        private SheetDataGetter _sheetDataGetter;
        
        
        #region SetUp
        public void BuildCharacter(EnemyData enemyData, SheetDataGetter sheetDataGetter
            ,CharacterHandler characterHandler)
        {
            _sheetDataGetter = sheetDataGetter;
            _characterHandler = characterHandler;
            
            data = enemyData;
            enemyAbility = new EnemyAbility(enemyData, this, sheetDataGetter);

            SetUpFeedbackDict();
            EnemyCanvas.InitCanvas();
            
            CharacterStats = new CharacterStats(data.MaxHp, this, EnemyCanvas);
            OnDeath += OnDeathAction;

            CombatManager.OnRoundStart += SetThisRoundSkill;
            CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
        }

        private void OnDestroy()
        {
            RemoveEvent();
        }

        #endregion
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            RemoveEvent();
           
            _characterHandler.OnEnemyDeath(this);
            Destroy(gameObject);
        }

        void RemoveEvent()
        {
            CombatManager.OnRoundStart -= SetThisRoundSkill;
            CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
        }
        
        
        private void SetThisRoundSkill(RoundInfo info)
        {
            currentSkill = enemyAbility.GetNextSkill();
            enemyAbility.UpdateSkillsCd();
            SetIntentionUI();
        }
        
        
        /// <summary>
        /// 設置分裂行動
        /// </summary>
        public void SetSplitEnemySkill(string enemyId)
        {
            
            var spawnAction = new SplitEnemyAction(enemyId, this);
            var intention = _sheetDataGetter.GetIntention("分裂");
            
            currentSkill = new EnemySkill(
                new List<GameActionBase>(){spawnAction}, intention);
            
            SetIntentionUI();
        }
        
        
        public void SetIntentionUI()
        {
            if (currentSkill.GetIntentionValue(out string info))
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                EnemyCanvas.NextActionValueText.text = info;
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
                
            EnemyCanvas.IntentImage.sprite = currentSkill._intention.IntentionSprite;
            EnemyCanvas.Intention = currentSkill._intention;
            EnemyCanvas.IntentionGO.gameObject.SetActive(true);
            
        }

        
        /// <summary>
        /// 戰鬥開始時的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator BattleStartActionRoutine()
        {
            if (enemyAbility.UseStartBattleSkill())
            {
                var startBattleSkills = enemyAbility.GetStartBattleSkill();

                foreach (var startBattleSkill in startBattleSkills)
                {
                    yield return ActionRoutine(startBattleSkill);
                }
            }
        }

        
        /// <summary>
        /// 每回合的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator ActionRoutine()
        {
            return ActionRoutine(currentSkill);
        }
        
        public virtual IEnumerator ActionRoutine(EnemySkill skill)
        {
            if(skill._intention.ActionType == ActionType.Attack)
                PlayDefaultAttackFeedback();
            skill.PlaySkill();
            yield return null;
        }

        public void SetMaxHealth(int maxHealth)
        {
            CharacterStats = new CharacterStats(maxHealth, this, EnemyCanvas);
        }

        public string GetId()
        {
            return data.ID;
        }
    }
}