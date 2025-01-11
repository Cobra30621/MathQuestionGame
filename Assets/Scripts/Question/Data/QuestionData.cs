using System.Collections.Generic;
using System.Linq;
using Question.Enum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.Data
{
    [CreateAssetMenu(fileName = "QuestionData",menuName = "Question/QuestionData")]
    public class QuestionData : SerializedScriptableObject
    {
        public List<QuestionClip> questionClip;

        public List<Question> GetQuestion(Publisher publisher , Grade grade)
        {
            var questionClips = questionClip
                .Where(clip => clip.publisher == publisher && clip.grade == grade)
                .ToList();
            var questions = new List<Question>();
            foreach (var clip in questionClips)
            {
                questions.AddRange(clip.questions);
            }

            return questions;
        }
    }
}