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
    /// 使用 ScriptableObject 形式定義卡片的基本資訊、等級資料及特效。
    /// 搭配 SODataBase 及 CardDataOverview，用於管理單張卡牌的屬性與設定。
    /// </summary>
    [CreateAssetMenu(fileName = "Card Data", menuName = "Card")]
    public class CardData : SODataBase<CardDataOverview>
    {
        [LabelText("是否為開發者卡片")]
        [SerializeField]
        private bool isDevelopCard;  

        
        #region 一定要設定
        
        [ShowIf("@isDevelopCard == false")]
        [FoldoutGroup("一定要設定")]
        [LabelText("Group Id (卡片唯一 id，對應到表單)")]
        [SerializeField]
        private string cardId; 
        
        [FoldoutGroup("一定要設定")]
        [SerializeField]
        [LabelText("卡片的圖片")]
        private Sprite cardSprite;  
    
        
        [SerializeField]
        [FoldoutGroup("一定要設定")]
        [LabelText("卡片職業類別")]
        private AllyClassType _allyClassType;  
        
        
        [SerializeField]
        [FoldoutGroup("一定要設定")]
        [LabelText("卡牌特效")]
        private FxInfo fxInfo;  
        
        #endregion
        
        #region 卡片效果
        [ShowIf("@isDevelopCard == false")]
        [LabelText("卡片效果(表單)")]
        [SerializeField]
        private List<CardLevelInfo> _levelInfos;
        
        
        [ShowIf("isDevelopCard")]
        [LabelText("卡片效果 (開發者卡片)")]
        [SerializeField]
        private CardLevelInfo developLevelInfo;  

        #endregion

        
        
        #region 暫時用不到
        [FoldoutGroup("暫時用不到")]
        [LabelText("卡片關鍵字顯示")]
        private List<SpecialKeywords> specialKeywordsList;  

        
        [FoldoutGroup("暫時用不到")]
        [SerializeField]
        [LabelText("無法手動打出的卡片")]
        private bool canNotPlay;  
        // 是否不可手動打出
        
        [FoldoutGroup("暫時用不到")]
        [LabelText("虛無: 本回合沒打出，消耗")]
        [SerializeField]
        private bool exhaustIfNotPlay;
        
        [FoldoutGroup("暫時用不到")]
        [LabelText("卡片稀有度")]
        [SerializeField]
        private RarityType rarity; 
        
        #endregion

        
        
        

        #region 快取屬性

        public bool IsDevelopCard => isDevelopCard;
        // public string CardId => DisplayName;
        public string CardId => cardId;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        public List<CardLevelInfo> LevelInfos => _levelInfos;
        public FxInfo FxInfo => fxInfo;
        public bool CanNotPlay => canNotPlay;

   

        public AllyClassType AllyClassType
        {
            get => _allyClassType;
            set => _allyClassType = value;
        }

        #endregion

        #region 方法

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
