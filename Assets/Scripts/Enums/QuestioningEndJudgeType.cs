namespace NueGames.Enums
{
    // 答題的邏輯判斷，是用答對數量判斷?
    public enum QuestioningEndJudgeType
    {
        LimitedQuestionCount, // 固定題數，依據答對數量發動效果
        CorrectOrWrongCount // 固定不固定，如果答對、答錯題數達標，就發動效果
    }
}