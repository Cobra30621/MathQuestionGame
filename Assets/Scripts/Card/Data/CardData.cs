using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Display;
using Effect.Parameters;
using NueGames.Enums;
using rStarTools.Scripts.StringList;
using Save;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;

namespace Card.Data
{
    /// <summary>
    /// 卡片資料物件
    /// 透過 SODataBase 與 CardDataOverview 結合，管理單張卡牌的各種設定與屬性
    /// </summary>
    [CreateAssetMenu(fileName = "Card Data", menuName = "Collection/Card", order = 0)]
    public class CardData : SODataBase<CardDataOverview>
    {
        #region Inspector 設定
        
        [LabelText("開發者卡片")]
        [SerializeField]
        private bool isDevelopCard;  
        // 是否為開發者專用卡片，切換不同資料來源
        
        [ShowIf("isDevelopCard")]
        [LabelText("卡片效果 (開發者)")]
        [SerializeField]
        private CardLevelInfo developLevelInfo;  
        // 當 isDevelopCard = true 時，使用此等級資訊
        
        [ShowIf("@isDevelopCard == false")]
        [SerializeField]
        private string cardId;  
        // 卡片群組 ID (非開發者卡片時使用)
        
        [ShowIf("@isDevelopCard == false")]
        [SerializeField]
        private List<CardLevelInfo> _levelInfos;  
        // 存放多層級的卡片資訊列表
        
        [SerializeField]
        private AllyClassType _allyClassType;  
        // 卡片所屬職業／陣營類型
        
        #endregion

        #region 數值參數
        
        [FoldoutGroup("數值參數")]
        [SerializeField]
        private bool canNotPlay;  
        // 是否不可手動打出
        
        [FoldoutGroup("數值參數")]
        [SerializeField]
        private bool exhaustIfNotPlay;  
        // 如果未打出，回合結束後是否疲憊（移除）
        
        #endregion

        #region 卡牌特效與動畫設定
        
        [FoldoutGroup("卡牌特效")]
        [SerializeField]
        private FxInfo fxInfo;  
        // 卡片釋放時的特效設定
        
        [FoldoutGroup("角色動畫")]
        [SerializeField]
        private bool useDefaultAttackFeedback;  
        // 是否使用預設攻擊反饋動畫
        
        [FoldoutGroup("角色動畫")]
        [SerializeField]
        private bool useCustomFeedback;  
        // 是否使用自訂反饋動畫
        
        [FoldoutGroup("角色動畫")]
        [ShowIf("useCustomFeedback")]
        [SerializeField]
        private CustomerFeedbackSetting customerFeedback;  
        // 自訂反饋動畫的設定，須搭配 useCustomFeedback = true
        
        #endregion

        #region 顯示設定
        
        [FoldoutGroup("卡牌顯示")]
        [SerializeField]
        private Sprite cardSprite;  
        // 卡面圖片
        
        [FoldoutGroup("卡牌顯示")]
        [SerializeField]
        private RarityType rarity;  
        // 稀有度
        
        [FoldoutGroup("卡牌顯示")]
        [TextArea(4, 10)]
        [SerializeField]
        private string description;  
        // 原始描述文字
        
        [FoldoutGroup("顯示提示字")]
        private List<SpecialKeywords> specialKeywordsList;  
        // 卡片特殊關鍵字列表（運行階段賦值）
        
        #endregion

        #region 快取屬性

        public bool IsDevelopCard => isDevelopCard;
        public string CardId => cardId;
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
                if (customerFeedback == null) return false;
                return useCustomFeedback;
            }
        }

        public string CustomFeedbackKey
        {
            get
            {
                if (customerFeedback == null) return string.Empty;
                return customerFeedback.customFeedbackKey;
            }
        }

        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public string MyDescription { get; set; }

        public AllyClassType AllyClassType
        {
            get => _allyClassType;
            set => _allyClassType = value;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 將原始 description 複製到 MyDescription 以便後續動態替換
        /// </summary>
        public void UpdateDescription()
        {
            MyDescription = description;
        }

        /// <summary>
        /// 根據等級取得對應的 CardLevelInfo
        /// </summary>
        /// <param name="level">0 為第一階，依此類推</param>
        public CardLevelInfo GetLevelInfo(int level)
        {
            // 開發者卡片直接回傳 developLevelInfo
            if (isDevelopCard)
                return developLevelInfo;

            // 等級超出清單範圍則拋例外
            if (level >= _levelInfos.Count)
                throw new Exception($"Level {level} 超過卡片 {_levelInfos.Count} 個等級資訊");

            return _levelInfos[level];
        }

        /// <summary>
        /// 動態替換並設置卡片的等級資訊列表
        /// </summary>
        /// <param name="levelInfos">新等級資訊列表</param>
        public void SetCardLevels(List<CardLevelInfo> levelInfos)
        {
            _levelInfos = levelInfos;
        }

        #endregion
    }
}
