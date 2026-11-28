using System.Collections.Generic;
using Card;
using Card.Data;
using Characters.Enemy.Data;
using Effect;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sheets
{
    /// <summary>
    /// 數據來源：正式表單讀 CardA、SkillA 等分頁；測試表單讀 CardTest、SkillTest 等分頁
    /// </summary>
    public enum SheetSource
    {
        [LabelText("正式")] Official,
        [LabelText("測試")] Test,
    }

    /// <summary>
    /// Retrieves data from various sheets for use in the game.
    /// 同時保存正式與測試兩份表單資料；遊戲執行時依 SheetDataMode 決定用哪一份。
    /// </summary>
    public class SheetDataGetter : ScriptableObject
    {
        #region Sheet Link
        [BoxGroup("表單")] [LabelText("表單連結")]
        [InfoBox("正式資料讀取 CardA、SkillA、EnemyA、EnemyIntentionA 分頁；測試資料讀取 CardTest、SkillTest、EnemyTest、EnemyIntentionTest 分頁")]
        [Tooltip("貼上 Google Sheet 網址或表單 ID")]
        public string sheetLink = "https://docs.google.com/spreadsheets/d/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/edit";

        /// <summary>
        /// 從 Google Sheet 網址或 ID 取出表單 ID；無法取得時回傳 null
        /// </summary>
        public static string ExtractSheetId(string link)
        {
            if (string.IsNullOrWhiteSpace(link)) return null;
            link = link.Trim();

            const string marker = "/d/";
            var start = link.IndexOf(marker, System.StringComparison.Ordinal);
            if (start < 0) return link.Contains("/") ? null : link;

            start += marker.Length;
            var end = link.IndexOfAny(new[] { '/', '?', '#' }, start);
            var id = end < 0 ? link.Substring(start) : link.Substring(start, end - start);
            return string.IsNullOrEmpty(id) ? null : id;
        }

        /// <summary>
        /// 組出分頁名稱，例如 "Card" → 正式 "CardA"、測試 "CardTest"
        /// </summary>
        public static string GetSheetName(string baseName, SheetSource source)
            => baseName + (source == SheetSource.Test ? "Test" : "A");

        /// <summary>
        /// 組出指定分頁的 opensheet 網址；連結無效時回傳 null
        /// </summary>
        public string GetSheetUrl(string baseName, SheetSource source)
        {
            var id = ExtractSheetId(sheetLink);
            return id == null ? null : $"https://opensheet.elk.sh/{id}/{GetSheetName(baseName, source)}";
        }
        #endregion

        #region 正式資料
        [FormerlySerializedAs("enemyData")]
        [BoxGroup("正式資料")] [Required] [InlineEditor] [LabelText("敵人")]
        public EnemyDataOverview officialEnemyData;

        [FormerlySerializedAs("enemySkillData")] [FormerlySerializedAs("enemyIntentionData")]
        [BoxGroup("正式資料")] [Required] [InlineEditor] [LabelText("敵人技能")]
        public EnemyIntentionData officialEnemyIntentionData;

        [FormerlySerializedAs("skillData")]
        [BoxGroup("正式資料")] [Required] [InlineEditor] [LabelText("技能")]
        public SkillData officialSkillData;

        [FormerlySerializedAs("cardLevelData")]
        [BoxGroup("正式資料")] [Required] [InlineEditor] [LabelText("卡片等級")]
        public CardLevelData officialCardLevelData;
        #endregion

        #region 測試資料
        [BoxGroup("測試資料")] [InlineEditor] [LabelText("敵人")]
        [InfoBox("尚未建立測試資料：在開發者測試器按「下載測試資料」會自動建立", InfoMessageType.Info, nameof(MissingTestAssets))]
        public EnemyTestData testEnemyData;

        [BoxGroup("測試資料")] [InlineEditor] [LabelText("敵人技能")]
        public EnemyIntentionData testEnemyIntentionData;

        [BoxGroup("測試資料")] [InlineEditor] [LabelText("技能")]
        public SkillData testSkillData;

        [BoxGroup("測試資料")] [InlineEditor] [LabelText("卡片等級")]
        public CardLevelData testCardLevelData;

        private bool MissingTestAssets => testEnemyData == null || testEnemyIntentionData == null ||
                                          testSkillData == null || testCardLevelData == null;

        /// <summary>
        /// 測試資料是否齊全（資料檔都在、而且下載過）
        /// </summary>
        public bool HasTestData(out string missing)
        {
            var list = new List<string>();
            if (testSkillData == null || testSkillData.GetAllSkillInfos().Count == 0) list.Add("SkillTest");
            // EnemyIntentions 不會存檔，重開 Unity 後只剩 dict，所以用 dict 判斷
            if (testEnemyIntentionData == null || testEnemyIntentionData.dict == null ||
                testEnemyIntentionData.dict.Count == 0) list.Add("EnemyIntentionTest");
            if (testEnemyData == null || testEnemyData.GetAllData().Count == 0) list.Add("EnemyTest");
            if (testCardLevelData == null || testCardLevelData.GetAllCardInfo().Count == 0) list.Add("CardTest");
            missing = string.Join("、", list);
            return list.Count == 0;
        }
        #endregion

        #region 依目前模式取得資料
        private static bool UseTest => SheetDataMode.IsTest;

        /// <summary>目前使用中的技能資料</summary>
        public SkillData skillData => UseTest ? testSkillData : officialSkillData;

        /// <summary>目前使用中的敵人技能資料</summary>
        public EnemyIntentionData enemyIntentionData => UseTest ? testEnemyIntentionData : officialEnemyIntentionData;

        /// <summary>目前使用中的卡片等級資料</summary>
        public CardLevelData cardLevelData => UseTest ? testCardLevelData : officialCardLevelData;

        /// <summary>依 ID 取得目前使用中的敵人資料</summary>
        public EnemyData FindEnemy(string id)
        {
            var data = UseTest ? testEnemyData.FindUniqueId(id) : officialEnemyData.FindUniqueId(id);
            if (data == null)
                Debug.LogError($"{(UseTest ? "測試" : "正式")}敵人資料找不到 ID '{id}'");
            return data;
        }
        #endregion

        #region 其他資料
        [Required] [InlineEditor] [LabelText("敵人 Prefab")]
        public EnemyPrefabData enemyPrefabData;

        [Required] [InlineEditor] [LabelText("意圖")]
        public IntentionData intentionData;

        [Required] [InlineEditor] [LabelText("正式版遊戲的卡片")]
        public DeckData officialDeck;

        [Required] [InlineEditor] [LabelText("開發者卡片")]
        public DeckData developDeck;
        #endregion

        /// <summary>
        /// Retrieves intention data based on the given ID.
        /// </summary>
        public Intention GetIntention(string id)
        {
            return intentionData.GetIntention(id);
        }

        /// <summary>
        /// Retrieves enemy skill data based on the given ID.
        /// </summary>
        public EnemyIntentionInfo GetEnemySkillInfo(string id)
        {
            return enemyIntentionData.GetEnemyIntentionInfo(id);
        }

        /// <summary>
        /// Retrieves skill data based on the given ID.
        /// </summary>
        public SkillInfo GetSkillInfo(string id)
        {
            return skillData.GetSkillInfo(id);
        }
    }
}
