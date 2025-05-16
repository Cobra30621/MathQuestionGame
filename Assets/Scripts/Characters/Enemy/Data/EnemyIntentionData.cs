using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using Utilities;

namespace Characters.Enemy.Data
{
    /// <summary>
    /// 從 Google Sheet 載入並管理敵人每回合的意圖的 ScriptableObject
    /// </summary>
    public class EnemyIntentionData : SerializedScriptableObject
    {
        /// <summary>是否正在載入資料中</summary>
        public bool IsLoading { get; private set; }

        /// <summary>載入後的敵人技能資料陣列</summary>
        public EnemyIntentionInfo[] EnemyIntentions { get; private set; }

        /// <summary>以 SkillID 為 Key 的快速查詢字典</summary>
        public Dictionary<string, EnemyIntentionInfo> dict;

        #region 私有欄位

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemyIntentionA";
        // Google Sheet API 網址，用於取得敵人技能意圖資料

        #endregion

        #region 資料載入

        /// <summary>
        /// 在 Inspector 顯示按鈕，點擊後從 Google Sheet 下載並解析資料
        /// </summary>
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemyIntentionInfo>(url, infos =>
            {
                // 過濾掉 SkillID 欄位為空的列
                EnemyIntentions = infos
                    .Where(info => !info.SkillID.IsNullOrWhitespace())
                    .ToArray();
                Debug.Log($"EnemyIntentionInfo Count: {EnemyIntentions.Length}");

                // 將 SkillID 字串轉換成 List<string>
                foreach (var skillData in EnemyIntentions)
                {
                    skillData.skillIDs = Helper.ConvertStringToStringList(skillData.SkillID);
                }

                // 建立查詢字典
                BuildDict();

                IsLoading = false;
            });
        }

        #endregion

        #region 查詢方法

        /// <summary>
        /// 依 SkillID 查詢對應的 EnemyIntentionInfo，找不到時輸出錯誤
        /// </summary>
        /// <param name="id">要查詢的技能 ID</param>
        /// <param name="whoFinding">調用者名稱，用於錯誤訊息標示</param>
        public EnemyIntentionInfo GetEnemyIntentionInfo(string id, string whoFinding = "")
        {
            if (dict.TryGetValue(id, out var action))
            {
                return action;
            }

            Debug.LogError($"{whoFinding} 找不到 EnemyIntentionInfo: '{id}'");
            return null;
        }

        #endregion

        #region 字典建立

        /// <summary>
        /// 將 EnemySkills 陣列中的每筆資料，以 SkillID 為 Key 建立字典，檢查重複並報錯
        /// </summary>
        public void BuildDict()
        {
            dict = new Dictionary<string, EnemyIntentionInfo>();
            foreach (var enemyAction in EnemyIntentions)
            {
                if (dict.ContainsKey(enemyAction.SkillID))
                {
                    Debug.LogError($"Duplicate ID: {enemyAction.SkillID}");
                }
                else
                {
                    dict.Add(enemyAction.SkillID, enemyAction);
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// 單筆敵人每回合的技能資料結構
    /// </summary>
    [Serializable]
    public class EnemyIntentionInfo
    {
        /// <summary>複合欄位，原始 SkillID 可能包含多個以逗號分隔</summary>
        public string SkillID;
        
        /// <summary>解析後的單一或多筆 SkillID 清單</summary>
        public List<string> skillIDs;
        
        /// <summary>冷卻時間（回合數）</summary>
        public int CD;

        /// <summary>意圖名稱（用於 UI 顯示或行為判斷）</summary>
        public string Intention;

        /// <summary>技能備註／說明文字</summary>
        public string ps;
    }
}
