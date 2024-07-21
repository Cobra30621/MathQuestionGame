using System;
using System.Collections.Generic;
using System.Linq;
using NueGames.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities;

namespace Card
{
    [CreateAssetMenu(fileName = "CardLevelData",menuName = "CardLevelData",order = 0)]
    public class CardLevelData :  ScriptableObject
    {
        #region Private Variables

        [SerializeField]
        [LabelWidth(30)]
        [LabelText("Url:")]
        [BoxGroup("LoadData")]
        private string url = "https://opensheet.elk.sh/17o-e5oCXd3G-jgaeQcWVH2am7DFnWY5afiKsLWWvOQs/CardLevelData_dev";

        [SerializeField]
        [TableList]
        private CardLevelInfo[] cardInfos;

        #endregion
        
        
        #region Public Methods

        public List<CardLevelInfo> GetAllCardInfo()
        {
            return cardInfos.ToList();
        }

        #endregion

        #region Private Methods

        [Button]
        [BoxGroup("LoadData")]
        private void ParseDataFromGoogleSheet()
        {
            GoogleSheetService.LoadDataArray<CardLevelInfo>(url , infos => cardInfos = infos);
        }

        #endregion
    }
    
    [Serializable]
    public class CardLevelInfo
    {
        public int ID;
        public string GroupID;
        public string SkillID;
        public int ManaCost;
        public int UpgradeCost;
        public AllyClassType Class;
        public bool MaxLevel;
        public string TitleLang;
        public string DesLang;
        public ActionTargetType ActionTargetType => EffectInfos[0].Target; // follow first effect
        public List<SkillInfo> EffectInfos;

        public void SetEffect(List<SkillInfo> effectInfos)
        {
            EffectInfos = effectInfos;
        }
    }

}