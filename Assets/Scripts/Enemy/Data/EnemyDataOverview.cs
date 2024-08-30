using System.Collections.Generic;
using System.Linq;
using Enemy.Data;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using Utilities;

namespace Enemy
{
    [CreateAssetMenu] 
    public class EnemyDataOverview : DataOverviewBase<EnemyDataOverview, EnemyData>
    {
        
        public bool IsLoading { get; private set; }
        
        #region Private Variables

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemyA";

   
        #endregion
        
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemyData>(url , infos =>
            {
                // 將空行列剔除
                var enemyInfos = infos.Where(info => 
                    !info.ID.IsNullOrWhitespace()).ToArray();
                Debug.Log($"EnemyData Count: {enemyInfos.Length}");
            
                ids = new List<EnemyData>();
                foreach (var enemyInfo in enemyInfos)
                {
                    enemyInfo.SetDataId($"{enemyInfo.ID}");
                    enemyInfo.SetDisplayName($"{enemyInfo.Lang}/{enemyInfo.Level}");
                    enemyInfo.enemySkillIDs = Helper.ConvertStringToStringList(enemyInfo.EnemySkillID);
                    
                    AddData(enemyInfo);
                }

                IsLoading = false;
            });
            
        }
        
    }
}