using System.Collections.Generic;
using Enemy.Data;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
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
                ids = new List<EnemyData>();
                Debug.Log($"{infos.Length}");
            
                foreach (var enemyInfo in infos)
                {
                    // enemyInfo.SetDisplayName($"{enemyInfo.ID}");
                    enemyInfo.SetDataId($"{enemyInfo.ID}");
                    enemyInfo.SetDisplayName($"{enemyInfo.Lang}-{enemyInfo.Level}");
                    enemyInfo.enemySkillIDs = Helper.ConvertStringToStringList(enemyInfo.EnemySkillID);
                    
                    AddData(enemyInfo);
                }

                IsLoading = false;
            });
            
        }
        
    }
}