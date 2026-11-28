using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities;

namespace Characters.Enemy.Data
{
    /// <summary>
    /// 測試表單（EnemyTest 分頁）的敵人資料。
    /// EnemyDataOverview 是全專案唯一的 Singleton，不能複製一份當測試用，所以另外存在這裡。
    /// </summary>
    public class EnemyTestData : ScriptableObject
    {
        /// <summary>是否正在載入資料</summary>
        public bool IsLoading { get; private set; }

        [SerializeField]
        [TableList]
        [LabelText("測試敵人資料")]
        private List<EnemyData> enemies = new List<EnemyData>();

        public List<EnemyData> GetAllData() => enemies;

        public EnemyData FindUniqueId(string id) => enemies.Find(enemy => enemy.DataId == id);

        /// <summary>從指定網址載入</summary>
        public void ParseDataFromGoogleSheet(string sheetUrl)
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemyData>(sheetUrl, infos =>
            {
                enemies = EnemyDataOverview.PrepareEnemyData(infos);
                IsLoading = false;
            });
        }
    }
}
