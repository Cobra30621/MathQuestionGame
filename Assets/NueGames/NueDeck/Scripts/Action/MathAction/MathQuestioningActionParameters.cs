using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters;
using NueGames.NueDeck.Scripts.Enums;

namespace Assets.NueGames.NueDeck.Scripts.Action
{
    [Serializable]
    public class MathQuestioningActionParameters
    {
        public bool UseMathAction;
        public QuestioningEndJudgeType QuestioningEndJudgeType;
        
        public int QuestionCount;
        public List<GameActionBase> LimitedQuestionAction;
        
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
            SetJudgeType(false, QuestioningEndJudgeType.LimitedQuestionCount);
            SetQuestionCountValue( -1, null);
            SetCorrectActionValue(false, -1, null);
            SetWrongActionValue(false, -1, null);
            SetTimeValue(false, -1);
            SetCharacter(null, null);
        }

        public void SetJudgeType(bool useMathAction, QuestioningEndJudgeType questioningEndJudgeType)
        {
            UseMathAction = useMathAction;
            QuestioningEndJudgeType = questioningEndJudgeType;
        }

        public void SetQuestionCountValue(int questionCount, List<GameActionBase> limitedQuestionActions)
        {
            QuestionCount = questionCount;
            LimitedQuestionAction = limitedQuestionActions;
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