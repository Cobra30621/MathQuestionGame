using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Question
{
    [CreateAssetMenu(fileName = "QuestionData",menuName = "Question/Question Data",order = 0)]
    public class QuestionsData  : ScriptableObject
    {
        [SerializeField] private QuestionType questionType;
        [SerializeField] private MathType mathType;
        [SerializeField] private string chapterName;
        
        public QuestionType QuestionType => questionType;
        public MathType MathType => mathType;
        public string ChapterName => chapterName;
        public List<MultipleChoiceQuestion> MultipleChoiceQuestions;

        public void SetChapterName(string name)
        {
            chapterName = name;
        }
    }

    [Serializable]
    public class MultipleChoiceQuestion
    {
        [SerializeField] private Sprite questionSprite;
        [SerializeField] private Sprite optionSprite;
        [SerializeField] private int answer;
        [SerializeField] private int optionCount;
        
        public Sprite QuestionSprite => questionSprite;
        public Sprite OptionSprite => optionSprite;
        public int Answer => answer;
        public int OptionCount => optionCount;

        public MultipleChoiceQuestion(int answer, int optionCount, Sprite questionSprite, Sprite optionSprite)
        {
            this.questionSprite = questionSprite;
            this.optionSprite = optionSprite;
            this.answer = answer;
            this.optionCount = optionCount;
        }


        public MultipleChoiceQuestion(Sprite questionSprite, int answer)
        {
            this.questionSprite = questionSprite;
            this.answer = answer;
        }
    }

    public enum MathType
    {
        Numebr, Quantity, Relation, SpaceAndShape, DataAndUncertainty
    }
    
    
    public enum QuestionType
    {
        MultipleChoice,
    }
}
