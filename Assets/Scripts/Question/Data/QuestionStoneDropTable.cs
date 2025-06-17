using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Question.Data
{
    /// <summary>
    /// 儲存題目數量與對應掉落寶石金額的對照表。
    /// 例如：答對 3 題可能會掉落 [5, 10, 15] 金額的寶石。
    /// </summary>
    public class QuestionStoneDropTable : SerializedScriptableObject
    {
        [LabelText("題數對應的寶石掉落金額清單 (Key: 題數, Value: 金額清單)")]
        [SerializeField]
        private Dictionary<int, List<int>> stoneDropAmountsByQuestionCount;

        /// <summary>
        /// 取得目前有定義掉落資料的所有題數（即所有的 key）。
        /// </summary>
        /// <returns>所有已定義的題數清單。</returns>
        public List<int> GetAvailableQuestionCounts()
        {
            return stoneDropAmountsByQuestionCount.Keys.ToList();
        }

        /// <summary>
        /// 根據指定的題數，取得對應的寶石掉落金額清單。
        /// </summary>
        /// <param name="questionCount">作答題數</param>
        /// <returns>對應題數的寶石掉落金額清單，若題數無定義則回傳空清單並印出錯誤。</returns>
        public List<int> GetStoneDropAmountsForQuestionCount(int questionCount)
        {
            if (stoneDropAmountsByQuestionCount.ContainsKey(questionCount))
            {
                return stoneDropAmountsByQuestionCount[questionCount];
            }
            else
            {
                Debug.LogError($"找不到題數 {questionCount} 對應的寶石掉落資料。");
                return new List<int>();
            }
        }
    }
}