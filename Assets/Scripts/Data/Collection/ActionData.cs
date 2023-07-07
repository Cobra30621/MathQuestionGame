using System;
using System.Collections.Generic;
using System.Text;
using NueGames.Action.MathAction;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.NueExtentions;
using Sirenix.Serialization;
using UnityEngine;

namespace NueGames.Data.Collection
{
    /// <summary>
    /// 用於設計遊戲行為所需資料
    /// </summary>
    [Tooltip("用於設計遊戲行為所需資料")]
    [Serializable]
    public class ActionData
    {
        [Tooltip("遊戲行為類型")]
        [SerializeField] private GameActionType gameActionType;
        
        [Header("數值")]
        [Tooltip("數值，用於傷害大小、增加異常狀態層數等")]
        [SerializeField] private int actionValue;
        [Tooltip("加成數值\n用於如 DamageByQuestioning(根據答對題數造成傷害) 行動")]
        [SerializeField] private int multiplierValue;
        [Tooltip("行為延遲時間")]
        [SerializeField] private float actionDelay;

        [Header("其他")]
        [Tooltip("能力類型\n用於 ApplyPower(賦予能力), DamageByAllyPowerValue(根據能力造成傷害) 等跟能力有關行動")]
        [SerializeField] private PowerType powerType;
        [Tooltip("答題結果類型\n用於 DamageByAnswerCount(根據答題數造成傷害) 等跟答題有關行動")]
        [SerializeField] private AnswerOutcomeType answerOutcomeType;
        
        [Header("特效")]
        [Tooltip("播放特效(FX)，只有在 FXAction 中使用")] 
        [SerializeField] private FxName fxName;
        [SerializeField] private FxSpawnPosition fxSpawnPosition;
        
        
        /// <summary>
        /// 起始的卡組
        /// </summary>
        [Header("卡牌")]
        public PileType SourcePile;
        /// <summary>
        /// 目標的卡組
        /// </summary>
        public PileType TargetPile;
        /// <summary>
        /// 目標卡牌
        /// </summary>
        public CardData TargetCardData;

        
        /// <summary>
        /// 觸發的行動
        /// </summary>
        [Header("觸發的行動")] 
        [OdinSerialize] public List<ActionData> TriggerActionList;
        
        /// <summary>
        /// 遊戲行為類型
        /// </summary>
        public GameActionType GameActionType => gameActionType;
        /// <summary>
        /// 能力類型
        /// 用於 ApplyPower(賦予能力), DamageByAllyPowerValue(根據能力造成傷害) 等跟能力有關行動
        /// </summary>
        public PowerType PowerType => powerType;
        /// <summary>
        /// 答題結果類型\n用於 DamageByAnswerCount(根據答題數造成傷害) 等跟答題有關行動
        /// </summary>
        public AnswerOutcomeType AnswerOutcomeType => answerOutcomeType;
        /// <summary>
        /// 數值，用於傷害大小、增加異常狀態層數等
        /// </summary>
        public int ActionValue => actionValue;
        /// <summary>
        /// 加成數值
        /// 用於如 DamageByQuestioning(根據答對題數造成傷害) 行動
        /// </summary>
        // TODO 重新命名
        public int MultiplierValue => multiplierValue;
        /// <summary>
        /// 行為延遲時間
        /// </summary>
        public float ActionDelay => actionDelay;
        /// <summary>
        /// 播放特效(FX)，只有在 FXAction 中使用
        /// </summary>
        public FxName FxName => fxName;
        /// <summary>
        /// 播放特效產生的位置(FX)，只有在 FXAction 中使用
        /// </summary>
        public FxSpawnPosition FxSpawnPosition => fxSpawnPosition;
        
        #region Editor

#if UNITY_EDITOR
        public void EditActionType(GameActionType newType) =>  gameActionType = newType;
        public void EditPower(PowerType newPowerType) => powerType = newPowerType;
        public void EditActionValue(int newValue) => actionValue = newValue;
        public void EditAdditionValue(int newValue) => multiplierValue = newValue;
        public void EditActionDelay(float newValue) => actionDelay = newValue;

        

#endif


        #endregion
    }

    [Serializable]
    public class CardDescriptionData
    {
        [Header("Text")]
        [SerializeField] private string descriptionText;
        [SerializeField] private bool enableOverrideColor;
        [SerializeField] private Color overrideColor = Color.black;
       
        [Header("Modifer")]
        [SerializeField] private bool useModifier;

        [SerializeField] private CardActionDataListType cardActionDataListType;
        [SerializeField] private int modifiedActionValueIndex;
        [SerializeField] private PowerType modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public CardActionDataListType CardActionDataListType => cardActionDataListType;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public PowerType ModiferStats => modiferStats;
        public bool UsePrefixOnModifiedValue => usePrefixOnModifiedValue;
        public string ModifiedValuePrefix => modifiedValuePrefix;
        public bool OverrideColorOnValueScaled => overrideColorOnValueScaled;
        
        private CombatManager CombatManager => CombatManager.Instance;

        public string GetDescription()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            if (EnableOverrideColor && !string.IsNullOrEmpty(str.ToString())) 
                str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
            
            return str.ToString();
        }

        public string GetModifiedValue(CardData cardData)
        {
            List<ActionData> cardActionDataList = cardData.GetCardActionDataList(cardActionDataListType);
            
            if (cardActionDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=cardActionDataList.Count)
                modifiedActionValueIndex = cardActionDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = cardActionDataList[ModifiedActionValueIndex].ActionValue;
            var modifer = 0;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
               
                if (player)
                {
                    // modifer = player.CharacterStats.StatusDict[ModiferStats].Amount;
                    // Amount += modifer;
                    if(cardActionDataList[ModifiedActionValueIndex].GameActionType == GameActionType.Damage)
                        value = CombatCalculator.GetDamageValue(value, player);

                    // if(cardActionDataList[ModifiedActionValueIndex].GameActionType == GameActionType.Block)
                    //     value = CombatCalculator.GetBlockValue(value, player);
                    // if (modifer != 0)
                    // {
                    //     if (usePrefixOnModifiedValue)
                    //         str.Append(modifiedValuePrefix);
                    // }
                }
            }
           
            str.Append(value);

            if (EnableOverrideColor)
            {
                if (OverrideColorOnValueScaled)
                {
                    if (modifer != 0)
                        str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
                else
                {
                    str.Replace(str.ToString(),ColorExtentions.ColorString(str.ToString(),OverrideColor));
                }
               
            }
            
            return str.ToString();
        }

        #region Editor
#if UNITY_EDITOR
        
        public string GetDescriptionEditor()
        {
            var str = new StringBuilder();
            
            str.Append(DescriptionText);
            
            return str.ToString();
        }

        public string GetModifiedValueEditor(CardData cardData)
        {
            List<ActionData> cardActionDataList = cardData.GetCardActionDataList(cardActionDataListType);
            
            if (cardActionDataList.Count <= 0) return "";
            
            if (ModifiedActionValueIndex>=cardActionDataList.Count)
                modifiedActionValueIndex = cardActionDataList.Count - 1;

            if (ModifiedActionValueIndex<0)
                modifiedActionValueIndex = 0;
            
            var str = new StringBuilder();
            var value = cardActionDataList[ModifiedActionValueIndex].ActionValue;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
                if (player)
                {
                    // TODO: 修正
                    // var modifer = player.CharacterStats.StatusDict[ModiferStats].StatusValue;
                    var modifer = 0;
                    value += modifer;
                
                    if (modifer!= 0)
                        str.Append("*");
                }
            }
           
            str.Append(value);
          
            return str.ToString();
        }
        
        public void EditDescriptionText(string newText) => descriptionText = newText;
        public void EditEnableOverrideColor(bool newStatus) => enableOverrideColor = newStatus;
        public void EditOverrideColor(Color newColor) => overrideColor = newColor;
        public void EditUseModifier(bool newStatus) => useModifier = newStatus;
        public void EditCardActionDataListType(CardActionDataListType newCardActionDataListType) => cardActionDataListType = newCardActionDataListType;
        public void EditModifiedActionValueIndex(int newIndex) => modifiedActionValueIndex = newIndex;
        public void EditModiferStats(PowerType newPowerType) => modiferStats = newPowerType;
        public void EditUsePrefixOnModifiedValues(bool newStatus) => usePrefixOnModifiedValue = newStatus;
        public void EditPrefixOnModifiedValues(string newText) => modifiedValuePrefix = newText;
        public void EditOverrideColorOnValueScaled(bool newStatus) => overrideColorOnValueScaled = newStatus;

#endif
        #endregion
    }
}
