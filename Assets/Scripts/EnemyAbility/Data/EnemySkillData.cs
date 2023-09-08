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
    /// Represents an enemy's skillData.
    /// </summary>
    public class EnemySkillData
    {
        #region Setting

        /// <summary>
        /// The name of the skillData.
        /// </summary>
        [VerticalGroup("基本")]
        [SerializeField] private string name;

        /// <summary>
        /// The intention Skill for the skillData.
        /// </summary>
        [VerticalGroup("基本")]
        [SerializeField] private EnemyIntentionData intention;
        
        
        

        /// <summary>
        /// The cooldown of the skillData.
        /// </summary>
        [VerticalGroup("發動條件")]
        [SerializeField] private int skillCd;

        /// <summary>
        /// The maximum number of times the skillData can be used.
        /// </summary>
        [VerticalGroup("發動條件")]
        [SerializeField] private int maxUseCount = -1;


        [VerticalGroup("發動條件")] 
        [SerializeField]
        private bool useCondition;

        /// <summary>
        /// The condition that determines when the skillData can be used.
        /// </summary>
        [VerticalGroup("發動條件")]
        [ShowIf("useCondition")]
        [SerializeField] private ConditionBase condition;

        /// <summary>
        /// The target type for the skillData's action.
        /// </summary>
        [VerticalGroup("執行行動")]
        [SerializeField] private ActionTargetType actionTargetType;

        /// <summary>
        /// The action to be performed by the skillData.
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
        public EnemyActionBase EnemyAction => enemyAction;
        public ActionTargetType ActionTargetType => actionTargetType;
        public int SkillCd => skillCd;
        public int MaxUseCount => maxUseCount;

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

        public ConditionBase GetCopyCondition()
        {
            if (condition != null)
            {
                return condition.GetCopy();
            }
            else
            {
                return null;
            }
        }
        
#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Fx);
        }
#endif

    }
}

