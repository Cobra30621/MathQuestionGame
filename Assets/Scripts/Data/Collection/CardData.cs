using System;
using System.Collections.Generic;
using System.Text;
using NueGames.Action.MathAction;
using NueGames.Characters;
using NueGames.Combat;
using NueGames.Enums;
using NueGames.Managers;
using NueGames.NueExtentions;
using UnityEditor;
using UnityEngine;

namespace NueGames.Data.Collection
{
    [CreateAssetMenu(fileName = "Card Data",menuName = "NueDeck/Collection/Card",order = 0)]
    public class CardData : ScriptableObject
    {
        [Header("Card Profile")] 
        [SerializeField] private string id;
        [SerializeField] private string cardName;
        [SerializeField] private int manaCost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;
        [SerializeField] private bool needPowerToPlay;
        [SerializeField] private PowerType needPowerType;
        [SerializeField] private int powerCost;
        
        [Header("Action Settings")]
        [SerializeField] private ActionTargetType actionTargetType;
        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<ActionData> cardActionDataList;
        
        [Header("Description")]
        [SerializeField] private List<CardDescriptionData> cardDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        
        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        [Header("Math Action Settings")]
        [SerializeField] private List<ActionData> correctCardActionDataList;
        [SerializeField] private List<ActionData> wrongCardActionDataList;
        [SerializeField] private List<ActionData> limitedQuestionCardActionDataList;
        [SerializeField] private MathQuestioningActionParameters mathQuestioningActionParameters = new MathQuestioningActionParameters();

        [Header("Random Action Settings")] 
        [SerializeField] private bool useRandomAction;

        [SerializeField] private RandomActionData randomActionData;
        
        #region Cache
        public string Id => id;
        public bool UsableWithoutTarget => usableWithoutTarget;
        public ActionTargetType ActionTargetType => actionTargetType;
        public int ManaCost => manaCost;
        public bool NeedPowerToPlay => needPowerToPlay;
        public PowerType NeedPowerType => needPowerType;
        public int PowerCost => powerCost;
        public string CardName => cardName;
        public Sprite CardSprite => cardSprite;
        public List<ActionData> CardActionDataList => cardActionDataList;
        public List<ActionData> CorrectCardActionDataList => correctCardActionDataList;
        public List<ActionData> WrongCardActionDataList => wrongCardActionDataList;
        public List<ActionData> LimitedQuestionCardActionDataList => limitedQuestionCardActionDataList;
        public MathQuestioningActionParameters MathQuestioningActionParameters => mathQuestioningActionParameters;
        
        public List<CardDescriptionData> CardDescriptionDataList => cardDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;

        public bool ExhaustAfterPlay => exhaustAfterPlay;
        public bool UseRandomAction => useRandomAction;
        public RandomActionData RandomActionData => randomActionData;

        #endregion
        
        #region Methods
        public void UpdateDescription()
        {
            var str = new StringBuilder();

            foreach (var descriptionData in cardDescriptionDataList)
            {
                str.Append(descriptionData.UseModifier
                    ? descriptionData.GetModifiedValue(this)
                    : descriptionData.GetDescription());
            }
            
            MyDescription = str.ToString();
        }
        
        public List<ActionData> GetCardActionDataList(CardActionDataListType cardActionDataListType)
        {
            switch (cardActionDataListType)
            {
                case CardActionDataListType.Normal:
                    return cardActionDataList;
                case CardActionDataListType.Correct:
                    return correctCardActionDataList;
                case CardActionDataListType.Wrong:
                    return wrongCardActionDataList;
                default:
                    Debug.LogWarning($"請設定{cardActionDataListType}的Data List");
                    return cardActionDataList;
            }
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        public void EditCardName(string newName) => cardName = newName;
        public void EditId(string newId) => id = newId;
        public void EditManaCost(int newCost) => manaCost = newCost;
        public void EditNeedPowerToPlay(bool newValue) => needPowerToPlay = newValue;
        public void EditNeedPowerType(PowerType powerType) => needPowerType = powerType;
        public void EditPowerCost(int newValue) => powerCost = newValue;
        public void EditRarity(RarityType targetRarity) => rarity = targetRarity;
        public void EditCardSprite(Sprite newSprite) => cardSprite = newSprite;
        public void EditUsableWithoutTarget(bool newStatus) => usableWithoutTarget = newStatus;
        
        public void EditActionTargetType(ActionTargetType newActionTargetType) =>
            actionTargetType = newActionTargetType;
        public void EditExhaustAfterPlay(bool newStatus) => exhaustAfterPlay = newStatus;
        public void EditCardActionDataList(List<ActionData> newCardActionDataList) =>
            cardActionDataList = newCardActionDataList;
        public void EditCorrectCardActionDataList(List<ActionData> newCardActionDataList) =>
            correctCardActionDataList= newCardActionDataList;
        public void EditWrongCardActionDataList(List<ActionData> newCardActionDataList) =>
            wrongCardActionDataList = newCardActionDataList;
        public void EditLimitedQuestionCardActionDataList(List<ActionData> newCardActionDataList) =>
           limitedQuestionCardActionDataList = newCardActionDataList;
        
        public void EditUseMathAction(bool newValue)=> mathQuestioningActionParameters.UseMathAction = newValue;
        public void EditQuestioningEndJudgeType(QuestioningEndJudgeType newValue) =>
            mathQuestioningActionParameters.QuestioningEndJudgeType = newValue;

        public void EditQuestionCount(int newValue) => mathQuestioningActionParameters.QuestionCount = newValue;
        public void EditUseCorrectAction(bool newValue) => mathQuestioningActionParameters.UseCorrectAction = newValue;

        public void EditCorrectActionNeedAnswerCount(int newValue) =>
            mathQuestioningActionParameters.CorrectActionNeedAnswerCount = newValue;
        public void EditUseWrongAction(bool newValue) => mathQuestioningActionParameters.UseWrongAction = newValue;

        public void EditWrongActionNeedAnswerCount(int newValue) =>
            mathQuestioningActionParameters.WrongActionNeedAnswerCount = newValue;
        public void EditCardDescriptionDataList(List<CardDescriptionData> newCardDescriptionDataList) =>
            cardDescriptionDataList = newCardDescriptionDataList;
        public void EditSpecialKeywordsList(List<SpecialKeywords> newSpecialKeywordsList) =>
            specialKeywordsList = newSpecialKeywordsList;
        public void EditAudioType(AudioActionType newAudioActionType) => audioType = newAudioActionType;
        
        public void EditFileName()
        {
            var path = AssetDatabase.GetAssetPath(this);
            Debug.Log(path);
            AssetDatabase.RenameAsset(path, Id);
        }
#endif

        #endregion

    }

    /// <summary>
    /// 用於設計遊戲行為所需資料
    /// </summary>
    [Tooltip("用於設計遊戲行為所需資料")]
    [Serializable]
    public class ActionData
    {
        [Tooltip("遊戲行為類型")]
        [SerializeField] private GameActionType gameActionType;
        [Tooltip("能力類型\n用於 ApplyPower(賦予能力), DamageByAllyPowerValue(根據能力造成傷害) 等跟能力有關行動")]
        [SerializeField] private PowerType powerType;
        [Tooltip("答題結果類型\n用於 DamageByAnswerCount(根據答題數造成傷害) 等跟答題有關行動")]
        [SerializeField] private AnswerOutcomeType answerOutcomeType;
        [Tooltip("數值，用於傷害大小、增加異常狀態層數等")]
        [SerializeField] private int actionValue;
        [Tooltip("加成數值\n用於如 DamageByQuestioning(根據答對題數造成傷害) 行動")]
        [SerializeField] private int additionValue;
        [Tooltip("行為延遲時間")]
        [SerializeField] private float actionDelay;
        
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
        public int AdditionValue => additionValue;
        /// <summary>
        /// 行為延遲時間
        /// </summary>
        public float ActionDelay => actionDelay;
        
        
        #region Editor

#if UNITY_EDITOR
        public void EditActionType(GameActionType newType) =>  gameActionType = newType;
        public void EditPower(PowerType newPowerType) => powerType = newPowerType;
        public void EditActionValue(int newValue) => actionValue = newValue;
        public void EditAdditionValue(int newValue) => additionValue = newValue;
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