using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sheets
{
    /// <summary>
    /// 遊戲執行時使用正式或測試數據。
    /// 預設為正式；只有測試場景的開發者測試器會切到測試，
    /// 回到主選單時一律切回正式。
    /// </summary>
    public static class SheetDataMode
    {
        private const int MainMenuBuildIndex = 0;

        /// <summary>目前使用的數據來源</summary>
        public static SheetSource Current { get; private set; } = SheetSource.Official;

        /// <summary>提供測試數據的讀取器（切到測試時設定）</summary>
        public static SheetDataGetter Getter { get; private set; }

        /// <summary>是否正在使用測試數據</summary>
        public static bool IsTest => Current == SheetSource.Test && Getter != null;

        /// <summary>
        /// 切換數據來源。切到測試但測試數據還沒下載時，維持正式並報錯。
        /// </summary>
        public static void Use(SheetSource source, SheetDataGetter getter)
        {
            if (source == SheetSource.Test)
            {
                if (getter == null || !getter.HasTestData(out var missing))
                {
                    // Debug.LogError("測試數據不完整，改用正式數據。請先下載測試資料" +
                    //                (getter == null ? "" : $"（缺少：{missing}）"));
                    Debug.LogError("測試數據不完整，改用正式數據。請先下載測試資料" +
                                   (getter == null ? "" : $"（缺少：）"));
                    source = SheetSource.Official;
                }
                else
                {
                    Debug.LogWarning("使用測試數據");
                }
            }

            Current = source;
            Getter = getter;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnPlay()
        {
            Current = SheetSource.Official;
            Getter = null;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 從主選單開始的流程一律使用正式數據
            if (scene.buildIndex == MainMenuBuildIndex && Current != SheetSource.Official)
            {
                Debug.Log("回到主選單，切回正式數據");
                Current = SheetSource.Official;
            }
        }
    }
}
