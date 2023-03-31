using System;
using System.Collections.Generic;
using NueGames.Characters;
using NueGames.Enums;

namespace NueGames.Action.MathAction
{
    /// <summary>
    /// 數學行動所需參數
    /// </summary>
    [Serializable]
    public class MathQuestioningActionParameters
    {
        /// <summary>
        /// 是否使用數學行為
        /// </summary>
        public bool UseMathAction;
        /// <summary>
        /// 答題結束的判斷依據
        /// </summary>
        public QuestioningEndJudgeType QuestioningEndJudgeType;
 
        /// <summary>
        /// 問題數量
        /// </summary>
        public int QuestionCount;
        /// <summary>
        /// 題目答完後，發動的效果
        /// </summary>
        public List<GameActionBase> LimitedQuestionAction;
        
        /// <summary>
        /// 使用答對題數達標，就發動效果
        /// </summary>
        public bool UseCorrectAction;
        /// <summary>
        /// 需要答對的題數
        /// </summary>
        public int CorrectActionNeedAnswerCount;
        /// <summary>
        /// 答對題數達標發動的效果
        /// </summary>
        public List<GameActionBase> CorrectActions;

        /// <summary>
        /// 使用答錯題數達標，就發動效果
        /// </summary>
        public bool UseWrongAction;
        /// <summary>
        /// 需要答錯的題數
        /// </summary>
        public int WrongActionNeedAnswerCount;
        /// <summary>
        /// 答錯題數達標發動的效果
        /// </summary>
        public List<GameActionBase> WrongActions;
        
        /// <summary>
        /// 使用倒數計時
        /// </summary>
        public  bool UseTimeCountDown;
        /// <summary>
        /// 答題時間
        /// </summary>
        public  int Time;

        /// <summary>
        /// 產生行為的對象
        /// </summary>
        public CharacterBase SelfCharacter;
        /// <summary>
        /// 目標對象
        /// </summary>
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