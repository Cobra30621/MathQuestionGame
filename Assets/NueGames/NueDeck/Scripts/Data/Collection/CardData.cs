using System;
using System.Collections.Generic;
using System.Text;
using Assets.NueGames.NueDeck.Scripts.Action;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEditor;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Data.Collection
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
        [SerializeField] private List<CardActionData> cardActionDataList;
        
        [Header("Description")]
        [SerializeField] private List<CardDescriptionData> cardDescriptionDataList;
        [SerializeField] private List<SpecialKeywords> specialKeywordsList;
        
        [Header("Fx")]
        [SerializeField] private AudioActionType audioType;

        [Header("Math Action Settings")]
        [SerializeField] private List<CardActionData> correctCardActionDataList;
        [SerializeField] private List<CardActionData> wrongCardActionDataList;
        [SerializeField] private List<CardActionData> limitedQuestionCardActionDataList;
        [SerializeField] private MathQuestioningActionParameters mathQuestioningActionParameters;

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
        public List<CardActionData> CardActionDataList => cardActionDataList;
        public List<CardActionData> CorrectCardActionDataList => correctCardActionDataList;
        public List<CardActionData> WrongCardActionDataList => wrongCardActionDataList;
        public List<CardActionData> LimitedQuestionCardActionDataList => limitedQuestionCardActionDataList;
        public MathQuestioningActionParameters MathQuestioningActionParameters => mathQuestioningActionParameters;
        
        public List<CardDescriptionData> CardDescriptionDataList => cardDescriptionDataList;
        public List<SpecialKeywords> KeywordsList => specialKeywordsList;
        public AudioActionType AudioType => audioType;
        public string MyDescription { get; set; }
        public RarityType Rarity => rarity;

        public bool ExhaustAfterPlay => exhaustAfterPlay;

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
        
        public List<CardActionData> GetCardActionDataList(CardActionDataListType cardActionDataListType)
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
        public void EditCardActionDataList(List<CardActionData> newCardActionDataList) =>
            cardActionDataList = newCardActionDataList;
        public void EditCorrectCardActionDataList(List<CardActionData> newCardActionDataList) =>
            correctCardActionDataList= newCardActionDataList;
        public void EditWrongCardActionDataList(List<CardActionData> newCardActionDataList) =>
            wrongCardActionDataList = newCardActionDataList;
        public void EditLimitedQuestionCardActionDataList(List<CardActionData> newCardActionDataList) =>
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

    [Serializable]
    public class CardActionData
    {
        [SerializeField] private GameActionType gameActionType;
        [SerializeField] private PowerType powerType;
        [SerializeField] private int actionValue;
        [SerializeField] private int additionValue;
        [SerializeField] private float actionDelay;

        public GameActionType GameActionType => gameActionType;
        public PowerType PowerType => powerType;
        public int ActionValue => actionValue;
        public int AdditionValue => additionValue;
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
            List<CardActionData> cardActionDataList = cardData.GetCardActionDataList(cardActionDataListType);
            
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
                    // modifer = player.CharacterStats.StatusDict[ModiferStats].Value;
                    // Value += modifer;
                    if(cardActionDataList[ModifiedActionValueIndex].GameActionType == GameActionType.Damage)
                        value = CombatCalculator.GetDamageValue(value, player);

                    if(cardActionDataList[ModifiedActionValueIndex].GameActionType == GameActionType.Block)
                        value = CombatCalculator.GetBlockValue(value, player);
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
            List<CardActionData> cardActionDataList = cardData.GetCardActionDataList(cardActionDataListType);
            
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