namespace Question.QuestionLoader
{
    [System.Serializable]
    public class ProblemData
    {
        public Problem[] problem;
    }

    [System.Serializable]
    public class Problem
    {
        public string problemLink;
        public string ansLink;
        public string answer;
    }
}