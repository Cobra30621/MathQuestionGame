using System;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Effect.Parameters;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Tool;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Effect
{
    [CreateAssetMenu(fileName = "SkillData",menuName = "SkillData",order = 0)]
    public class SkillData : SerializedScriptableObject
    {
        public bool IsLoading { get; private set; }
        
        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/SkillData_dev";

        [FormerlySerializedAs("effectInfos")]
        [SerializeField]
        [TableList]
        private SkillInfo[] skillInfos;


        public Dictionary<string, SkillInfo> dict;
        
        #region GetSkillInfo

        public List<SkillInfo> GetAllSkillInfos()
        {
            return skillInfos.ToList();
        }
        
       
        public SkillInfo GetSkillInfo(string id, string whoFinding = "")
        {
            if (dict.TryGetValue(id, out var value))
            {
                return value;
            }
            else
            {
                Debug.LogError($"{whoFinding} can't find SkillInfo: '{id}'");
                return null;
            }
        }
        
        #endregion

    
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            Debug.Log($"Url {url}");
            IsLoading = true;
            GoogleSheetService.LoadDataArray<SkillInfo>(url , infos =>
            {
                // 將空行列剔除
                skillInfos = infos.Where(skill => 
                    !skill.SkillID.IsNullOrWhitespace()).ToArray();
                Debug.Log($"SkillInfos Count: {skillInfos.Length}");
                
                foreach (var skillInfo in skillInfos)
                {
                    var effectParameterList = Helper.ConvertStringToIntList(skillInfo.EffectParameter);
                    skillInfo.EffectParameterList = effectParameterList;
                }
                
                BuildDict();
                IsLoading = false;
            });
        }

        private void BuildDict()
        {
            dict = new Dictionary<string, SkillInfo>();
            foreach (var info in skillInfos)
            {
                if (dict.ContainsKey(info.SkillID.ToString()))
                    Debug.LogError($"Duplicate SkillID : {info.SkillID}");
                else
                    dict.Add(info.SkillID.ToString(), info);
            }
        }
    }
    
    
    [Serializable]
    public class SkillInfo 
    {
        public string SkillID;
        public EffectName EffectID;
        public string EffectParameter;
        public List <int> EffectParameterList; 
        public ActionTargetType Target;
        public string ps;

        public override string ToString()
        {
            return
                $"{ps} | {EffectID} : {EffectParameter}, Target:{Target} SkillId : {SkillID}";
        }
    }
}