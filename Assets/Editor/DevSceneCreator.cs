using System;
using System.IO;
using System.Linq;
using Card.Data;
using Characters.Ally;
using Tool;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EditorTools
{
    /// <summary>
    /// 一鍵建立個人測試場景：複製 Test Template 1、建立個人測試角色與牌組，
    /// 把角色註冊到 ally data overview，並設定好場景中的開發者測試器。
    /// 選單：Tools/開發者工具/建立我的測試場景
    /// </summary>
    public class DevSceneCreator : EditorWindow
    {
        private const string TemplateScenePath = "Assets/Scenes/Develop Scene/Test Template 1.unity";
        private const string SceneFolder = "Assets/Scenes/Develop Scene";
        private const string BaseAllyPath = "Assets/Data/Allies/Test/Test Ally.asset";
        private const string AllyFolder = "Assets/Data/Allies/Test";
        private const string AllyOverviewPath = "Assets/Data/Allies/ally data overview.asset";
        private const string DeckFolder = "Assets/Data/Decks/Develop";

        private string userName = "";
        private AllyData baseAlly;
        private bool createDeck = true;

        [MenuItem("Tools/開發者工具/建立我的測試場景")]
        public static void Open()
        {
            var window = GetWindow<DevSceneCreator>(true, "建立我的測試場景");
            window.minSize = new Vector2(420, 260);
            window.baseAlly = AssetDatabase.LoadAssetAtPath<AllyData>(BaseAllyPath);
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "輸入你的名字（建議英文），按下建立後會自動：\n" +
                "1. 複製 Test Template 1 → <名字> Test 場景\n" +
                "2. 建立測試角色 <名字> Test（顯示名稱「<名字>測試」）並註冊到總覽\n" +
                "3. 建立個人牌組 <名字> Deck，作為角色的初始卡牌\n" +
                "4. 把場景的開發者測試器設成你的角色，存檔並開啟場景\n" +
                "已存在的角色或牌組會直接沿用，不會覆蓋。",
                MessageType.Info);

            userName = EditorGUILayout.TextField("你的名字", userName);
            baseAlly = (AllyData)EditorGUILayout.ObjectField("角色範本", baseAlly, typeof(AllyData), false);
            createDeck = EditorGUILayout.Toggle(new GUIContent("建立個人牌組",
                "複製範本角色的初始牌組，避免改到大家共用的牌組"), createDeck);

            var error = Validate(userName.Trim());
            if (error != null) EditorGUILayout.HelpBox(error, MessageType.Warning);

            using (new EditorGUI.DisabledScope(error != null))
            {
                if (GUILayout.Button("建立", GUILayout.Height(32)))
                {
                    Create(userName.Trim());
                }
            }
        }

        private string Validate(string name)
        {
            if (string.IsNullOrEmpty(name)) return "請輸入名字";
            if (name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0) return "名字含有不能用在檔名的字元";
            if (baseAlly == null) return "請指定角色範本";
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(TemplateScenePath) == null)
                return $"找不到範本場景：{TemplateScenePath}";
            if (File.Exists(ScenePath(name))) return $"場景已存在：{ScenePath(name)}";
            return null;
        }

        private static string ScenePath(string name) => $"{SceneFolder}/{name} Test.unity";
        private static string AllyPath(string name) => $"{AllyFolder}/{name} Test.asset";
        private static string DeckPath(string name) => $"{DeckFolder}/{name} Deck.asset";

        private void Create(string name)
        {
            // 先讓使用者處理目前開啟場景的未存檔變更
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            try
            {
                var overview = LoadOverview();
                if (overview == null)
                {
                    EditorUtility.DisplayDialog("建立失敗", "找不到 AllyDataOverview（ally data overview.asset）", "OK");
                    return;
                }

                var ally = GetOrCreateAlly(name, overview);
                var scenePath = CreateScene(name, ally);

                EditorUtility.DisplayDialog("建立完成",
                    $"場景：{scenePath}\n" +
                    $"角色：{AssetDatabase.GetAssetPath(ally)}（{ally.DisplayName}）\n" +
                    (ally.InitialDeck != null ? $"牌組：{AssetDatabase.GetAssetPath(ally.InitialDeck)}\n" : "") +
                    "\n修改角色的初始卡牌或遺物後，按 Play 就會直接進入戰鬥。\n" +
                    "記得 commit 新增的檔案（含 .meta）與 ally data overview.asset。",
                    "OK");

                EditorGUIUtility.PingObject(ally);
                Selection.activeObject = ally;
                Close();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorUtility.DisplayDialog("建立失敗", e.Message, "OK");
            }
        }

        private static AllyDataOverview LoadOverview()
        {
            var overview = AssetDatabase.LoadAssetAtPath<AllyDataOverview>(AllyOverviewPath);
            if (overview != null) return overview;

            var guid = AssetDatabase.FindAssets("t:AllyDataOverview").FirstOrDefault();
            return guid == null ? null : AssetDatabase.LoadAssetAtPath<AllyDataOverview>(AssetDatabase.GUIDToAssetPath(guid));
        }

        private AllyData GetOrCreateAlly(string name, AllyDataOverview overview)
        {
            var allyPath = AllyPath(name);
            var ally = AssetDatabase.LoadAssetAtPath<AllyData>(allyPath);
            var isNewAlly = ally == null;
            var displayName = $"{name}測試";

            // 新角色要改顯示名稱（複製會帶著範本的名稱）；舊角色只在名稱為空時補上。
            // 下拉選單只列出有 displayName 的角色，且同名會混淆，所以先檢查。
            var needDisplayName = isNewAlly || string.IsNullOrEmpty(ally.DisplayName);
            if (needDisplayName && overview.ContainDisplayName(displayName))
                throw new Exception($"總覽中已有同名角色「{displayName}」");

            if (isNewAlly)
            {
                if (!AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(baseAlly), allyPath))
                    throw new Exception($"無法建立角色：{allyPath}");

                ally = AssetDatabase.LoadAssetAtPath<AllyData>(allyPath);
                // 複製會連 dataId 一起複製，必須換成新的，否則會和範本角色撞 ID
                ally.SetDataId(Guid.NewGuid().ToString());
            }

            if (needDisplayName) ally.SetDisplayName(displayName);

            if (createDeck)
            {
                var deckPath = DeckPath(name);
                var deck = AssetDatabase.LoadAssetAtPath<DeckData>(deckPath);
                if (deck == null && ally.InitialDeck != null)
                {
                    if (!AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(ally.InitialDeck), deckPath))
                        throw new Exception($"無法建立牌組：{deckPath}");
                    deck = AssetDatabase.LoadAssetAtPath<DeckData>(deckPath);
                }

                if (deck != null) ally.InitialDeck = deck;
            }

            EditorUtility.SetDirty(ally);

            if (!overview.ContainsId(ally.DataId))
            {
                overview.AddNewData(ally);
                EditorUtility.SetDirty(overview);
            }

            AssetDatabase.SaveAssets();
            return ally;
        }

        private static string CreateScene(string name, AllyData ally)
        {
            var scenePath = ScenePath(name);
            // 用 CopyAsset 複製，Unity 會產生新的 GUID
            if (!AssetDatabase.CopyAsset(TemplateScenePath, scenePath))
                throw new Exception($"無法複製場景：{scenePath}");

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

            var developTool = FindObjectOfType<DevelopTool>();
            if (developTool == null)
                throw new Exception("場景中找不到開發者測試器（DevelopTool）");

            // 透過 SerializedObject 設定，Prefab 覆寫會被正確記錄
            var so = new SerializedObject(developTool);
            var idProperty = so.FindProperty("allyName.id");
            if (idProperty == null)
                throw new Exception("DevelopTool 上找不到 allyName.id 欄位");
            idProperty.stringValue = ally.DataId;
            so.ApplyModifiedProperties();

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);

            Selection.activeGameObject = developTool.gameObject;
            return scenePath;
        }
    }
}
