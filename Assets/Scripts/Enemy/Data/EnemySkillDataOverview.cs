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

        public EnemySkillData[] EnemySkills { get; private set; }

        public Dictionary<string, EnemySkillData> dict;


        #region Private Variables

        [SerializeField] [LabelWidth(30)] [LabelText("Url:")] [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/1tCaaCrB-9xcP_sgVigBUC5hSNOEx9KzDwsomPIC7asU/EnemySkillA";

        #endregion

        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<EnemySkillData>(url, infos =>
            {
                Debug.Log($"EnemySkillData Count: {infos.Length}");

                EnemySkills = infos;

                foreach (var skillData in infos)
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