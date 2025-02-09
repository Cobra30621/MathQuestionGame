using Managers;
using Save.Data;
using UnityEngine;

namespace VersionControl
{
    /// <summary>
    /// 表示存檔版本的兼容性狀態
    /// </summary>
    public enum SaveCompatibilityStatus
    {
        FullyCompatible,       // 0: 版本完全相同，不需要處理
        MinorVersionDiff,      // 1: 版本號不同，但不影響存檔狀況，允許直接讀取
        SaveTooOld,            // 11: 存檔太舊，應該還可以沿用部分數據
        SaveTooNew,            // 21: 存檔太新，應該還可以沿用部分數據
        Incompatible           // -1: 主要版本不同，可能不相容
    }

    /// <summary>
    /// 用於處理舊存檔與當前版本的兼容性問題
    /// </summary>
    public static class SaveVersionHandler
    {
        /// <summary>
        /// 如果需要，轉換 `PermanentGameData`
        /// </summary>
        public static PermanentGameData ConvertPermanentDataIfNeeded(
            GameVersion saveGameVersion, PermanentGameData permanentData)
        {
            var systemGameVersion = GameManager.Instance.SystemVersion();
            var status = CheckSaveCompatibility(saveGameVersion, systemGameVersion);

            switch (status)
            {
                case SaveCompatibilityStatus.FullyCompatible:
                case SaveCompatibilityStatus.MinorVersionDiff:
                    return permanentData;

                case SaveCompatibilityStatus.SaveTooOld:
                    return MigrateOldPermanentData(permanentData);

                case SaveCompatibilityStatus.SaveTooNew:
                    return HandleNewerPermanentData(permanentData);

                default:
                    Debug.LogWarning("存檔版本與系統版本不相容，可能需要手動處理");
                    return null;
            }
        }

        /// <summary>
        /// 如果需要，轉換 `GameData`
        /// </summary>
        public static GameData ConvertGameDataIfNeeded(GameVersion saveGameVersion, GameData gameData)
        {
            var systemGameVersion = GameManager.Instance.SystemVersion();
            var status = CheckSaveCompatibility(saveGameVersion, systemGameVersion);

            switch (status)
            {
                case SaveCompatibilityStatus.FullyCompatible:
                case SaveCompatibilityStatus.MinorVersionDiff:
                    return gameData;

                case SaveCompatibilityStatus.SaveTooOld:
                    return MigrateOldGameData(gameData);

                case SaveCompatibilityStatus.SaveTooNew:
                    return HandleNewerGameData(gameData);

                default:
                    Debug.LogWarning("存檔版本與系統版本不相容，可能需要手動處理");
                    return null;
            }
        }

        /// <summary>
        /// 檢查存檔版本是否兼容
        /// </summary>
        public static SaveCompatibilityStatus CheckSaveCompatibility(GameVersion saveGameVersion, GameVersion systemGameVersion)
        {
            Debug.Log($"系統版本：{systemGameVersion.GetVersionNum()}");
            Debug.Log($"存檔版本：{saveGameVersion.GetVersionNum()}");

            if (saveGameVersion.MainVersionNum != systemGameVersion.MainVersionNum)
            {
                return SaveCompatibilityStatus.Incompatible; // 主要版本不同，可能不相容
            }

            if (saveGameVersion.SubVersionNum == systemGameVersion.SubVersionNum)
            {
                return (saveGameVersion.FixVersionNum == systemGameVersion.FixVersionNum)
                    ? SaveCompatibilityStatus.FullyCompatible
                    : SaveCompatibilityStatus.MinorVersionDiff;
            }

            return (saveGameVersion.SubVersionNum < systemGameVersion.SubVersionNum)
                ? SaveCompatibilityStatus.SaveTooOld
                : SaveCompatibilityStatus.SaveTooNew;
        }

        /// <summary>
        /// 處理舊版本的 `PermanentGameData`（範例方法）
        /// </summary>
        private static PermanentGameData MigrateOldPermanentData(PermanentGameData oldData)
        {
            Debug.Log("遷移舊版本的 PermanentGameData...");
            return oldData;
        }

        /// <summary>
        /// 處理較新版本的 `PermanentGameData`（範例方法）
        /// </summary>
        private static PermanentGameData HandleNewerPermanentData(PermanentGameData newData)
        {
            Debug.Log("處理較新版本的 PermanentGameData...");
            return newData;
        }

        /// <summary>
        /// 處理舊版本的 `GameData`（範例方法）
        /// </summary>
        private static GameData MigrateOldGameData(GameData oldData)
        {
            Debug.Log("遷移舊版本的 GameData...");
            return oldData;
        }

        /// <summary>
        /// 處理較新版本的 `GameData`（範例方法）
        /// </summary>
        private static GameData HandleNewerGameData(GameData newData)
        {
            Debug.Log("處理較新版本的 GameData...");
            return newData;
        }
    }
}
