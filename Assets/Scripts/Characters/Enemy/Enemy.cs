using System.Collections;
using System.Collections.Generic;
using Action;
using Action.Enemy;
using Action.Parameters;
using Characters.Display;
using Characters.Enemy.Data;
using Combat;
using Sheets;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters.Enemy
{
    public class Enemy : CharacterBase
    {
        [Required]
        [SerializeField] protected IntentionDisplay intentionDisplay;

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
            _characterCanvas.InitCanvas();
            
            CharacterStats = new CharacterStats(data.MaxHp, this, _characterCanvas);
            
            SubscribeEvent();
        }

        protected override void SubscribeEvent()
        {
            base.SubscribeEvent();
            
            CombatManager.OnRoundStart += SetThisRoundSkill;
        }


        protected override void UnsubscribeEvent()
        {
            base.UnsubscribeEvent();
            CombatManager.OnRoundStart -= SetThisRoundSkill;
        }
        
        

        #endregion
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
           
           
            _characterHandler.OnEnemyDeath(this);
            Destroy(gameObject);
        }

        
        
        private void SetThisRoundSkill(RoundInfo info)
        {
            currentSkill = enemyAbility.GetNextSkill();
            enemyAbility.UpdateSkillsCd();
            UpdateIntentionDisplay();
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

            UpdateIntentionDisplay();
        }
        
        
        public void UpdateIntentionDisplay()
        {
            intentionDisplay.Show(currentSkill);
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
            CharacterStats = new CharacterStats(maxHealth, this, _characterCanvas);
        }

        public string GetId()
        {
            return data.ID;
        }
    }
}