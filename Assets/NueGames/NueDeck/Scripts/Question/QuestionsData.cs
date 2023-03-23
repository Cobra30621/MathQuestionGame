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
    }

    [Serializable]
    public class MultipleChoiceQuestion
    {
        [SerializeField] private Sprite questionSprite;
        [SerializeField] private int answer;
        
        public Sprite QuestionSprite => questionSprite;
        public int Answer => answer;
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
