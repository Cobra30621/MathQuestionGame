using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question
{
    /// <summary>
    /// 設置讀取圖片的參數
    /// 詳細設定可以參考這篇文章：https://hackmd.io/@Cobra3279/B1NeCrmh3
    /// </summary>
    [CreateAssetMenu(fileName = "LoadQuestionsData",menuName = "Question/Load Questions Data",order = 0)]
    public class LoadQuestionsData  : SerializedScriptableObject
    {
        public List<LoadQuestionsParameter> Parameters;
        
        public LoadQuestionsParameter GetParameter(Publisher publisher, Grade grade)
        {
            foreach (var parameter in Parameters)
            {
                if (parameter.Grade == grade && parameter.Publisher == publisher)
                {
                    return parameter;
                }
            }

            Debug.LogError($"不存在 Grade:{grade}, Publisher:{publisher}的讀題參數\n" +
                           $"請去 Assets/Data/Questions/LoadQuestionData.asset 設定");
            
            return Parameters[0];
        }
    }
}
