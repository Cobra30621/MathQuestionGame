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
    }
}
