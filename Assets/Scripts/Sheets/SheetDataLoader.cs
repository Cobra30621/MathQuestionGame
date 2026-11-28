﻿using System.Collections;
using System.Linq;
using Card.Data;
using Characters.Enemy.Data;
using Effect;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



namespace Sheets
{
    /// <summary>
    /// Loads data from Google Sheets for enemies, enemy skills, and skills.
    /// 可下載正式（CardA 等分頁）或測試（CardTest 等分頁）資料，分別存在不同的資料檔。
    /// </summary>
    public class SheetDataLoader : MonoBehaviour
    {
        // 遊戲匯入用的四張分頁名稱（不含結尾；正式加 A，測試加 Test）
        private const string SkillSheet = "Skill";
        private const string EnemyIntentionSheet = "EnemyIntention";
        private const string EnemySheet = "Enemy";
        private const string CardSheet = "Card";

        // 測試資料檔自動建立的位置
        private const string TestDataFolder = "Assets/Data/Sheets Test";

        [Required] [InlineEditor]
        [LabelText("資料讀取器")]
        public SheetDataGetter getter;

        [LabelText("資料驗證")]
        [Required] public SheetDataValidator sheetDataValidator;

        private const float TimeoutSeconds = 30f;
        private bool _timedOut;

        /// <summary>是否正在下載</summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// 下載目前使用中的數據（遊戲中作弊面板用）：主選單流程為正式，測試場景依開發者測試器的設定
        /// </summary>
        public void Load()
        {
            Load(SheetDataMode.Current);
        }

        [Button("下載正式資料", ButtonSizes.Medium)]
        [HorizontalGroup("Load")]
        public void LoadOfficial() => Load(SheetSource.Official);

        [Button("下載測試資料", ButtonSizes.Medium)]
        [HorizontalGroup("Load")]
        public void LoadTest() => Load(SheetSource.Test);

        /// <summary>
        /// 從 Google Sheets 下載指定來源的資料
        /// </summary>
        public void Load(SheetSource source)
        {
            if (IsLoading)
            {
                Debug.LogWarning("表單還在下載中，請稍候");
                return;
            }

#if UNITY_EDITOR
            // 編輯模式（沒按 Play）時 MonoBehaviour 的 coroutine 不會推進，改用編輯器更新來執行
            if (!Application.isPlaying)
            {
                RunInEditor(LoadCoroutine(source));
                return;
            }
#endif
            StartCoroutine(LoadCoroutine(source));
        }

#if UNITY_EDITOR
        private static void RunInEditor(IEnumerator routine)
        {
            var stack = new System.Collections.Generic.Stack<IEnumerator>();
            stack.Push(routine);

            void Tick()
            {
                if (stack.Count > 0)
                {
                    var top = stack.Peek();
                    if (top.MoveNext())
                    {
                        if (top.Current is IEnumerator nested) stack.Push(nested);
                    }
                    else
                    {
                        stack.Pop();
                    }
                }

                if (stack.Count == 0) EditorApplication.update -= Tick;
            }

            EditorApplication.update += Tick;
        }
#endif

        /// <summary>
        /// Coroutine for loading data from Google Sheets.
        /// </summary>
        private IEnumerator LoadCoroutine(SheetSource source)
        {
            var isTest = source == SheetSource.Test;
            var sourceName = isTest ? "測試" : "正式";

            if (SheetDataGetter.ExtractSheetId(getter.sheetLink) == null)
            {
                Debug.LogError($"表單連結無效，請在 SheetDataGetter 貼上 Google Sheet 網址：'{getter.sheetLink}'");
                yield break;
            }

            if (isTest && !EnsureTestAssets()) yield break;

            var skillData = isTest ? getter.testSkillData : getter.officialSkillData;
            var enemyIntentionData = isTest ? getter.testEnemyIntentionData : getter.officialEnemyIntentionData;
            var cardLevelData = isTest ? getter.testCardLevelData : getter.officialCardLevelData;

            var sheetNames = string.Join("、", new[] { SkillSheet, EnemyIntentionSheet, EnemySheet, CardSheet }
                .Select(name => SheetDataGetter.GetSheetName(name, source)));
            Debug.Log($"開始下載{sourceName}資料：{sheetNames}");

            IsLoading = true;

            // Load skill data from Google Sheets
            skillData.ParseDataFromGoogleSheet(getter.GetSheetUrl(SkillSheet, source));
            yield return WaitForSheet(() => skillData.IsLoading, SkillSheet, source);
            if (_timedOut) yield break;

            // Load enemy skill data from Google Sheets
            enemyIntentionData.ParseDataFromGoogleSheet(getter.GetSheetUrl(EnemyIntentionSheet, source));
            yield return WaitForSheet(() => enemyIntentionData.IsLoading, EnemyIntentionSheet, source);
            if (_timedOut) yield break;

            // Load enemy data from Google Sheets
            if (isTest)
            {
                getter.testEnemyData.ParseDataFromGoogleSheet(getter.GetSheetUrl(EnemySheet, source));
                yield return WaitForSheet(() => getter.testEnemyData.IsLoading, EnemySheet, source);
            }
            else
            {
                getter.officialEnemyData.ParseDataFromGoogleSheet(getter.GetSheetUrl(EnemySheet, source));
                yield return WaitForSheet(() => getter.officialEnemyData.IsLoading, EnemySheet, source);
            }
            if (_timedOut) yield break;

            cardLevelData.ParseDataFromGoogleSheet(getter.GetSheetUrl(CardSheet, source));
            yield return WaitForSheet(() => cardLevelData.IsLoading, CardSheet, source);
            if (_timedOut) yield break;

            // 正式資料才寫進卡片（CardData）；測試資料在遊戲執行時直接從測試卡片等級資料讀取
            if (!isTest)
            {
                // 建立遊戲正式版卡牌
                BuildCardData(getter.officialDeck);
                // 建立開發者卡牌
                BuildCardData(getter.developDeck);
            }

            Debug.Log($"{sourceName}資料下載完成");

            SaveAsset(isTest);
            IsLoading = false;

            // Validate data
            sheetDataValidator.ValidateSheet(source);
        }

        /// <summary>
        /// 等待一張分頁下載完成。分頁不存在或回應格式錯誤時不會有回呼，超過時間就報錯並停止下載。
        /// </summary>
        private IEnumerator WaitForSheet(System.Func<bool> isLoading, string baseName, SheetSource source)
        {
            _timedOut = false;
            var startTime = Time.realtimeSinceStartup;
            while (isLoading())
            {
                if (Time.realtimeSinceStartup - startTime > TimeoutSeconds)
                {
                    _timedOut = true;
                    IsLoading = false;
                    Debug.LogError($"下載分頁 {SheetDataGetter.GetSheetName(baseName, source)} 失敗或逾時，" +
                                   "請確認分頁名稱正確、表單已開啟「知道連結的任何人都能檢視」，並查看上方的錯誤訊息");
                    yield break;
                }
                yield return null;
            }
        }

        private void BuildCardData(DeckData deck)
        {
            foreach (var cardData in deck.CardList)
            {
                var cardLevelInfos = getter.officialCardLevelData.GetLevelInfo(cardData.CardId);
                cardData.SetCardLevels(cardLevelInfos);
                if (cardLevelInfos != null && cardLevelInfos.Count > 0)
                {
                    cardData.AllyClassType = cardLevelInfos[0].Class;
                }
                else
                {
                    Debug.LogError("Could not find card level info for " + cardData.CardId);
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(cardData);
#endif
            }
        }

        /// <summary>
        /// 確認測試資料檔都存在；在編輯器中缺少時自動建立
        /// </summary>
        private bool EnsureTestAssets()
        {
#if UNITY_EDITOR
            getter.testSkillData = GetOrCreateAsset(getter.testSkillData, "SkillData Test");
            getter.testEnemyIntentionData = GetOrCreateAsset(getter.testEnemyIntentionData, "Enemy Intention Data Test");
            getter.testEnemyData = GetOrCreateAsset(getter.testEnemyData, "Enemy Data Test");
            getter.testCardLevelData = GetOrCreateAsset(getter.testCardLevelData, "CardLevelData Test");
            EditorUtility.SetDirty(getter);
            return true;
#else
            if (getter.testSkillData == null || getter.testEnemyIntentionData == null ||
                getter.testEnemyData == null || getter.testCardLevelData == null)
            {
                Debug.LogError("找不到測試資料檔，請先在 Unity 編輯器中下載一次測試資料");
                return false;
            }
            return true;
#endif
        }

#if UNITY_EDITOR
        private static T GetOrCreateAsset<T>(T current, string assetName) where T : ScriptableObject
        {
            if (current != null) return current;

            if (!AssetDatabase.IsValidFolder(TestDataFolder))
            {
                AssetDatabase.CreateFolder("Assets/Data", "Sheets Test");
            }

            var path = $"{TestDataFolder}/{assetName}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null) return asset;

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            Debug.Log($"建立測試資料檔：{path}");
            return asset;
        }
#endif

        private void SaveAsset(bool isTest)
        {
#if UNITY_EDITOR
            if (isTest)
            {
                EditorUtility.SetDirty(getter.testSkillData);
                EditorUtility.SetDirty(getter.testEnemyIntentionData);
                EditorUtility.SetDirty(getter.testEnemyData);
                EditorUtility.SetDirty(getter.testCardLevelData);
            }
            else
            {
                EditorUtility.SetDirty(getter.officialSkillData);
                EditorUtility.SetDirty(getter.officialEnemyIntentionData);
                EditorUtility.SetDirty(getter.officialEnemyData);
                EditorUtility.SetDirty(getter.officialCardLevelData);
            }
            AssetDatabase.SaveAssets();
#endif
        }
    }
}
