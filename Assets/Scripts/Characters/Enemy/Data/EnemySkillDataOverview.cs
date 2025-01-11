using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using Utilities;

namespace Characters.Enemy.Data
{
    public class EnemySkillDataOverview : SerializedScriptableObject
    {
        public bool IsLoading { get; private set; }

        public EnemySkillData[] EnemySkills { get; private set; }

        public Dictionary<string, EnemySkillData> dict;


        #region Private Variables

        [SerializeField] [LabelWidth(30)] [LabelText("Url:")] [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemyIntentionA";

        #endregion

        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemySkillData>(url, infos =>
            {
                // 將空行列剔除
                EnemySkills = infos.Where(info => 
                    !info.SkillID.IsNullOrWhitespace()).ToArray();;
                Debug.Log($"EnemySkillData Count: {EnemySkills.Length}");

                foreach (var skillData in EnemySkills)
                {
                    skillData.skillIDs = Helper.ConvertStringToStringList(skillData.SkillID);
                }

                BuildDict();

                IsLoading = false;
            });
        }

        public EnemySkillData GetEnemySkillData(string id, string whoFinding = "")
        {
            if (dict.TryGetValue(id, out var action))
            {
                return action;
            }

            Debug.LogError($"{whoFinding} can't find EnemySkillData: '{id}'");
            return null;
        }

        public void BuildDict()
        {
            dict = new Dictionary<string, EnemySkillData>();
            foreach (var enemyAction   in EnemySkills)
            {
                if (dict.ContainsKey(enemyAction.SkillID))
                {
                    Debug.LogError($"Duplicate ID: {enemyAction.SkillID}");
                }
                else
                {
                    dict.Add(enemyAction.SkillID, enemyAction);
                }
            }
        }
    }
}