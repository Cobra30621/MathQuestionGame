using System;
using System.Collections.Generic;
using System.Linq;
using Action;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
{
    [CreateAssetMenu(fileName = "SkillData",menuName = "SkillData",order = 0)]
    public class SkillData : ScriptableObject
    {
        #region Private Variables

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/SkillData_dev";

        [FormerlySerializedAs("effectInfos")]
        [SerializeField]
        [TableList]
        private SkillInfo[] skillInfos;

        #endregion
        
        
        #region Public Methods

        public List<SkillInfo> GetSkillInfos()
        {
            return skillInfos.ToList();
        }

        #endregion

        #region Private Methods

        [Button]
        [BoxGroup("LoadData")]
        private void ParseDataFromGoogleSheet()
        {
            GoogleSheetService.LoadDataArray<SkillInfo>(url , infos => skillInfos = infos);
        }

        #endregion
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