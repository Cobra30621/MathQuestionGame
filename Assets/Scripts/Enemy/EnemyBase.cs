using System.Collections;
using Action.Parameters;
using Enemy.Data;
using NueGames.Characters;
using NueGames.Combat;
using UnityEngine;

namespace Enemy
{
    public class EnemyBase : CharacterBase
    {
        [SerializeField] protected EnemyCanvas enemyCanvas;
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        
        public EnemyData data;

        public EnemyAbility enemyAbility;

        public EnemySkill currentSkill;

        #region SetUp

        public override void BuildCharacter()
        {
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(data.MaxHp, this);
            CharacterStats.SetCharacterCanvasEvent(EnemyCanvas);
            OnDeath += OnDeathAction;
            CharacterStats.SetCurrentHealth(CharacterStats.MaxHealth);
            Debug.Log($"Max health: {data.MaxHp} " +  CharacterStats.MaxHealth);

            CombatManager.OnRoundStart += SetThisRoundSkill;
            CombatManager.OnRoundEnd += CharacterStats.HandleAllPowerOnRoundEnd;
        }
        
        public void SetEnemyInfo(EnemyData enemyData, SheetDataGetter sheetDataGetter)
        {
            data = enemyData;
            enemyAbility = new EnemyAbility(enemyData, this, sheetDataGetter);
        }
     
        
        #endregion
        
        protected override void OnDeathAction(DamageInfo damageInfo)
        {
            base.OnDeathAction(damageInfo);
            CombatManager.OnRoundStart -= SetThisRoundSkill;
            CombatManager.OnRoundEnd -= CharacterStats.HandleAllPowerOnRoundEnd;
           
            CombatManager.OnEnemyDeath(this);
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
                Debug.Log($"Enemy Canvas{EnemyCanvas}");
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