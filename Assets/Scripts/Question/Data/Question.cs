using System;
using UnityEngine;

namespace Question
{
    
    /// <summary>
    /// Represents a question with associated information.
    /// </summary>
    [Serializable]
    public class Question
    {
        /// <summary>
        /// The sprite representing the question.
        /// </summary>
        public Sprite QuestionSprite;

        /// <summary>
        /// The correct answer to the question.
        /// </summary>
        public int Answer;

        /// <summary>
        /// The grade of the question.
        /// </summary>
        public Grade Grade;

        /// <summary>
        /// The publisher of the question.
        /// </summary>
        public Publisher Publisher;

        /// <summary>
        /// The mathematical type or subject of the question.
        /// </summary>
        public MathType MathType;
    }
}