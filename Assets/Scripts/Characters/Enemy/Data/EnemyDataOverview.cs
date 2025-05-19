using System.Collections.Generic;
using System.Linq;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using Utilities;

namespace Characters.Enemy.Data
{
    /// <summary>
    /// 敵人資料總覽
    /// 繼承自 DataOverviewBase，管理所有 EnemyData 實例的載入與索引
    /// </summary>
    [CreateAssetMenu]
    public class EnemyDataOverview : DataOverviewBase<EnemyDataOverview, EnemyData>
    {
        /// <summary>是否正在載入資料</summary>
        public bool IsLoading { get; private set; }
        
        #region 私有欄位

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemyA";
        // Google Sheet API 網址，用於取得 EnemyData 原始列表

        #endregion
        
        /// <summary>
        /// Inspector 按鈕：從 Google Sheet 載入 EnemyData 列表
        /// </summary>
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemyData>(url, infos =>
            {
                // 過濾掉 ID 欄位為空的列
                var enemyInfos = infos
                    .Where(info => !info.ID.IsNullOrWhitespace())
                    .ToArray();
                Debug.Log($"EnemyData Count: {enemyInfos.Length}");
                
                // 清空現有索引
                ids = new List<EnemyData>();
                
                foreach (var enemyInfo in enemyInfos)
                {
                    // 設定在 DataOverviewBase 中使用的唯一識別與顯示名稱
                    enemyInfo.SetDataId($"{enemyInfo.ID}");
                    enemyInfo.SetDisplayName($"{enemyInfo.Lang}/{enemyInfo.Level}");
                    
                    // 解析原始逗號分隔字串為清單
                    enemyInfo.enemyIntentionIDs = Helper.ConvertStringToStringList(enemyInfo.EnemyIntentionID);
                    enemyInfo.startBattleIntentionIDs = Helper.ConvertStringToStringList(enemyInfo.StartBattleIntentionID);
                    
                    // 加入到總覽列表與字典
                    AddData(enemyInfo);
                }

                IsLoading = false;
            });
        }
    }
}
