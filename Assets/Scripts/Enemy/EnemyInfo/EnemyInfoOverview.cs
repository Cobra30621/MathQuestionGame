using System.Collections.Generic;
using Enemy.EnemySkillInfo;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;
using Utilities;

namespace Enemy
{
    [CreateAssetMenu] 
    public class EnemyInfoOverview : DataOverviewBase<EnemyInfoOverview, EnemyInfo.EnemyInfo>
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
            GoogleSheetService.LoadDataArray<EnemyInfo.EnemyInfo>(url , infos =>
            {
                ids = new List<EnemyInfo.EnemyInfo>();
                Debug.Log($"{infos.Length}");
            
                foreach (var enemyInfo in infos)
                {
                    // enemyInfo.SetDisplayName($"{enemyInfo.ID}");
                    enemyInfo.SetDataId($"{enemyInfo.ID}");
                    enemyInfo.SetDisplayName($"{enemyInfo.Lang}-{enemyInfo.Level}");
                    AddData(enemyInfo);
                }

                IsLoading = false;
            });
            
        }
        
        public void SetEnemySkillInfo(EnemyActionOverview enemyActionOverview)
        {
            foreach (var enemyInfo in ids)
            {
                var stringToList = Helper.ConvertStringToList(enemyInfo.MobSkillID);

                enemyInfo.EnemySkillInfos = new List<EnemySkillInfo.EnemyAction>();
                foreach (var id in stringToList)
                {
                    var enemySkillInfo = enemyActionOverview.FindUniqueId($"{id}");
                    if (enemySkillInfo == null)
                    {
                        Debug.LogError($"找不到敵人 {enemyInfo.ID} 的 {nameof(EnemyAction)} id {id}\n請去 Google 表單確認");
                    }
                    else
                    {
                        enemyInfo.EnemySkillInfos.Add(enemySkillInfo);
                    }
                    
                    
                }
            }
        }
    }
}