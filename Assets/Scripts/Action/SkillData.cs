using System;
using System.Collections.Generic;
using System.Linq;
using Action;
using NueGames.Enums;
using rStarTools.Scripts.ScriptableObjects.DataOverviews;
using rStarTools.Scripts.StringList;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
{
    [CreateAssetMenu(fileName = "SkillData",menuName = "SkillData",order = 0)]
    public class SkillData : DataOverviewBase<SkillData, SkillInfo>
    {
        
        public bool IsLoading { get; private set; }
        
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
        public void ParseDataFromGoogleSheet()
        {
            IsLoading = true;
            GoogleSheetService.LoadDataArray<SkillInfo>(url , infos =>
            {
                ids = new List<SkillInfo>();
                Debug.Log($"{infos.Length}");
            
                foreach (var info in infos)
                {
                    info.SetDisplayName($"{info.SkillID}");
                    info.SetDataId($"{info.SkillID}");
                    AddData(info);
                }
                IsLoading = false;
            });
        }

        #endregion
    }
    
    
    [Serializable]
    public class SkillInfo : DataBase<SkillData>
    {
        public int SkillID;
        public GameActionType EffectID;
        public string EffectParameter;
        public List <int> EffectParameterList; 
        public ActionTargetType Target;
        public string ps;
    }
}