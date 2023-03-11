using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Card;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Combat;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Enums;
using Question;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    public class EnterMathQuestioningAction : GameActionBase
    {
        public MathQuestioningActionParameters parameters;
        
        public EnterMathQuestioningAction()
        {
            FxType = FxType.Attack;
            AudioActionType = AudioActionType.Attack;
        }
        
        public override void SetValue(CardActionParameter cardActionParameter)
        {
            CardActionData data = cardActionParameter.CardActionData;
            parameters = data.MathQuestioningActionParameters;
            TargetCharacter = cardActionParameter.TargetCharacter;
            Duration = cardActionParameter.CardActionData.ActionDelay;
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
        public List<CardActionData> CorrectActions;

        public bool UseWrongAction;
        public int WrongActionNeedAnswerCount;
        public List<CardActionData> WrongActions;
        
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

        public void SetCorrectActionValue(bool useCorrectAction, int correctActionNeedAnswerCount, List<CardActionData> correctActions)
        {
            UseCorrectAction = useCorrectAction;
            CorrectActionNeedAnswerCount = correctActionNeedAnswerCount;
            CorrectActions = correctActions;
        }

        public void SetWrongActionValue(bool useWrongAction, int wrongActionNeedAnswerCount, List<CardActionData> wrongActions)
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