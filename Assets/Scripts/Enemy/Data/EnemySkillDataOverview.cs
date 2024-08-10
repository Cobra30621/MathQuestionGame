using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;
using Utilities;

namespace Enemy.Data
{
    public class EnemySkillDataOverview : SerializedScriptableObject
    {
        public bool IsLoading { get; private set; }

        public Data.EnemySkillData[] EnemyActions { get; private set; }

        public Dictionary<string, Data.EnemySkillData> dict;
        
        
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
            GoogleSheetService.LoadDataArray<Data.EnemySkillData>(url , infos =>
            {
                Debug.Log($"{infos.Length}");

                EnemyActions = infos;
                
                foreach (var skillData in infos)
                {
                    skillData.skillIDs = Helper.ConvertStringToStringList(skillData.SkillID);
                }
                
                BuildDict();

                IsLoading = false;
            });
        }

        public Data.EnemySkillData GetEnemyAction(string id)
        {
            if (dict.TryGetValue(id, out var action))
            {
                return action;
            }

            Debug.LogError($"Can't find EnemyAction: {id}");
            return null;
        }

        public void BuildDict()
        {
            dict = new Dictionary<string, Data.EnemySkillData>();
            foreach (var enemyAction in EnemyActions)
            {
                if (dict.ContainsKey(enemyAction.ID))
                {
                    Debug.LogError($"Duplicate ID: {enemyAction.ID}");
                }
                else
                {
                    dict.Add(enemyAction.ID, enemyAction);
                }
            }
        }
    }
    
    
    
}