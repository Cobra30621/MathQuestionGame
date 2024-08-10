using System.Collections.Generic;
using Card;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;
using Utilities;

namespace Enemy.EnemySkillInfo
{
    public class EnemyActionOverview : DataOverviewBase<EnemyActionOverview, EnemyAction>
    {
        public bool IsLoading { get; private set; }
        
        #region Private Variables

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemySkillA";

   
        #endregion
        
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemyAction>(url , infos =>
            {
                ids = new List<EnemyAction>();
                Debug.Log($"{infos.Length}");
            
                foreach (var enemyInfo in infos)
                {
                    enemyInfo.SetDisplayName($"{enemyInfo.ID}");
                    enemyInfo.SetDataId($"{enemyInfo.ID}");
                    AddData(enemyInfo);
                }

                IsLoading = false;
            });
        }

        public void SetSkillInfo(SkillData skillData)
        {
            foreach (var enemyAction in ids)
            {
                var stringToList = Helper.ConvertStringToList(enemyAction.SkillID);

                enemyAction.skillInfos = new List<SkillInfo>();
                foreach (var id in stringToList)
                {
                    var enemySkillInfo = skillData.FindUniqueId($"{id}");
                    if (enemySkillInfo == null)
                    {
                        Debug.LogError($"找不到敵人 {enemyAction.ID} 的 {nameof(EnemyAction)} id {id}\n請去 Google 表單確認");
                    }
                    else
                    {
                        enemyAction.skillInfos.Add(enemySkillInfo);
                    }
                    
                    
                }
            }
        }

    }
}