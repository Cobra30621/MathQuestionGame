using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardAction;
using NueGames.Action;
using NueGames.Action.MathAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using NueGames.Power;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace NueGames.Data.Collection
{
    [CreateAssetMenu(fileName = "Card Data", menuName = "NueDeck/Collection/Card", order = 0)]
    public class CardData : SerializedScriptableObject ,ISerializeReferenceByAssetGuid
    {
        [FoldoutGroup("卡片行為")]
        [DetailedInfoBox("如何創建新的卡片行為(CardAction)...", 
            "如何創建新的卡片行為(CardAction)\n" +
            "請去 Assets/Scripts/CardAction 資料夾中，創建新的 cs 檔。\n" +
            "技術文件放在 Assets/Scripts/CardAction/CardActionBase.cs 的開頭註解" )]
        [SerializeField] private CardActionBase cardAction;
     
        [FoldoutGroup("數值參數")]
        [SerializeField]private ActionTargetType actionTargetType;
        [FoldoutGroup("數值參數")]
        [SerializeField] private int manaCost;
        [FoldoutGroup("數值參數")]
        [SerializeField] private bool exhaustAfterPlay;
        
        [FoldoutGroup("卡牌特效")]
        [SerializeField] private  FxName  fxName;
        [FoldoutGroup("卡牌特效")]
        [SerializeField] private FxSpawnPosition fxSpawnPosition;

        [FoldoutGroup("角色動畫")] 
        [SerializeField] private bool useDefaultAttackFeedback;
        [FoldoutGroup("角色動畫")] 
        [SerializeField] private CustomerFeedbackSetting customerFeedback = new CustomerFeedbackSetting();
        
        
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private string cardName;
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private Sprite cardSprite;
        [FoldoutGroup("卡牌顯示")]
        [SerializeField] private RarityType rarity;
        [FoldoutGroup("卡牌顯示")]
        [TextArea(4, 10)] [SerializeField] private string description;

        private List<SpecialKeywords> specialKeywordsList;


        #region Cache
        public string CardName => cardName;
        public int ManaCost => manaCost;
        public Sprite CardSprite => cardSprite;
        public RarityType Rarity => rarity;
        public ActionTargetType ActionTargetType => actionTargetType;
        
        public CardActionBase CardAction => cardAction;

        public FxName FxName => fxName;
        public FxSpawnPosition FxSpawnPosition => fxSpawnPosition;
        public bool UseDefaultAttackFeedback => useDefaultAttackFeedback;
        public bool UseCustomFeedback
        {
            get
            {
                if (customerFeedback == null)
                    return false;
                return customerFeedback.useCustomFeedback;
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


        #endregion
        
    }

}