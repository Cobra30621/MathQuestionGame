using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using EnemyAbility.EnemyAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Data.Collection;
using NueGames.Data.Containers;
using NueGames.Enums;
using NueGames.Parameters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EnemyAbility
{
    /// <summary>
    /// Represents an enemy's skill.
    /// </summary>
    public class EnemySkill
    {
        #region Setting

        /// <summary>
        /// The name of the skill.
        /// </summary>
        [VerticalGroup("基本")]
        [SerializeField] private string name;

        /// <summary>
        /// The intention data for the skill.
        /// </summary>
        [VerticalGroup("基本")]
        [SerializeField] private EnemyIntentionData intention;
        
        
        

        /// <summary>
        /// The cooldown of the skill.
        /// </summary>
        [VerticalGroup("發動條件")]
        [SerializeField] private int skillCd;

        /// <summary>
        /// The maximum number of times the skill can be used.
        /// </summary>
        [VerticalGroup("發動條件")]
        [SerializeField] private int maxUseCount = -1;


        [VerticalGroup("發動條件")] 
        [SerializeField]
        private bool useCondition;

        /// <summary>
        /// The condition that determines when the skill can be used.
        /// </summary>
        [VerticalGroup("發動條件")]
        [ShowIf("useCondition")]
        [SerializeField] private ConditionBase condition;

        /// <summary>
        /// The target type for the skill's action.
        /// </summary>
        [VerticalGroup("執行行動")]
        [SerializeField] private ActionTargetType actionTargetType;

        /// <summary>
        /// The action to be performed by the skill.
        /// </summary>
        [VerticalGroup("執行行動")]
        [SerializeField] private EnemyActionBase enemyAction;

        [VerticalGroup("特效")]
        [ValueDropdown("GetAssets")]
        [SerializeField] private GameObject fxGo;
        /// <summary>
        /// The spawn position of the effect.
        /// </summary>
        [VerticalGroup("特效")]
        [SerializeField] private FxSpawnPosition fxSpawnPosition;

        /// <summary>
        /// Indicates whether to use the default attack feedback.
        /// </summary>
        [VerticalGroup("角色動畫")]
        [SerializeField] private bool useDefaultAttackFeedback;

        
        [VerticalGroup("角色動畫")] 
        [SerializeField] private bool useCustomFeedback;
        /// <summary>
        /// The custom feedback settings.
        /// </summary>
        [VerticalGroup("角色動畫")]
        [ShowIf("useCustomFeedback")]
        [SerializeField] private CustomerFeedbackSetting customerFeedback;

        public EnemyIntentionData Intention => intention;
        public GameObject FxGo => fxGo;
        public FxSpawnPosition FxSpawnPosition => fxSpawnPosition;
        public bool UseDefaultAttackFeedback => useDefaultAttackFeedback;
        public CustomerFeedbackSetting CustomerFeedbackSetting => customerFeedback;
        public ActionTargetType ActionTargetType => actionTargetType;

        /// <summary>
        /// Indicates whether to use custom feedback.
        /// </summary>
        public bool UseCustomFeedback
        {
            get
            {
                if (customerFeedback == null)
                    return false;
                return useCustomFeedback;
            }
        }

        /// <summary>
        /// The key for the custom feedback.
        /// </summary>
        public string CustomFeedbackKey
        {
            get
            {
                if (customerFeedback == null)
                    return "";
                return customerFeedback.customFeedbackKey;
            }
        }

        #endregion

        #region Variable

        private int currentCd;
        private int hasUsedCount;
        private EnemyBase _enemy;

        #endregion

        /// <summary>
        /// Executed when the battle starts.
        /// </summary>
        public void OnBattleStart()
        {
            currentCd = 0;
            hasUsedCount = 0;
        }

        /// <summary>
        /// Sets the associated enemy for this skill.
        /// </summary>
        /// <param name="enemyBase">The enemy base to set.</param>
        public void SetEnemy(EnemyBase enemyBase)
        {
            _enemy = enemyBase;
            condition?.SetEnemy(enemyBase);
        }

        /// <summary>
        /// Plays the skill on the provided target list.
        /// </summary>
        /// <param name="targetList">The list of target characters.</param>
        public void PlaySkill(List<CharacterBase> targetList)
        {
            currentCd = skillCd;
            hasUsedCount++;
            enemyAction.SetValue(_enemy, this, targetList);
            enemyAction.DoAction();
        }

        /// <summary>
        /// Updates the cooldown of the skill.
        /// </summary>
        public void UpdateSkillCd()
        {
            currentCd--;
            if (currentCd < 0)
            {
                currentCd = 0;
            }
        }


        public bool GetIntentionValue(out int value)
        {
            if (Intention.ShowIntentionValue)
            {
                value = -1;
                if (!enemyAction.IsDamageAction) return false;
                
                var damageInfo = new DamageInfo()
                {
                    Target = CombatManager.Instance.MainAlly,
                    damageValue = enemyAction.DamageValueForIntention,
                    ActionSource = new ActionSource()
                    {
                        SourceType = SourceType.Enemy,
                        SourceCharacter = _enemy
                    }
                };
                value = CombatCalculator.GetDamageValue(damageInfo);
                return true;
            }

            value = -1;
            return false;
        }


        /// <summary>
        /// Checks if the skill can be played.
        /// </summary>
        /// <returns>True if the skill can be played, otherwise false.</returns>
        public bool CanPlay()
        {
            return currentCd <= 0 && CheckCondition() && CheckUseCount();
        }

        /// <summary>
        /// Checks the condition for using the skill.
        /// </summary>
        /// <returns>True if the condition is met, otherwise false.</returns>
        private bool CheckCondition()
        {
            return (condition?.Judge() ?? true);
        }

        /// <summary>
        /// Checks the use count of the skill.
        /// </summary>
        /// <returns>True if the use count condition is met, otherwise false.</returns>
        private bool CheckUseCount()
        {
            return (hasUsedCount < maxUseCount) || (maxUseCount == -1);
        }
        
#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Fx);
        }
#endif

    }
}
