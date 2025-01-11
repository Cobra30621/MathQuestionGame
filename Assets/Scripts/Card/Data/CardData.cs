using System;
using System.Collections;
using System.Collections.Generic;
using Action.Parameters;
using Action.Sequence;
using Characters.Display;
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

        [LabelText("開發者卡片")]
        [SerializeField] private bool isDevelopCard;

        [ShowIf("isDevelopCard")]
        [LabelText("卡片效果")]
        [SerializeField] private CardLevelInfo developLevelInfo;
        
        [ShowIf("@isDevelopCard == false")]
        [SerializeField] private string cardId;
        
        [ShowIf("@isDevelopCard == false")]
        [SerializeField] private List<CardLevelInfo> _levelInfos;

        [SerializeField] private AllyClassType _allyClassType;
        
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool canNotPlay;
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool exhaustIfNotPlay;
        
        
        [FoldoutGroup("卡牌特效")] 
        [SerializeField] private FxInfo fxInfo;
        
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

        public bool IsDevelopCard => isDevelopCard;
        public string CardId => cardId;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        
        public List<CardLevelInfo> LevelInfos => _levelInfos;
        public FxInfo FxInfo => fxInfo;
        public bool CanNotPlay => canNotPlay;
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
        

        
        public AllyClassType AllyClassType
        { get => _allyClassType; set => _allyClassType = value; }

        #endregion

        #region Methods
        public void UpdateDescription()
        {
            
            MyDescription = description;
        }
        
        public CardLevelInfo GetLevelInfo(int level)
        {
            // 如果是開發者卡片，回傳開發者資訊
            if (isDevelopCard)
            {
                return developLevelInfo;
            }
            
            if (level >= LevelInfos.Count)
            {
                throw new Exception($"level {level} 超過 {name} 的 LevelInfos {LevelInfos.Count} 數量");
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