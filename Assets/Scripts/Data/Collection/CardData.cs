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
    [CreateAssetMenu(fileName = "Card Data", menuName = "NueDeck/Collection/Card", order = 0)]
    public class CardData : ScriptableObject
    {
        [Header("Card Profile")] [SerializeField]
        private string id;

        [SerializeField] private string cardName;
        [SerializeField] private int manaCost;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private RarityType rarity;
        [SerializeField] private bool needPowerToPlay;
        [SerializeField] private PowerType needPowerType;
        [SerializeField] private int powerCost;

        [Header("Action Settings")] [SerializeField]
        private ActionTargetType actionTargetType;

        [SerializeField] private bool usableWithoutTarget;
        [SerializeField] private bool exhaustAfterPlay;
        [SerializeField] private List<ActionData> cardActionDataList;

        [Header("Description")] [SerializeField]
        private List<CardDescriptionData> cardDescriptionDataList;

        [SerializeField] private List<SpecialKeywords> specialKeywordsList;

        [Header("Fx")] [SerializeField] private AudioActionType audioType;

        [Header("Math Action Settings")] [SerializeField]
        private List<ActionData> correctCardActionDataList;

        [SerializeField] private List<ActionData> wrongCardActionDataList;
        [SerializeField] private List<ActionData> limitedQuestionCardActionDataList;

        [SerializeField]
        private MathQuestioningActionParameters mathQuestioningActionParameters = new MathQuestioningActionParameters();

        [Header("Random Action Settings")] [SerializeField]
        private bool useRandomAction;

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
            correctCardActionDataList = newCardActionDataList;

        public void EditWrongCardActionDataList(List<ActionData> newCardActionDataList) =>
            wrongCardActionDataList = newCardActionDataList;

        public void EditLimitedQuestionCardActionDataList(List<ActionData> newCardActionDataList) =>
            limitedQuestionCardActionDataList = newCardActionDataList;

        public void EditUseMathAction(bool newValue) => mathQuestioningActionParameters.UseMathAction = newValue;

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

}