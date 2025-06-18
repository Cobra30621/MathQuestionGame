using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Question.Data;

namespace Question.UI
{
    public class RewardTableRenderer : MonoBehaviour
    {
        [Header("資料來源")]
        [SerializeField] private QuestionStoneDropTable rewardData;

        [Header("UI 元件")]
        [SerializeField] private Transform contentRoot; // 放置 RewardRow 的地方
        [SerializeField] private GameObject rewardRowPrefab; // 一列（Row）Prefab

        private void Start()
        {
            RenderRewardTable();
        }

        private void RenderRewardTable()
        {
            // 取得所有出現過的題數（整合3/5/10題模式中的題數）
            HashSet<int> allQuestionCounts = new HashSet<int>();

            foreach (int q in rewardData.GetAvailableQuestionCounts())
                allQuestionCounts.Add(q);

            foreach (int count in rewardData.GetAllQuestionCountsFromAllModes())
                allQuestionCounts.Add(count);

            List<int> sortedCounts = new List<int>(allQuestionCounts);
            sortedCounts.Sort();

            foreach (int count in sortedCounts)
            {
                GameObject row = Instantiate(rewardRowPrefab, contentRoot);
                TextMeshProUGUI[] texts = row.GetComponentsInChildren<TextMeshProUGUI>();

                texts[0].text = $"{count-1}";
                texts[1].text = GetRewardOrDash(3, count);
                texts[2].text = GetRewardOrDash(5, count);
                texts[3].text = GetRewardOrDash(10, count);
            }
        }

        private string GetRewardOrDash(int modeQuestionCount, int actualCount)
        {
            List<int> drops = rewardData.GetStoneDropAmountsForQuestionCount(modeQuestionCount);
            if (actualCount - 1 < 0 || actualCount - 1 >= drops.Count)
                return "-";
            return drops[actualCount - 1].ToString();
        }
    }
}