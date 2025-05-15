using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Effect.Parameters;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Effect
{
    /// <summary>
    /// SkillData：從 Google Sheet 載入技能資料的 ScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "SkillData", menuName = "SkillData", order = 0)]
    public class SkillData : SerializedScriptableObject
    {
        /// <summary>是否正在載入中</summary>
        public bool IsLoading { get; private set; }
        
        #region 私有欄位
        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/SkillData_dev";  
        // Google Sheet API 的網址

        [SerializeField]
        [TableList]
        private SkillInfo[] skillInfos;  
        // 解析後的技能資訊陣列

        /// <summary>技能 ID 對應 SkillInfo 的查詢辭典</summary>
        public Dictionary<string, SkillInfo> dict;
        #endregion

        #region 查詢方法
        /// <summary>
        /// 取得所有載入的 SkillInfo 清單
        /// </summary>
        public List<SkillInfo> GetAllSkillInfos()
        {
            return skillInfos.ToList();
        }
        
        /// <summary>
        /// 依照技能 ID 查詢對應的 SkillInfo
        /// </summary>
        /// <param name="id">欲查詢的技能 ID</param>
        /// <param name="whoFinding">調用者名稱，用於錯誤訊息</param>
        public SkillInfo GetSkillInfo(string id, string whoFinding = "")
        {
            if (dict.TryGetValue(id, out var value))
            {
                return value;
            }
            else
            {
                Debug.LogError($"{whoFinding} 找不到 SkillInfo: '{id}'");
                return null;
            }
        }
        #endregion

        #region 資料解析
        /// <summary>
        /// Inspector 按鈕：從 Google Sheet 載入並解析技能資料
        /// </summary>
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            Debug.Log($"開始從 URL 載入：{url}");
            IsLoading = true;
            GoogleSheetService.LoadDataArray<SkillInfo>(url, infos =>
            {
                // 過濾掉 SkillID 欄位為空的列
                skillInfos = infos
                    .Where(skill => !skill.SkillID.IsNullOrWhitespace())
                    .ToArray();
                Debug.Log($"載入後的 SkillInfo 數量: {skillInfos.Length}");
                
                // 將 EffectParameter 字串轉為整數清單
                foreach (var skillInfo in skillInfos)
                {
                    var effectParameterList = Helper.ConvertStringToIntList(skillInfo.EffectParameter);
                    skillInfo.EffectParameterList = effectParameterList;
                }
                
                // 建立 ID -> Info 的辭典
                BuildDict();
                IsLoading = false;
            });
        }

        /// <summary>
        /// 建立 dict 字典，將 SkillID 映射到對應的 SkillInfo
        /// </summary>
        private void BuildDict()
        {
            dict = new Dictionary<string, SkillInfo>();
            foreach (var info in skillInfos)
            {
                if (dict.ContainsKey(info.SkillID))
                    Debug.LogError($"重複的 SkillID : {info.SkillID}");
                else
                    dict.Add(info.SkillID, info);
            }
        }
        #endregion
    }
    
    
    /// <summary>
    /// 單筆技能資料結構
    /// </summary>
    [Serializable]
    public class SkillInfo 
    {
        /// <summary>技能唯一識別碼</summary>
        public string SkillID;

        /// <summary>效果對應列舉</summary>
        public EffectName EffectID;

        /// <summary>從表單讀入的效果參數字串（逗號分隔）</summary>
        public string EffectParameter;

        /// <summary>解析後的效果參數整數清單</summary>
        public List<int> EffectParameterList; 

        /// <summary>行動目標選擇類型</summary>
        public ActionTargetType Target;

        /// <summary>技能備註或說明文字</summary>
        public string ps;

        /// <summary>
        /// 用於 Debug 或 Log 輸出的字串格式
        /// </summary>
        public override string ToString()
        {
            return $"{ps} | {EffectID} : {EffectParameter}, Target:{Target} SkillId : {SkillID}";
        }
    }
}
