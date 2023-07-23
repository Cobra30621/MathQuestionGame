using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using NueGames.Action;
using NueGames.Action.MathAction;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.NueExtentions;
using NueGames.Power;
using Sirenix.OdinInspector;
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
        #region 基礎參數

        [FoldoutGroup("基礎參數")]
        [PropertyTooltip("行為名稱")]
        public ActionName actionName;
        
        [DetailedInfoBox("數值計算公式...", 
            "BaseValue + MultiplierValue * MultiplierAmount" +
            "\n基礎數值 + 加成數值 * 加成數量" +
            "\nMultiplierAmount(加成數量) 的參數在 GameAction 中")]
        
        [FoldoutGroup("基礎參數")]
        [PropertyTooltip("基礎數值，用於傷害大小、增加異常狀態層數等")]
        public int BaseValue;
        
        
        [FoldoutGroup("基礎參數")]
        [PropertyTooltip("加成數值")]
        public float MultiplierValue;
        

        [FoldoutGroup("基礎參數")]
        [PropertyTooltip("行為延遲時間")]
        public float Delay;
        
        #endregion
        
        #region 播放特效

        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效名稱")]
        public FxName FxName;
        
        [FoldoutGroup("播放的特效")]
        [PropertyTooltip("特效產生處")]
        public FxSpawnPosition FxSpawnPosition;
        

        #endregion
        
        #region 傷害參數
        [FoldoutGroup("傷害參數")]
        [PropertyTooltip("固定傷害，不受能力影響")]
        public bool FixDamage;
        
        [FoldoutGroup("傷害參數")]
        [PropertyTooltip("無視護盾的傷害")]
        public bool CanPierceArmor;
        

        #endregion
        
        #region 能力參數

        [InfoBox("用於與能力(Power)相關的遊戲行為")]
        
        [FoldoutGroup("能力參數")]
        [PropertyTooltip("用於 ApplyPower(賦予能力), DamageByAllyPowerValue(根據能力造成傷害) 等跟能力有關行動")]
        public PowerName powerName;

        

        #endregion
        
        #region 卡組參數

        [InfoBox("用於與卡組相關的遊戲行為")]
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("起始的卡組")]
        public PileType SourcePile;
        
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("目標的卡組")]
        public PileType TargetPile;
        
        [FoldoutGroup("卡組參數")]
        [PropertyTooltip("目標卡牌")]
        public CardData TargetCardData;

        #endregion

        #region 觸發遊戲行為

        
        [FoldoutGroup("觸發的行為")]
        [InfoBox("用於會觸發其他遊戲行為的遊戲行為")]
        [PropertyTooltip("觸發的行為")]
        [OdinSerialize] public List<ActionData> TriggerActionList;
        
        

        #endregion


        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
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
        [SerializeField] private PowerName modiferStats;
        [SerializeField] private bool usePrefixOnModifiedValue;
        [SerializeField] private string modifiedValuePrefix = "*";
        [SerializeField] private bool overrideColorOnValueScaled;

        public string DescriptionText => descriptionText;
        public bool EnableOverrideColor => enableOverrideColor;
        public Color OverrideColor => overrideColor;
        public bool UseModifier => useModifier;
        public CardActionDataListType CardActionDataListType => cardActionDataListType;
        public int ModifiedActionValueIndex => modifiedActionValueIndex;
        public PowerName ModiferStats => modiferStats;
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
            var value = cardActionDataList[ModifiedActionValueIndex].BaseValue;
            var modifer = 0;
            if (CombatManager)
            {
                var player = CombatManager.CurrentMainAlly;
               
                if (player)
                {
                    // TODO 獲得加成數值
                    // modifer = player.CharacterStats.StatusDict[ModiferStats].Amount;
                    // Amount += modifer;
                    // if(cardActionDataList[ModifiedActionValueIndex].actionName == ActionName.Damage)
                    //     value = CombatCalculator.GetDamageValue(value, player);

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
            var value = cardActionDataList[ModifiedActionValueIndex].BaseValue;
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
        public void EditModiferStats(PowerName newPowerName) => modiferStats = newPowerName;
        public void EditUsePrefixOnModifiedValues(bool newStatus) => usePrefixOnModifiedValue = newStatus;
        public void EditPrefixOnModifiedValues(string newText) => modifiedValuePrefix = newText;
        public void EditOverrideColorOnValueScaled(bool newStatus) => overrideColorOnValueScaled = newStatus;

#endif
        #endregion
    }
}
