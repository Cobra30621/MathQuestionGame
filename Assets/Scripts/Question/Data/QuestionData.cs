using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question
{
    [CreateAssetMenu(fileName = "QuestionData",menuName = "Question/QuestionData")]
    public class QuestionData : SerializedScriptableObject
    {
        public List<QuestionClip> questionClip;
    }
}