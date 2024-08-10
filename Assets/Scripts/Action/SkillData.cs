using System;
using System.Collections.Generic;
using System.Linq;
using Action;
using NueGames.Enums;
using rStarTools.Scripts.ScriptableObjects.DataOverviews;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using Tool;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
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

        public List<SkillInfo> GetSkillInfos()
        {
            return skillInfos.ToList();
        }
        
       
        public SkillInfo GetSkillInfo(string id)
        {
            if (dict.TryGetValue(id, out var value))
            {
                return value;
            }
            else
            {
                Debug.LogError($"No SkillInfo found for ID : {id}");
                return null;
            }
        }
        
        #endregion

    
        [Button]
        [BoxGroup("LoadData")]
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<SkillInfo>(url , infos =>
            {
                skillInfos = infos;
                
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
        public int SkillID;
        public GameActionType EffectID;
        public string EffectParameter;
        public List <int> EffectParameterList; 
        public ActionTargetType Target;
        public string ps;
    }
}