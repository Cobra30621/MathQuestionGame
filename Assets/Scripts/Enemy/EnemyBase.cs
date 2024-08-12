using System.Collections;
using Action.Parameters;
using Combat;
using Enemy.Data;
using NueGames.Characters;
using NueGames.Combat;
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
        
        
        
        #region SetUp
        public void BuildCharacter(EnemyData enemyData, SheetDataGetter sheetDataGetter
            ,CharacterHandler characterHandler)
        {
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
        
        
        #endregion
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            CombatManager.OnRoundStart -= SetThisRoundSkill;
            CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
           
            _characterHandler.OnEnemyDeath(this);
            Destroy(gameObject);
        }
        
        
        private void SetThisRoundSkill(RoundInfo info)
        {
            currentSkill = enemyAbility.GetNextSkill();
            enemyAbility.UpdateSkillsCd();
            SetIntentionUI();
        }
        
        public void SetIntentionUI()
        {
            if (currentSkill.GetIntentionValue(out int value))
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(true);
                EnemyCanvas.NextActionValueText.text = $"{value}";
            }
            else
            {
                EnemyCanvas.NextActionValueText.gameObject.SetActive(false);
            }
            
            EnemyCanvas.IntentImage.sprite = currentSkill.intention.IntentionSprite;
            EnemyCanvas.Intention = currentSkill.intention;
            EnemyCanvas.IntentionGO.gameObject.SetActive(true);
        }

        
        /// <summary>
        /// 戰鬥開始時的行動
        /// </summary>
        /// <returns></returns>
        public IEnumerator BattleStartActionRoutine()
        {
            yield return ActionRoutine(enemyAbility.GetStartBattleSkill());
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
            skill.PlaySkill();
            yield return null;
        }

        
    }
}