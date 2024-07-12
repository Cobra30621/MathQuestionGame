using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Data.Collection;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Card.Data
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "Collection/Card", order = 0)]
    public class CardData : SerializedScriptableObject ,ISerializeReferenceByAssetGuid
    {
        // equivalent to groupID now, I guess
        [SerializeField] private string cardId;
        [SerializeField] private List<CardLevelInfo> _levelInfos;
        
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool exhaustAfterPlay;
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool canNotPlay;
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool exhaustIfNotPlay;
        
        [FoldoutGroup("卡牌特效")]
        [ValueDropdown("GetAssets")]
        [SerializeField] private GameObject fxGo;

        [FoldoutGroup("卡牌特效")]
        [SerializeField] private FxSpawnPosition fxSpawnPosition;

        [FoldoutGroup("角色動畫")] 
        [SerializeField] private bool useDefaultAttackFeedback;
        [FoldoutGroup("角色動畫")] 
        [SerializeField] private bool useCustomFeedback;

        [FoldoutGroup("角色動畫")] 
        [ShowIf("useCustomFeedback")] 
        [SerializeField] private CustomerFeedbackSetting customerFeedback;
        
        
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private string cardName;
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private Sprite cardSprite;
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private RarityType rarity;
        [FoldoutGroup("卡牌顯示")]
        [TextArea(4, 10)] [SerializeField] private string description;

        [FoldoutGroup("顯示提示字")]
        private List<SpecialKeywords> specialKeywordsList;


        #region Cache
        public string CardId => cardId;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        
        public List<CardLevelInfo> LevelInfos => _levelInfos;

        public GameObject FxGo => fxGo;
        public FxSpawnPosition FxSpawnPosition => fxSpawnPosition;
        public bool CanNotPlay => canNotPlay;
        public bool ExhaustIfNotPlay => exhaustAfterPlay;
        
        public bool UseDefaultAttackFeedback => useDefaultAttackFeedback;
        public bool UseCustomFeedback
        {
            get
            {
                if (customerFeedback == null)
                    return false;
                return useCustomFeedback;
            }
        }

        public string CustomFeedbackKey
        {
            get
            {
                if (customerFeedback == null)
                    return "";
                return customerFeedback.customFeedbackKey;
            }
        }

        public string Description => description;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public string MyDescription { get; set; }
        

        public bool ExhaustAfterPlay => exhaustAfterPlay;

        #endregion

        #region Methods
        public void UpdateDescription()
        {
            
            MyDescription = description;
        }
        
        public CardLevelInfo GetLevelInfo(int level)
        {
            if (level >= LevelInfos.Count)
            {
                throw new Exception($"level {level} 超過 {cardName} 的 LevelInfos {LevelInfos.Count} 數量");
            }
            return LevelInfos[level];
        }
        
        public void SetCardLevels(List<CardLevelInfo> levelInfos)
        {
            _levelInfos = levelInfos;
        }
        
        

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        private IEnumerable GetAssets()
        {
            return AssetGetter.GetAssets(AssetGetter.DataName.Fx);
        }
#endif

        #endregion
        
    }

}