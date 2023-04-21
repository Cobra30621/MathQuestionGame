using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NueGames.Action.MathAction;
using NueGames.Data.Collection;
using NueGames.Enums;
using NueGames.NueExtentions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NueGames.NueDeck.Editor
{
    public class CardEditorWindow : ExtendedEditorWindow
    {
#if UNITY_EDITOR
        

        private static CardEditorWindow CurrentWindow { get; set; }
        private SerializedObject _serializedObject;

        private const string CardDataDefaultPath = "Assets/Data/Cards/";
       
        #region Cache Card Data
        private static CardData CachedCardData { get; set; }
        private List<CardData> AllCardDataList { get; set; }
        private CardData SelectedCardData { get; set; }
        private string CardId { get; set; }
        private string CardName{ get; set; }
        private int ManaCost{ get; set; }
        private bool NeedPowerToPlay { get; set; }
        private PowerType NeedPowerType { get; set; }
        private int PowerCost { get; set; }
        private Sprite CardSprite{ get; set; }
        private ActionTargetType ActionTargetType { get; set; }
        private bool UsableWithoutTarget{ get; set; }
        private bool ExhaustAfterPlay{ get; set; }
        private List<ActionData> CardActionDataList{ get; set; }
        private List<ActionData> CorrectCardActionDataList{ get; set; }
        private List<ActionData> WrongCardActionDataList{ get; set; }
        private List<ActionData> LimitedQuestionCardActionDataList{ get; set; }

        private bool useMathAction;
        private bool UseMathAction { get => useMathAction; 
            set
            {
                bool previousMathAction = useMathAction;
                useMathAction = value;
                if (previousMathAction != useMathAction)
                {
                    if (useMathAction)  // 使用 MathAction ，自動將 CardAction 的 GameActionType 變成EnterMathQuestioning
                    {
                        CardActionDataList.Clear();
                        ActionData actionData = new ActionData();
                        actionData.EditActionType(GameActionType.EnterMathQuestioning);
                        CardActionDataList.Add(actionData);
                    }
                }
            }
        }
        
        private QuestioningEndJudgeType QuestioningEndJudgeType{ get; set; }
        private int QuestionCount{ get; set; }
        
        private bool UseCorrectAction{ get; set; }
        private int CorrectActionNeedAnswerCount{ get; set; }

        public bool UseWrongAction{ get; set; }
        private int WrongActionNeedAnswerCount{ get; set; }
        
        private bool UseTimeCountDown{ get; set; }
        private int Time{ get; set; }
        
        
        private List<CardDescriptionData> CardDescriptionDataList{ get; set; }
        private List<SpecialKeywords> SpecialKeywordsList{ get; set; }
        private AudioActionType AudioType{ get; set; }

        private RarityType CardRarity { get; set; }

        private void CacheCardData()
        {
            CardId = SelectedCardData.Id;
            CardName = SelectedCardData.CardName;
            ManaCost = SelectedCardData.ManaCost;
            NeedPowerToPlay = SelectedCardData.NeedPowerToPlay;
            NeedPowerType = SelectedCardData.NeedPowerType;
            PowerCost = SelectedCardData.PowerCost;
            CardSprite = SelectedCardData.CardSprite;
            ActionTargetType = SelectedCardData.ActionTargetType;
            UsableWithoutTarget = SelectedCardData.UsableWithoutTarget;
            ExhaustAfterPlay = SelectedCardData.ExhaustAfterPlay;
            CardActionDataList = SelectedCardData.CardActionDataList;
            CorrectCardActionDataList = SelectedCardData.CorrectCardActionDataList;
            WrongCardActionDataList = SelectedCardData.WrongCardActionDataList;
            LimitedQuestionCardActionDataList = SelectedCardData.LimitedQuestionCardActionDataList;
            
            CardDescriptionDataList = SelectedCardData.CardDescriptionDataList.Count>0 ? new List<CardDescriptionData>(SelectedCardData.CardDescriptionDataList) : new List<CardDescriptionData>();
            SpecialKeywordsList = SelectedCardData.KeywordsList.Count>0 ? new List<SpecialKeywords>(SelectedCardData.KeywordsList) : new List<SpecialKeywords>();
            AudioType = SelectedCardData.AudioType;
            CardRarity = SelectedCardData.Rarity;

            MathQuestioningActionParameters parameters = SelectedCardData.MathQuestioningActionParameters;
            QuestioningEndJudgeType = parameters.QuestioningEndJudgeType;
            UseMathAction = parameters.UseMathAction;
            QuestionCount = parameters.QuestionCount;
            UseCorrectAction = parameters.UseCorrectAction;
            CorrectActionNeedAnswerCount = parameters.CorrectActionNeedAnswerCount;
            UseWrongAction = parameters.UseWrongAction;
            WrongActionNeedAnswerCount = parameters.WrongActionNeedAnswerCount;
        }
        
        private void ClearCachedCardData()
        {
            CardId = String.Empty;
            CardName = String.Empty;
            ManaCost = 0;
            NeedPowerToPlay = false;
            NeedPowerType = PowerType.MathMana;
            PowerCost = 0;
            CardSprite = null;
            UsableWithoutTarget = false;
            ActionTargetType = ActionTargetType.Enemy;
            ExhaustAfterPlay = false;
            CardActionDataList?.Clear();
            CorrectCardActionDataList?.Clear();
            WrongCardActionDataList?.Clear();
            LimitedQuestionCardActionDataList?.Clear();
            
            CardDescriptionDataList?.Clear();
            SpecialKeywordsList?.Clear();
            AudioType = AudioActionType.Attack;
            CardRarity = RarityType.Common;

            UseMathAction = false;
            QuestionCount = 0;
            UseCorrectAction = false;
            CorrectActionNeedAnswerCount = 0;
            UseWrongAction = false;
            WrongActionNeedAnswerCount = 0;
        }
        #endregion
        
        #region Setup
        [MenuItem("Tools/NueDeck/Card Editor")]
        public static void OpenCardEditor() =>  CurrentWindow = GetWindow<CardEditorWindow>("Card Editor");
        public static void OpenCardEditor(CardData targetData)
        {
            CachedCardData = targetData;
            OpenCardEditor();
        } 
        
        private void OnEnable()
        {
            AllCardDataList?.Clear();
            AllCardDataList = ListExtentions.GetAllInstances<CardData>().ToList();
            
            if (CachedCardData)
            {
                SelectedCardData = CachedCardData;
                _serializedObject = new SerializedObject(SelectedCardData);
                CacheCardData();
            }
            
            Selection.selectionChanged += Repaint;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= Repaint;
            CachedCardData = null;
            SelectedCardData = null;
        }
        #endregion

        #region Process
        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            
            DrawAllCardButtons();
            EditorGUILayout.Space();
            DrawSelectedCard();
            
            EditorGUILayout.EndHorizontal();
        }
        #endregion
        
        #region Layout Methods
        private Vector2 _allCardButtonsScrollPos;
        private void DrawAllCardButtons()
        {
            _allCardButtonsScrollPos = EditorGUILayout.BeginScrollView(_allCardButtonsScrollPos, GUILayout.Width(150), GUILayout.ExpandHeight(true));
            EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.ExpandHeight(true));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Cards",EditorStyles.boldLabel,GUILayout.Width(50),GUILayout.Height(20));
            
            GUILayout.FlexibleSpace();
            
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.blue;
            if (GUILayout.Button("Refresh",GUILayout.Width(75),GUILayout.Height(20)))
                RefreshCardData();
            GUI.backgroundColor = oldColor;
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();

            foreach (var data in AllCardDataList)
                if (GUILayout.Button(data.CardName,GUILayout.MaxWidth(150)))
                {
                    SelectedCardData = data;
                    _serializedObject = new SerializedObject(SelectedCardData);
                    CacheCardData();
                    GUI.FocusControl(null);
                }

            if (GUILayout.Button("+",GUILayout.MaxWidth(150)))
            {
                CreateNewCard();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        private void CreateNewCard()
        {
            var clone = CreateInstance<CardData>();
            var str = new StringBuilder();
            var count = AllCardDataList.Count;

            str.Append(count + 1).Append("_").Append("new_card_name");
            clone.EditId(str.ToString());
            clone.EditCardName(str.ToString());
            clone.EditCardActionDataList(new List<ActionData>());
            clone.EditCorrectCardActionDataList(new List<ActionData>());
            clone.EditWrongCardActionDataList(new List<ActionData>());
            clone.EditLimitedQuestionCardActionDataList(new List<ActionData>());
            clone.EditCardDescriptionDataList(new List<CardDescriptionData>());
            clone.EditSpecialKeywordsList(new List<SpecialKeywords>());
            clone.EditRarity(RarityType.Common);
            clone.EditQuestioningEndJudgeType(QuestioningEndJudgeType.LimitedQuestionCount);
            var path = str.Insert(0, CardDataDefaultPath).Append(".asset").ToString();
            var uniquePath = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(clone, uniquePath);
            AssetDatabase.SaveAssets();
            RefreshCardData();
            SelectedCardData = AllCardDataList.Find(x => x.Id == clone.Id);
            CacheCardData();
        }

        private void DrawSelectedCard()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
            if (!SelectedCardData)
            {
                EditorGUILayout.LabelField("Select card");
                return;
            }
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
           
            
            ChangeGeneralSettings();
            ChangeCardActionDataList();
            ChangeCardDescriptionDataList();
            ChangeSpecialKeywords();
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            var oldColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Save",GUILayout.Width(100),GUILayout.Height(30)))
                SaveCardData();
            
            GUI.backgroundColor = oldColor;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region Card Data Methods

        private void ChangeId()
        {
            CardId = EditorGUILayout.TextField("Card Id:", CardId);
        }
        private void ChangeCardName()
        {
            CardName = EditorGUILayout.TextField("Card Name:", CardName);
        }
        private void ChangeManaCost()
        {
            ManaCost = EditorGUILayout.IntField("Mana Cost:", ManaCost);
        }

        private void ChangeNeedPowerToPlay()
        {
            NeedPowerToPlay = EditorGUILayout.Toggle("需要消耗能力才能觸發卡片", NeedPowerToPlay);
            if (NeedPowerToPlay)
            {
                NeedPowerType = (PowerType) EditorGUILayout.EnumPopup("需要消耗的能力", NeedPowerType, GUILayout.Width(250));
                PowerCost = EditorGUILayout.IntField("消耗能力數量", PowerCost);
            }
        }

        private void ChangeRarity()
        { 
            CardRarity = (RarityType) EditorGUILayout.EnumPopup("Rarity: ",CardRarity,GUILayout.Width(250));
        }
        private void ChangeCardSprite()
        {
            EditorGUILayout.BeginHorizontal();
            CardSprite = (Sprite)EditorGUILayout.ObjectField("Card Sprite:", CardSprite,typeof(Sprite));
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void ChangeActionTargetType()
        {
            ActionTargetType =
                (ActionTargetType)EditorGUILayout.EnumPopup("Action Target Type: ", ActionTargetType,
                    GUILayout.Width(250));
        }
        
        private void ChangeUsableWithoutTarget()
        {
            UsableWithoutTarget = EditorGUILayout.Toggle("Usable Without Target:", UsableWithoutTarget);
        }
        
        private void ChangeExhaustAfterPlay()
        {
            ExhaustAfterPlay = EditorGUILayout.Toggle("Exhaust after play", ExhaustAfterPlay);
        }
        
        private bool _isGeneralSettingsFolded;
        private Vector2 _generalSettingsScrollPos;
        private void ChangeGeneralSettings()
        {
            _isGeneralSettingsFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isGeneralSettingsFolded, "General Settings");
            if (!_isGeneralSettingsFolded)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
            ChangeId();
            ChangeCardName();
            _generalSettingsScrollPos = EditorGUILayout.BeginScrollView(_generalSettingsScrollPos,GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            ChangeManaCost();
            ChangeNeedPowerToPlay();
            ChangeRarity();
            ChangeActionTargetType();
            ChangeUsableWithoutTarget();
            ChangeExhaustAfterPlay();
            ChangeAudioActionType();
            EditorGUILayout.EndVertical();
            GUILayout.Space(100);
            ChangeCardSprite();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        
        private bool _isCardActionDataListFolded;
        private Vector2 _cardActionScrollPos;
        private void ChangeCardActionDataList()
        {
            _isCardActionDataListFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isCardActionDataListFolded, "Card Actions");
            if (_isCardActionDataListFolded)
            {
                _cardActionScrollPos = EditorGUILayout.BeginScrollView(_cardActionScrollPos,GUILayout.ExpandWidth(true));
                UseMathAction = EditorGUILayout.Toggle("使用數學行動", UseMathAction);

                if (UseMathAction) // 顯示數學行動
                {
                    ChangeMathActions();
                }
                else
                {
                    ChangeSingleCardActionDataList(SelectedCardData.CardActionDataList);
                }

                EditorGUILayout.EndScrollView();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
            
        }
        
        private void ChangeMathActions()
        {
            QuestioningEndJudgeType = (QuestioningEndJudgeType)EditorGUILayout.EnumPopup
                ("答題結束後，如何判斷行動", QuestioningEndJudgeType,GUILayout.Width(250));

            EditorGUILayout.LabelField($"【答題結束後行動】");
            
            if (QuestioningEndJudgeType == QuestioningEndJudgeType.LimitedQuestionCount)
            {
                QuestionCount = EditorGUILayout.IntField("題目數量: ",QuestionCount);
                ChangeSingleCardActionDataList(LimitedQuestionCardActionDataList);
            }
            else if(QuestioningEndJudgeType == QuestioningEndJudgeType.CorrectOrWrongCount)
            {
                UseCorrectAction = EditorGUILayout.Toggle("Use Correct Action", UseCorrectAction);
                if (UseCorrectAction)
                {
                    ChangeSingleCardActionDataList(CorrectCardActionDataList);
                    CorrectActionNeedAnswerCount = EditorGUILayout.IntField("啟動行動，需答對題數", CorrectActionNeedAnswerCount);
                    EditorGUILayout.Space(20);
                }
                    
                UseWrongAction = EditorGUILayout.Toggle("Use Wrong Action", UseWrongAction);
                if (UseWrongAction)
                {
                    ChangeSingleCardActionDataList(WrongCardActionDataList);
                    WrongActionNeedAnswerCount = EditorGUILayout.IntField("啟動行動，需答錯題數", WrongActionNeedAnswerCount);
                    EditorGUILayout.Space(20);
                }
            }
        }
        
        
        private void ChangeSingleCardActionDataList(List<ActionData> cardActionDataList)
        {
            
            GUIStyle headStyle = new GUIStyle();
            headStyle.fontStyle = FontStyle.Bold;
            headStyle.normal.textColor = Color.white;
            headStyle.fontSize = 20;
            EditorGUILayout.BeginHorizontal();
            
            List<ActionData> _removedList = new List<ActionData>();
            for (var i = 0; i < cardActionDataList.Count; i++)
            {
                var cardActionData = cardActionDataList[i];
                EditorGUILayout.BeginVertical("box", GUILayout.Width(150), GUILayout.MaxHeight(50));
            
                EditorGUILayout.BeginHorizontal();
                GUIStyle idStyle = new GUIStyle();
                idStyle.fontSize = 16;
                idStyle.fixedWidth = 25;
                idStyle.fixedHeight = 25;
                idStyle.fontStyle = FontStyle.Bold;
                idStyle.normal.textColor = Color.white;
                EditorGUILayout.LabelField($"Action Index: {i}",idStyle);
                
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button("X", GUILayout.MaxWidth(25), GUILayout.MaxHeight(25)))
                    _removedList.Add(cardActionData);
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Separator();
                ChangeActionType(cardActionData);
                
                // var newActionDelay = EditorGUILayout.FloatField("Action Delay: ",cardActionData.ActionDelay);
                // cardActionData.EditActionDelay(newActionDelay);
                
                EditorGUILayout.EndVertical();
            }

            foreach (var cardActionData in _removedList)
                cardActionDataList.Remove(cardActionData);

            if (GUILayout.Button("+",GUILayout.Width(50),GUILayout.Height(50)))
                cardActionDataList.Add(new ActionData());
            
            EditorGUILayout.EndHorizontal();
            
        }

        void ChangeActionType(ActionData actionData)
        {
            var newActionType = (GameActionType)EditorGUILayout.EnumPopup("Action Type",actionData.GameActionType,GUILayout.Width(250));

            if (newActionType == GameActionType.ApplyPower || 
                newActionType == GameActionType.DamageByAllyPowerValue ||
                newActionType == GameActionType.ApplyPowerToAllEnemy ||
                newActionType == GameActionType.MultiplyPower)
            {
                var newPowerType = (PowerType)EditorGUILayout.EnumPopup("Power Type",actionData.PowerType,GUILayout.Width(250));
                actionData.EditPower(newPowerType);
            }
            
            var newActionValue = EditorGUILayout.IntField("Action Amount: ",actionData.ActionValue);
            actionData.EditActionValue(newActionValue);
            
            if (newActionType == GameActionType.DamageByQuestioning)
            {
                var newAdditionValue = EditorGUILayout.IntField("AdditionValue: ",actionData.AdditionValue);
                actionData.EditAdditionValue(newAdditionValue);
            }
            
            actionData.EditActionType(newActionType);
        }
        
        
        private bool _isDescriptonDataListFolded;
        private Vector2 _descriptionDataScrollPos;
      
        private void ChangeCardDescriptionDataList()
        {
            _isDescriptonDataListFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isDescriptonDataListFolded, "Description");
            if (_isDescriptonDataListFolded)
            {
                _descriptionDataScrollPos = EditorGUILayout.BeginScrollView(_descriptionDataScrollPos,GUILayout.ExpandWidth(true));
                EditorGUILayout.BeginHorizontal();
                List<CardDescriptionData> _removedList = new List<CardDescriptionData>();
                for (var i = 0; i < CardDescriptionDataList.Count; i++)
                {
                    var descriptionData = CardDescriptionDataList[i];
                    
                    EditorGUILayout.BeginVertical("box", GUILayout.Width(175), GUILayout.Height(100));
                    EditorGUILayout.BeginHorizontal();
                    descriptionData.EditUseModifier(EditorGUILayout.ToggleLeft("Use Modifier", descriptionData.UseModifier,
                        GUILayout.Width(125), GUILayout.Height(25)));
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(25)))
                        _removedList.Add(descriptionData);
                    EditorGUILayout.EndHorizontal();

                    if (descriptionData.UseModifier)
                    {
                        var newCardActionDataListType = (CardActionDataListType )EditorGUILayout.EnumPopup("CardActionDataListType",descriptionData.CardActionDataListType,GUILayout.Width(250));
                        descriptionData.EditCardActionDataListType(newCardActionDataListType);
                    }
                    
                    descriptionData.EditEnableOverrideColor(EditorGUILayout.ToggleLeft("Override Color", descriptionData.EnableOverrideColor,
                        GUILayout.Width(125), GUILayout.Height(25)));
                    
                    EditorGUILayout.Space(5);

                    if (descriptionData.EnableOverrideColor)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.Separator();
                        descriptionData.EditOverrideColor(EditorGUILayout.ColorField(descriptionData.OverrideColor));
                        descriptionData.EditOverrideColorOnValueScaled(EditorGUILayout.ToggleLeft("Color on scale", descriptionData.OverrideColorOnValueScaled,
                            GUILayout.Width(125), GUILayout.Height(25)));
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space(5);
                    }
                    
                    EditorGUILayout.BeginHorizontal();
                    if (descriptionData.UseModifier)
                    {
                        EditorGUILayout.BeginVertical();
                        
                        var clampedIndex = Mathf.Clamp(descriptionData.ModifiedActionValueIndex, 0,
                            CardActionDataList.Count - 1);
                        descriptionData.EditModifiedActionValueIndex(
                            EditorGUILayout.IntField("Action Index:",clampedIndex));
                       
                        descriptionData.EditModiferStats((PowerType)EditorGUILayout.EnumPopup("Scale Type:",descriptionData.ModiferStats));
                        descriptionData.EditUsePrefixOnModifiedValues(EditorGUILayout.ToggleLeft("Use prefix on scale", descriptionData.UsePrefixOnModifiedValue,
                            GUILayout.Width(125), GUILayout.Height(25)));
                        if (descriptionData.UsePrefixOnModifiedValue)
                            descriptionData.EditPrefixOnModifiedValues(
                                EditorGUILayout.TextField("Prefix:",descriptionData.ModifiedValuePrefix));
                        
                        EditorGUILayout.EndVertical();
                    }
                    else
                    { 
                        var desc = EditorGUILayout.TextArea(descriptionData.DescriptionText, GUILayout.Width(150),
                            GUILayout.Height(50));

                        // var hasExhaust = CardActionDataList.Find(x => x.GameActionType == GameActionType.Exhaust);
                        // if (ExhaustAfterPlay || hasExhaust != null)
                        // {
                        //     desc += " Exhaust ";
                        // }
                        //
                        descriptionData.EditDescriptionText(desc);
                    }
                    
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
                
                

                foreach (var cardActionData in _removedList)
                    CardDescriptionDataList.Remove(cardActionData);

                if (GUILayout.Button("+",GUILayout.Width(50),GUILayout.Height(50)))
                    CardDescriptionDataList.Add(new CardDescriptionData());
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
                
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal("box");
                var str = new StringBuilder();
                foreach (var cardDescriptionData in CardDescriptionDataList)
                {
                    str.Append(cardDescriptionData.UseModifier
                        ? cardDescriptionData.GetModifiedValueEditor(SelectedCardData)
                        : cardDescriptionData.GetDescriptionEditor());
                }
                
                GUIStyle headStyle = new GUIStyle();
                headStyle.fontStyle = FontStyle.Bold;
                headStyle.normal.textColor = Color.white;
                EditorGUILayout.BeginVertical();
                
                EditorGUILayout.LabelField("Description Preview",headStyle);
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField(str.ToString());
                EditorGUILayout.Separator();

               

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        private Vector2 _specialKeywordScrool;
        private bool _isSpecialKeywordsFolded;
        private void ChangeSpecialKeywords()
        {
            _isSpecialKeywordsFolded =EditorGUILayout.BeginFoldoutHeaderGroup(_isSpecialKeywordsFolded, "Special Keywords");
            if (!_isSpecialKeywordsFolded)
            {
                EditorGUILayout.EndFoldoutHeaderGroup();
                return;
            }
           
            EditorGUILayout.BeginVertical("box");
            _specialKeywordScrool = EditorGUILayout.BeginScrollView(_specialKeywordScrool);
            EditorGUILayout.BeginHorizontal();
            var specialKeyCount = Enum.GetNames(typeof(SpecialKeywords));

            for (var i = 0; i < specialKeyCount.Length; i++)
            {
                EditorGUILayout.BeginVertical(GUILayout.Width(100));
                var hasKey = SpecialKeywordsList.Contains((SpecialKeywords)i);
                EditorGUILayout.LabelField(((SpecialKeywords)i).ToString());
                var newValue = EditorGUILayout.Toggle(hasKey);
                if (newValue)
                {
                    if (!SpecialKeywordsList.Contains((SpecialKeywords)i))
                        SpecialKeywordsList.Add((SpecialKeywords)i);
                }
                else
                {
                    if (SpecialKeywordsList.Contains((SpecialKeywords)i))
                        SpecialKeywordsList.Remove((SpecialKeywords)i);
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
        private void ChangeAudioActionType()
        {
            AudioType = (AudioActionType)EditorGUILayout.EnumPopup("Audio Type:",AudioType);
        }

       
        private void SaveCardData()
        {
            if (!SelectedCardData) return;
            
            SelectedCardData.EditId(CardId);
            SelectedCardData.EditCardName(CardName);
            SelectedCardData.EditManaCost(ManaCost);
            SelectedCardData.EditNeedPowerToPlay(NeedPowerToPlay);
            SelectedCardData.EditNeedPowerType(NeedPowerType);
            SelectedCardData.EditPowerCost(PowerCost);
            
            SelectedCardData.EditCardSprite(CardSprite);
            SelectedCardData.EditActionTargetType(ActionTargetType);
            SelectedCardData.EditUsableWithoutTarget(UsableWithoutTarget);
            SelectedCardData.EditExhaustAfterPlay(ExhaustAfterPlay);
            
            SelectedCardData.EditCardActionDataList(CardActionDataList);
            SelectedCardData.EditCorrectCardActionDataList(CorrectCardActionDataList);
            SelectedCardData.EditWrongCardActionDataList(WrongCardActionDataList);
            SelectedCardData.EditLimitedQuestionCardActionDataList(LimitedQuestionCardActionDataList);
            
            SelectedCardData.EditUseMathAction(UseMathAction);
            SelectedCardData.EditQuestioningEndJudgeType(QuestioningEndJudgeType);
            SelectedCardData.EditQuestionCount(QuestionCount);
            SelectedCardData.EditUseCorrectAction(UseCorrectAction);
            SelectedCardData.EditCorrectActionNeedAnswerCount(CorrectActionNeedAnswerCount);
            SelectedCardData.EditUseWrongAction(UseWrongAction);
            SelectedCardData.EditWrongActionNeedAnswerCount(WrongActionNeedAnswerCount);
            
            SelectedCardData.EditCardDescriptionDataList(CardDescriptionDataList);
            SelectedCardData.EditSpecialKeywordsList(SpecialKeywordsList);
            SelectedCardData.EditAudioType(AudioType);
            
            EditorUtility.SetDirty(SelectedCardData);
            AssetDatabase.SaveAssets();
            
            SelectedCardData.EditFileName();
        }
        
        private void RefreshCardData()
        {
            SelectedCardData = null;
            ClearCachedCardData();
            AllCardDataList?.Clear();
            AllCardDataList = ListExtentions.GetAllInstances<CardData>().ToList();
        }
        #endregion
#endif
    }
}
