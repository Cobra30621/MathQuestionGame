using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VersionControl
{
    [CreateAssetMenu(fileName = "GameVersionData", menuName = "VersionControl/Game Version Data", order = 0)]
    public class GameVersionData : ScriptableObject
    {
        [LabelText("所有版本資料")]
        [SerializeField]
        [TableList()]
        private List<GameVersion> versionList = new List<GameVersion>();

        /// <summary>
        /// 取得所有版本資料
        /// </summary>
        public List<GameVersion> VersionList => versionList;

        /// <summary>
        /// 新增一個新版本（可由編輯器按鈕觸發）
        /// </summary>
        [Button("新增版本")]
        public void AddVersion(GameVersion newVersion)
        {
            versionList.Add(newVersion);
        }
        

        /// <summary>
        /// 取得最新的版本（依照版本號排序）
        /// </summary>
        [Button("取得最新版本")]
        public GameVersion GetLatestVersion()
        {
            return versionList
                .OrderByDescending(v => v.MainVersionNum)
                .ThenByDescending(v => v.SubVersionNum)
                .ThenByDescending(v => v.FixVersionNum)
                .FirstOrDefault();
        }
    }
}