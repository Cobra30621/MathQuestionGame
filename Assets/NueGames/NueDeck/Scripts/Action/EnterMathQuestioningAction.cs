using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Managers;
using Question;
using UnityEngine;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class EnterMathQuestioningAction : GameActionBase
    {
        private MathQuestioningActionParameters parameters;
        public GameActionManager GameActionManager => GameActionManager.Instance;
        
        public EnterMathQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardData cardData = cardActionParameter.CardData;
            Duration = cardActionParameter.CardActionData.ActionDelay;

            parameters = cardData.MathQuestioningActionParameters;
            parameters.CorrectActions = GameActionManager.GetGameActions(cardData, cardData.CorrectCardActionDataList , Self, Target);
            parameters.WrongActions = GameActionManager.GetGameActions(cardData, cardData.WrongCardActionDataList, Self, Target);
            parameters.TargetCharacter =cardActionParameter.TargetCharacter;
            parameters.SelfCharacter  = cardActionParameter.SelfCharacter;
        }

        public void SetValue(MathQuestioningActionParameters newParameters)
        {
            parameters = newParameters;
        }
        
        public override void DoAction()
        {
            QuestionManager.Instance.EnterQuestionMode(parameters);

            PlayAudio();
        }
    }
    
    [Serializable]
    public class MathQuestioningActionParameters
    {
        public bool UseLimitedQuestion;
        public int QuestionCount;
        
        public bool UseCorrectAction;
        public int CorrectActionNeedAnswerCount;
        public List<GameActionBase> CorrectActions;

        public bool UseWrongAction;
        public int WrongActionNeedAnswerCount;
        public List<GameActionBase> WrongActions;
        
        public  bool UseTimeCountDown;
        public  int Time;

        public CharacterBase SelfCharacter;
        public CharacterBase TargetCharacter;
        
        public MathQuestioningActionParameters()
        {
            SetQuestionCountValue(false, -1);
            SetCorrectActionValue(false, -1, null);
            SetWrongActionValue(false, -1, null);
            SetTimeValue(false, -1);
            SetCharacter(null, null);
        }

        public void SetQuestionCountValue(bool useLimitedQuestion, int questionCount)
        {
            UseLimitedQuestion = useLimitedQuestion;
            QuestionCount = questionCount;
        }

        public void SetCorrectActionValue(bool useCorrectAction, int correctActionNeedAnswerCount, List<GameActionBase> correctActions)
        {
            UseCorrectAction = useCorrectAction;
            CorrectActionNeedAnswerCount = correctActionNeedAnswerCount;
            CorrectActions = correctActions;
        }

        public void SetWrongActionValue(bool useWrongAction, int wrongActionNeedAnswerCount, List<GameActionBase> wrongActions)
        {
            UseWrongAction = useWrongAction;
            WrongActionNeedAnswerCount = wrongActionNeedAnswerCount;
            WrongActions = wrongActions;
        }
        
        public void SetTimeValue(bool useTimeCountDown, int time)
        {
            UseTimeCountDown = useTimeCountDown;
            Time = time;
        }

        public void SetCharacter(CharacterBase self, CharacterBase target)
        {
            SelfCharacter = self;
            TargetCharacter = target;
        }
    }
}